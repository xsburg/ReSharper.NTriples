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
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util.Caches;
using JetBrains.Util;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Cache
{
    [PsiComponent]
    public class NTriplesCache : ICache
    {
        private const int Version = 8;
        private readonly JetHashSet<IPsiSourceFile> myDirtyFiles = new JetHashSet<IPsiSourceFile>();

        private readonly OneToSetMap<string, NTriplesPrefixDeclarationSymbol> myNameToSymbolsPrefixDeclarationMap =
            new OneToSetMap<string, NTriplesPrefixDeclarationSymbol>();

        private readonly OneToSetMap<string, NTriplesUriIdentifierSymbol> myNameToSymbolsUriIdentifierMap =
            new OneToSetMap<string, NTriplesUriIdentifierSymbol>();

        private readonly IPersistentIndexManager myPersistentIdIndex;

        private readonly OneToListMap<IPsiSourceFile, NTriplesPrefixDeclarationSymbol> myProjectFileToSymbolsPrefixDeclarationMap =
            new OneToListMap<IPsiSourceFile, NTriplesPrefixDeclarationSymbol>();

        private readonly OneToListMap<IPsiSourceFile, NTriplesUriIdentifierSymbol> myProjectFileToSymbolsUriIdentifierMap =
            new OneToListMap<IPsiSourceFile, NTriplesUriIdentifierSymbol>();

        private readonly IPsiConfiguration myPsiConfiguration;
        private readonly IShellLocks myShellLocks;
        private SecretPersistentCache<NTriplesFileCache> myPersistentCache;

        public NTriplesCache(
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

        public IEnumerable<NTriplesUriIdentifierSymbol> GetAllUriIdentifierSymbols()
        {
            return this.myNameToSymbolsUriIdentifierMap.SelectMany(x => x.Value);
        }

        public IEnumerable<NTriplesUriIdentifierSymbol> GetAllUriIdentifiersInNamespace(string @namespace)
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

        public OneToListMap<IPsiSourceFile, NTriplesUriIdentifierSymbol> GetAllUriIdentifierSymbolsByFile()
        {
            return this.myProjectFileToSymbolsUriIdentifierMap;
        }

        public IEnumerable<NTriplesPrefixDeclarationSymbol> GetPrefixDeclarationSymbols(string name)
        {
            return this.myNameToSymbolsPrefixDeclarationMap.SelectMany(x => x.Value).Where(s => s.Name == name);
        }

        public IEnumerable<NTriplesPrefixDeclarationSymbol> GetAllPrefixDeclarationSymbols()
        {
            return this.myNameToSymbolsPrefixDeclarationMap.SelectMany(x => x.Value);
        }

        public IEnumerable<INTriplesSymbol> GetUriIdentifierSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
        {
            var symbols = this.myProjectFileToSymbolsUriIdentifierMap.GetValuesCollection(sourceFile);
            return symbols;
        }
        
        public IEnumerable<IUriIdentifierDeclaredElement> GetImportantSubjects()
        {
            bool foundImportant = false;
            foreach (var pair in this.myProjectFileToSymbolsUriIdentifierMap)
            {
                var sourceFile = pair.Key;
                foreach (var symbol in pair.Value)
                {
                    var uriIdentifier = GetUriIdentifier(sourceFile, symbol);
                    if (uriIdentifier == null)
                    {
                        continue;
                    }

                    var declaredElement = uriIdentifier.DescendantDeclaredElement;
                    if (declaredElement == null || !NTriplesIdentifierFilter.IsImportantSubject(declaredElement))
                    {
                        continue;
                    }

                    foundImportant = true;
                    yield return declaredElement;
                }
            }

            // Fallback to return subjects
            if (!foundImportant)
            {
                foreach (var pair in this.myProjectFileToSymbolsUriIdentifierMap)
                {
                    var sourceFile = pair.Key;
                    foreach (var symbol in pair.Value)
                    {
                        var uriIdentifier = GetUriIdentifier(sourceFile, symbol);
                        if (uriIdentifier == null)
                        {
                            continue;
                        }

                        var declaredElement = uriIdentifier.DescendantDeclaredElement;
                        if (declaredElement == null)
                        {
                            continue;
                        }

                        yield return declaredElement;
                    }
                }
            }
        }

        private static IUriIdentifier GetUriIdentifier(IPsiSourceFile sourceFile, NTriplesUriIdentifierSymbol symbol)
        {
            var file = sourceFile.GetPsiFile<NTriplesLanguage>(new DocumentRange(sourceFile.Document, 0));
            if (file == null)
            {
                return null;
            }

            var treeNode = file.FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
            if (treeNode == null)
            {
                return null;
            }

            var uriIdentifier = treeNode.GetContainingNode<IUriIdentifier>();
            return uriIdentifier;
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
                this.myPersistentCache = new SecretPersistentCache<NTriplesFileCache>(
                    this.myShellLocks, Version, "SecretCache", this.myPsiConfiguration);
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

        public void Merge(IPsiSourceFile sourceFile, object builtPart)
        {
            this.myShellLocks.AssertWriteAccessAllowed();

            var data = builtPart as IList<INTriplesSymbol>;

            if (data != null)
            {
                var uriIdentifierData = new List<NTriplesUriIdentifierSymbol>();
                var prefixDeclarationData = new List<NTriplesPrefixDeclarationSymbol>();
                foreach (INTriplesSymbol symbol in data)
                {
                    var uriIdentifierSymbol = symbol as NTriplesUriIdentifierSymbol;
                    if (uriIdentifierSymbol != null)
                    {
                        uriIdentifierData.Add(uriIdentifierSymbol);
                        continue;
                    }

                    var prefixDeclarationSymbol = symbol as NTriplesPrefixDeclarationSymbol;
                    if (prefixDeclarationSymbol != null)
                    {
                        prefixDeclarationData.Add(prefixDeclarationSymbol);
                    }
                }

                if (this.myPersistentCache != null)
                {
                    this.myPersistentCache.AddDataToSave(sourceFile, new NTriplesFileCache(uriIdentifierData));
                }

                // clear old declarations cache...
                // uri identifiers
                if (this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(sourceFile))
                {
                    foreach (NTriplesUriIdentifierSymbol oldDeclaration in this.myProjectFileToSymbolsUriIdentifierMap[sourceFile])
                    {
                        string oldName = oldDeclaration.Name;
                        this.myNameToSymbolsUriIdentifierMap.Remove(oldName, oldDeclaration);
                    }
                }
                // prefix declarations
                if (this.myProjectFileToSymbolsPrefixDeclarationMap.ContainsKey(sourceFile))
                {
                    foreach (
                        NTriplesPrefixDeclarationSymbol oldDeclaration in
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
                foreach (NTriplesUriIdentifierSymbol declaration in uriIdentifierData)
                {
                    this.myNameToSymbolsUriIdentifierMap.Add(declaration.Name, declaration);
                }

                this.myProjectFileToSymbolsPrefixDeclarationMap.AddValueRange(sourceFile, prefixDeclarationData);
                foreach (NTriplesPrefixDeclarationSymbol declaration in prefixDeclarationData)
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
            if (this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(sourceFile))
            {
                foreach (NTriplesUriIdentifierSymbol oldDeclaration in this.myProjectFileToSymbolsUriIdentifierMap[sourceFile])
                {
                    string oldName = oldDeclaration.Name;
                    this.myNameToSymbolsUriIdentifierMap.Remove(oldName, oldDeclaration);
                }
            }

            if (this.myProjectFileToSymbolsPrefixDeclarationMap.ContainsKey(sourceFile))
            {
                foreach (
                    NTriplesPrefixDeclarationSymbol oldDeclaration in this.myProjectFileToSymbolsPrefixDeclarationMap[sourceFile])
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
            return sourceFile.GetAllPossiblePsiLanguages().Any(x => x.Is<NTriplesLanguage>());
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
