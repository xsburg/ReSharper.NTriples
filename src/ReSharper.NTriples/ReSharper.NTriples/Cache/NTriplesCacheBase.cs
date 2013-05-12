// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCacheBase.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.Util;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Cache
{
    public abstract class NTriplesCacheBase : ICache
    {
        protected const int Version = 9;
        private readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();
        private readonly IPersistentIndexManager myPersistentIdIndex;
        private readonly IPsiConfiguration myPsiConfiguration;
        private readonly IShellLocks myShellLocks;
        private NTriplesPersistentCache<NTriplesFileCache> myPersistentCache;

        protected NTriplesCacheBase(
            Lifetime lifetime,
            IPsiServices psiServices,
            IShellLocks shellLocks,
            CacheManager cacheManager,
            IPsiConfiguration psiConfiguration,
            IPersistentIndexManager persistentIdIndex)
        {
            this.myPsiConfiguration = psiConfiguration;
            this.myPersistentIdIndex = persistentIdIndex;
            this.myShellLocks = shellLocks;

            //using (ReadLockCookie.Create())
            //{
            lifetime.AddBracket(() => cacheManager.RegisterCache(this), () => cacheManager.UnregisterCache(this));
            //}
        }

        public bool HasDirtyFiles
        {
            get
            {
                return !this.myDirtyFiles.IsEmpty();
            }
        }

        public object Build(IPsiSourceFile sourceFile, bool isStartup)
        {
            return NTriplesCacheBuilder.Build(sourceFile);
        }

        public object Build(IPsiAssembly assembly)
        {
            return null;
        }

        public object Load(IProgressIndicator progress, bool enablePersistence)
        {
            if (!enablePersistence)
            {
                return null;
            }

            Assertion.Assert(this.myPersistentCache == null, "myPersistentCache == null");

            using (ReadLockCookie.Create())
            {
                this.myPersistentCache = new NTriplesPersistentCache<NTriplesFileCache>(
                    this.myShellLocks, Version, "NTriplesCache", this.myPsiConfiguration);
            }

            var data = new Dictionary<IPsiSourceFile, NTriplesFileCache>();

            if (this.myPersistentCache.Load(
                progress,
                this.myPersistentIdIndex,
                (file, reader) =>
                {
                    using (ReadLockCookie.Create())
                    {
                        return NTriplesCacheBuilder.Read(reader, file);
                    }
                },
                (projectFile, psiSymbols) =>
                {
                    if (projectFile != null)
                    {
                        data[projectFile] = psiSymbols;
                    }
                }) != LoadResult.OK)
            {
                // clear all...
                ((ICache)this).Release();
                return null;
            }
            return data;
        }

        public void MarkAsDirty(IPsiSourceFile sourceFile)
        {
            if (Accepts(sourceFile))
            {
                this.myDirtyFiles.Add(sourceFile);
            }
        }

        public void Merge(IPsiAssembly assembly, object part)
        {
        }

        public void Merge(IPsiSourceFile sourceFile, object builtPart)
        {
            this.myShellLocks.AssertWriteAccessAllowed();

            var data = builtPart as IList<INTriplesSymbol>;

            if (data != null)
            {
                this.ClearCache(sourceFile);
                var fileCache = this.ComputeCache(sourceFile, data);

                if (this.myPersistentCache != null)
                {
                    this.myPersistentCache.AddDataToSave(sourceFile, fileCache);
                }

                this.myDirtyFiles.Remove(sourceFile);
            }
        }

        public void MergeLoaded(object data)
        {
            var parts = (Dictionary<IPsiSourceFile, NTriplesFileCache>)data;
            foreach (var pair in parts)
            {
                if (pair.Key.IsValid() && !this.myDirtyFiles.Contains(pair.Key))
                {
                    ((ICache)this).Merge(pair.Key, pair.Value);
                }
            }
        }

        public void OnAssemblyRemoved(IPsiAssembly assembly)
        {
        }

        public void OnDocumentChange(ProjectFileDocumentCopyChange args)
        {
            foreach (IPsiSourceFile sourceFile in args.ProjectFile.ToSourceFiles())
            {
                this.myShellLocks.AssertWriteAccessAllowed();
                if (Accepts(sourceFile))
                {
                    this.myShellLocks.AssertWriteAccessAllowed();
                    this.myDirtyFiles.Add(sourceFile);
                }
            }
        }

        public void OnFileRemoved(IPsiSourceFile sourceFile)
        {
            this.myShellLocks.AssertWriteAccessAllowed();

            this.myDirtyFiles.Remove(sourceFile);
            this.ClearCache(sourceFile);

            if (this.myPersistentCache != null)
            {
                this.myPersistentCache.MarkDataToDelete(sourceFile);
            }
        }

        public IEnumerable<IPsiSourceFile> OnProjectModelChange(ProjectModelChange change)
        {
            return EmptyList<IPsiSourceFile>.InstanceList;
        }

        public void OnPsiChange(ITreeNode elementContainingChanges, PsiChangedElementType type)
        {
            if (elementContainingChanges != null)
            {
                this.myShellLocks.AssertWriteAccessAllowed();
                IPsiSourceFile projectFile = elementContainingChanges.GetSourceFile();
                if (projectFile != null && Accepts(projectFile))
                {
                    this.myDirtyFiles.Add(projectFile);
                }
            }
        }

        public IEnumerable<IPsiSourceFile> OnPsiModulePropertiesChange(IPsiModule module)
        {
            return EmptyList<IPsiSourceFile>.InstanceList;
        }

        public void OnSandBoxCreated(SandBox sandBox)
        {
        }

        public void OnSandBoxPsiChange(ITreeNode elementContainingChanges)
        {
        }

        public void Release()
        {
        }

        public void Save(IProgressIndicator progress, bool enablePersistence)
        {
            if (!enablePersistence)
            {
                return;
            }

            Assertion.Assert(this.myPersistentCache != null, "myPersistentCache != null");
            this.myPersistentCache.Save(
                progress,
                this.myPersistentIdIndex,
                (writer, file, data) =>
                NTriplesCacheBuilder.Write(data, writer));
            this.myPersistentCache.Dispose();
            this.myPersistentCache = null;
        }

        public void SyncUpdate(bool underTransaction)
        {
            this.myShellLocks.AssertReadAccessAllowed();

            if (this.myDirtyFiles.Count > 0)
            {
                foreach (IPsiSourceFile projectFile in new List<IPsiSourceFile>(this.myDirtyFiles))
                {
                    using (WriteLockCookie.Create())
                    {
                        ICollection<INTriplesSymbol> ret = NTriplesCacheBuilder.Build(projectFile);
                        ((ICache)this).Merge(
                            projectFile,
                            ret != null
                                ? ret.ToList()
                                : null);
                    }
                }
            }
        }

        public bool UpToDate(IPsiSourceFile sourceFile)
        {
            this.myShellLocks.AssertReadAccessAllowed();

            if (!Accepts(sourceFile))
            {
                return true;
            }

            return !this.myDirtyFiles.Contains(sourceFile) && this.SymbolsUpToDate(sourceFile);
        }

        protected static bool Accepts(IPsiSourceFile sourceFile)
        {
            return sourceFile.GetAllPossiblePsiLanguages().Any(x => x.Is<NTriplesLanguage>());
        }

        protected abstract void ClearCache(IPsiSourceFile sourceFile);

        protected abstract NTriplesFileCache ComputeCache(IPsiSourceFile sourceFile, IList<INTriplesSymbol> data);
        protected abstract bool SymbolsUpToDate(IPsiSourceFile psiSourceFile);

        private class NTriplesPersistentCache<T> : SimplePersistentCache<T>
        {
            public NTriplesPersistentCache(
                IShellLocks locks, int formatVersion, string cacheDirectoryName, IPsiConfiguration psiConfiguration)
                : base(locks, formatVersion, cacheDirectoryName, psiConfiguration)
            {
            }

            protected override string LoadSaveProgressText
            {
                get
                {
                    return "N-Triples Caches";
                }
            }
        }
    }
}
