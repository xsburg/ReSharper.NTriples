// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCache.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    [PsiComponent]
    public class SecretCache : ICache
    {
        private const int Version = 8;
        private readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();

        private readonly OneToSetMap<string, SecretPrefixDeclarationSymbol> myNameToSymbolsPrefixDeclarationMap =
            new OneToSetMap<string, SecretPrefixDeclarationSymbol>();

        private readonly OneToSetMap<string, SecretUriIdentifierSymbol> myNameToSymbolsUriIdentifierMap =
            new OneToSetMap<string, SecretUriIdentifierSymbol>();

        private readonly IPersistentIndexManager myPersistentIdIndex;

        private readonly OneToListMap<IPsiSourceFile, SecretPrefixDeclarationSymbol> myProjectFileToSymbolsPrefixDeclarationMap =
            new OneToListMap<IPsiSourceFile, SecretPrefixDeclarationSymbol>();

        private readonly OneToListMap<IPsiSourceFile, SecretUriIdentifierSymbol> myProjectFileToSymbolsUriIdentifierMap =
            new OneToListMap<IPsiSourceFile, SecretUriIdentifierSymbol>();

        private readonly IPsiConfiguration myPsiConfiguration;
        private readonly IShellLocks myShellLocks;
        private SecretPersistentCache<SecretFileCache> myPersistentCache;

        public SecretCache(
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
            return SecretCacheBuilder.Build(sourceFile);
        }

        public object Build(IPsiAssembly assembly)
        {
            return null;
        }

        public IEnumerable<SecretUriIdentifierSymbol> GetAllUriIdentifierSymbols()
        {
            return this.myNameToSymbolsUriIdentifierMap.SelectMany(x => x.Value);
        }

        public IEnumerable<SecretUriIdentifierSymbol> GetAllUriIdentifiersInNamespace(string @namespace)
        {
            @namespace = FixNamespace(@namespace);
            return this.myNameToSymbolsUriIdentifierMap.SelectMany(x => x.Value).Where(s => s.Namespace == @namespace);
        }

        public IList<IPsiSourceFile> GetFilesContainingUri(string @namespace, string localName)
        {
            @namespace = FixNamespace(@namespace);
            var result = this.myProjectFileToSymbolsUriIdentifierMap
                             .Where(pair => pair.Value.Any(s => s.Namespace == @namespace && s.LocalName == localName))
                             .Select(pair => pair.Key)
                             .ToList();
            return result;
        }

        public OneToListMap<IPsiSourceFile, SecretUriIdentifierSymbol> GetAllUriIdentifierSymbolsByFile()
        {
            return this.myProjectFileToSymbolsUriIdentifierMap;
        }

        public IEnumerable<SecretPrefixDeclarationSymbol> GetPrefixDeclarationSymbols(string name)
        {
            return this.myNameToSymbolsPrefixDeclarationMap.SelectMany(x => x.Value).Where(s => s.Name == name);
        }

        public IEnumerable<SecretPrefixDeclarationSymbol> GetAllPrefixDeclarationSymbols()
        {
            return this.myNameToSymbolsPrefixDeclarationMap.SelectMany(x => x.Value);
        }

        public IEnumerable<ISecretSymbol> GetUriIdentifierSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
        {
            var symbols = this.myProjectFileToSymbolsUriIdentifierMap.GetValuesCollection(sourceFile);
            return symbols;
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
                this.myPersistentCache = new SecretPersistentCache<SecretFileCache>(
                    this.myShellLocks, Version, "SecretCache", this.myPsiConfiguration);
            }

            var data = new Dictionary<IPsiSourceFile, SecretFileCache>();

            if (this.myPersistentCache.Load(
                progress,
                this.myPersistentIdIndex,
                (file, reader) =>
                {
                    using (ReadLockCookie.Create())
                    {
                        return SecretCacheBuilder.Read(reader, file);
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

        public void Merge(IPsiSourceFile sourceFile, object builtPart)
        {
            this.myShellLocks.AssertWriteAccessAllowed();

            var data = builtPart as IList<ISecretSymbol>;

            if (data != null)
            {
                var uriIdentifierData = new List<SecretUriIdentifierSymbol>();
                var prefixDeclarationData = new List<SecretPrefixDeclarationSymbol>();
                foreach (ISecretSymbol symbol in data)
                {
                    var uriIdentifierSymbol = symbol as SecretUriIdentifierSymbol;
                    if (uriIdentifierSymbol != null)
                    {
                        uriIdentifierData.Add(uriIdentifierSymbol);
                        continue;
                    }

                    var prefixDeclarationSymbol = symbol as SecretPrefixDeclarationSymbol;
                    if (prefixDeclarationSymbol != null)
                    {
                        prefixDeclarationData.Add(prefixDeclarationSymbol);
                    }
                }

                if (this.myPersistentCache != null)
                {
                    this.myPersistentCache.AddDataToSave(sourceFile, new SecretFileCache(uriIdentifierData));
                }

                // clear old declarations cache...
                // uri identifiers
                if (this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(sourceFile))
                {
                    foreach (SecretUriIdentifierSymbol oldDeclaration in this.myProjectFileToSymbolsUriIdentifierMap[sourceFile])
                    {
                        string oldName = oldDeclaration.Name;
                        this.myNameToSymbolsUriIdentifierMap.Remove(oldName, oldDeclaration);
                    }
                }
                // prefix declarations
                if (this.myProjectFileToSymbolsPrefixDeclarationMap.ContainsKey(sourceFile))
                {
                    foreach (
                        SecretPrefixDeclarationSymbol oldDeclaration in
                            this.myProjectFileToSymbolsPrefixDeclarationMap[sourceFile])
                    {
                        string oldName = oldDeclaration.Name;
                        this.myNameToSymbolsPrefixDeclarationMap.Remove(oldName, oldDeclaration);
                    }
                }

                this.myDirtyFiles.Remove(sourceFile);

                this.myProjectFileToSymbolsUriIdentifierMap.RemoveKey(sourceFile);
                this.myProjectFileToSymbolsPrefixDeclarationMap.RemoveKey(sourceFile);

                // add to projectFile to data map...
                this.myProjectFileToSymbolsUriIdentifierMap.AddValueRange(sourceFile, uriIdentifierData);
                foreach (SecretUriIdentifierSymbol declaration in uriIdentifierData)
                {
                    this.myNameToSymbolsUriIdentifierMap.Add(declaration.Name, declaration);
                }

                this.myProjectFileToSymbolsPrefixDeclarationMap.AddValueRange(sourceFile, prefixDeclarationData);
                foreach (SecretPrefixDeclarationSymbol declaration in prefixDeclarationData)
                {
                    this.myNameToSymbolsPrefixDeclarationMap.Add(declaration.Name, declaration);
                }
            }
        }

        public void Merge(IPsiAssembly assembly, object part)
        {
        }

        public void MergeLoaded(object data)
        {
            var parts = (Dictionary<IPsiSourceFile, SecretFileCache>)data;
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
            if (this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(sourceFile))
            {
                foreach (SecretUriIdentifierSymbol oldDeclaration in this.myProjectFileToSymbolsUriIdentifierMap[sourceFile])
                {
                    string oldName = oldDeclaration.Name;
                    this.myNameToSymbolsUriIdentifierMap.Remove(oldName, oldDeclaration);
                }
            }

            if (this.myProjectFileToSymbolsPrefixDeclarationMap.ContainsKey(sourceFile))
            {
                foreach (
                    SecretPrefixDeclarationSymbol oldDeclaration in this.myProjectFileToSymbolsPrefixDeclarationMap[sourceFile])
                {
                    string oldName = oldDeclaration.Name;
                    this.myNameToSymbolsPrefixDeclarationMap.Remove(oldName, oldDeclaration);
                }
            }

            this.myProjectFileToSymbolsUriIdentifierMap.RemoveKey(sourceFile);
            this.myProjectFileToSymbolsPrefixDeclarationMap.RemoveKey(sourceFile);

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
                SecretCacheBuilder.Write(data, writer));
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
                        ICollection<ISecretSymbol> ret = SecretCacheBuilder.Build(projectFile);
                        if (ret != null)
                        {
                            ((ICache)this).Merge(projectFile, ret.ToList());
                        }
                        else
                        {
                            ((ICache)this).Merge(projectFile, null);
                        }
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

            return !this.myDirtyFiles.Contains(sourceFile) &&
                   this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(sourceFile) &&
                   this.myProjectFileToSymbolsPrefixDeclarationMap.ContainsKey(sourceFile);
        }

        private static bool Accepts(IPsiSourceFile sourceFile)
        {
            return sourceFile.GetAllPossiblePsiLanguages().Any(x => x.Is<SecretLanguage>());
        }

        private static string FixNamespace(string @namespace)
        {
            return @namespace ?? "";
        }

        private class SecretPersistentCache<T> : SimplePersistentCache<T>
        {
            public SecretPersistentCache(
                IShellLocks locks, int formatVersion, string cacheDirectoryName, IPsiConfiguration psiConfiguration)
                : base(locks, formatVersion, cacheDirectoryName, psiConfiguration)
            {
            }

            protected override string LoadSaveProgressText
            {
                get
                {
                    return "The Secret Caches";
                }
            }
        }
    }
}
