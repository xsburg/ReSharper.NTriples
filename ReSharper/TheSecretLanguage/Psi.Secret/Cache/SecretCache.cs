// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiCache.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly OneToSetMap<string, SecretPrefixSymbol> myNameToSymbolsOptionMap =
            new OneToSetMap<string, SecretPrefixSymbol>();

        private readonly OneToSetMap<string, SecretRuleSymbol> myNameToSymbolsRuleMap =
            new OneToSetMap<string, SecretRuleSymbol>();

        private readonly IPersistentIndexManager myPersistentIdIndex;

        private readonly OneToListMap<IPsiSourceFile, SecretPrefixSymbol> myProjectFileToSymbolsOptionMap =
            new OneToListMap<IPsiSourceFile, SecretPrefixSymbol>();

        private readonly OneToListMap<IPsiSourceFile, SecretRuleSymbol> myProjectFileToSymbolsRuleMap =
            new OneToListMap<IPsiSourceFile, SecretRuleSymbol>();

        private readonly IPsiConfiguration myPsiConfiguration;
        private readonly IShellLocks myShellLocks;
        private PsiPersistentCache<CachePair> myPersistentCache;

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
            MessageBox.ShowInfo("Building cache!");
            return SecretCacheBuilder.Build(sourceFile);
        }

        public object Build(IPsiAssembly assembly)
        {
            return null;
        }

        public IEnumerable<SecretPrefixSymbol> GetOptionSymbols(string name)
        {
            return this.myNameToSymbolsOptionMap[name];
        }

        public IEnumerable<ISecretSymbol> GetSymbols(string name)
        {
            return this.myNameToSymbolsRuleMap[name];
        }

        public IEnumerable<ISecretSymbol> GetSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
        {
            var symbols = this.myProjectFileToSymbolsRuleMap.GetValuesCollection(sourceFile);
            return symbols;
        }

        public object Load(IProgressIndicator progress, bool enablePersistence)
        {
            MessageBox.ShowInfo("Building cache!");
            if (!enablePersistence)
            {
                return null;
            }

            Assertion.Assert(this.myPersistentCache == null, "myPersistentCache == null");

            using (ReadLockCookie.Create())
            {
                this.myPersistentCache = new PsiPersistentCache<CachePair>(
                    this.myShellLocks, Version, "SecretCache", this.myPsiConfiguration);
            }

            var data = new Dictionary<IPsiSourceFile, CachePair>();

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
                var ruleData = new List<SecretRuleSymbol>();
                var optionData = new List<SecretPrefixSymbol>();
                foreach (ISecretSymbol symbol in data)
                {
                    var psiRuleSymbol = symbol as SecretRuleSymbol;
                    if (psiRuleSymbol != null)
                    {
                        ruleData.Add(psiRuleSymbol);
                    }
                    var psiOptionSymbol = symbol as SecretPrefixSymbol;
                    if (psiOptionSymbol != null)
                    {
                        optionData.Add(psiOptionSymbol);
                    }
                }
                if (this.myPersistentCache != null)
                {
                    this.myPersistentCache.AddDataToSave(sourceFile, new CachePair(ruleData, optionData));
                }

                // clear old declarations cache...
                //rules
                if (this.myProjectFileToSymbolsRuleMap.ContainsKey(sourceFile))
                {
                    foreach (SecretRuleSymbol oldDeclaration in this.myProjectFileToSymbolsRuleMap[sourceFile])
                    {
                        string oldName = oldDeclaration.Name;
                        this.myNameToSymbolsRuleMap.Remove(oldName, oldDeclaration);
                    }
                }

                //option
                if (this.myProjectFileToSymbolsOptionMap.ContainsKey(sourceFile))
                {
                    foreach (SecretPrefixSymbol oldDeclaration in this.myProjectFileToSymbolsOptionMap[sourceFile])
                    {
                        string oldName = oldDeclaration.Name;
                        this.myNameToSymbolsOptionMap.Remove(oldName, oldDeclaration);
                    }
                }
                this.myDirtyFiles.Remove(sourceFile);

                this.myProjectFileToSymbolsRuleMap.RemoveKey(sourceFile);
                this.myProjectFileToSymbolsOptionMap.RemoveKey(sourceFile);

                // add to projectFile to data map...
                this.myProjectFileToSymbolsRuleMap.AddValueRange(sourceFile, ruleData);
                this.myProjectFileToSymbolsOptionMap.AddValueRange(sourceFile, optionData);
                foreach (SecretRuleSymbol declaration in ruleData)
                {
                    this.myNameToSymbolsRuleMap.Add(declaration.Name, declaration);
                }
                foreach (SecretPrefixSymbol declaration in optionData)
                {
                    this.myNameToSymbolsOptionMap.Add(declaration.Name, declaration);
                }
            }
        }

        public void Merge(IPsiAssembly assembly, object part)
        {
        }

        public void MergeLoaded(object data)
        {
            var parts = (Dictionary<IPsiSourceFile, CachePair>)data;
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
            if (this.myProjectFileToSymbolsRuleMap.ContainsKey(sourceFile))
            {
                foreach (SecretRuleSymbol oldDeclaration in this.myProjectFileToSymbolsRuleMap[sourceFile])
                {
                    string oldName = oldDeclaration.Name;
                    this.myNameToSymbolsRuleMap.Remove(oldName, oldDeclaration);
                }
            }
            if (this.myProjectFileToSymbolsOptionMap.ContainsKey(sourceFile))
            {
                foreach (SecretPrefixSymbol oldDeclaration in this.myProjectFileToSymbolsOptionMap[sourceFile])
                {
                    string oldName = oldDeclaration.Name;
                    this.myNameToSymbolsOptionMap.Remove(oldName, oldDeclaration);
                }
            }
            this.myProjectFileToSymbolsRuleMap.RemoveKey(sourceFile);
            this.myProjectFileToSymbolsOptionMap.RemoveKey(sourceFile);
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
            return !this.myDirtyFiles.Contains(sourceFile) && this.myProjectFileToSymbolsRuleMap.ContainsKey(sourceFile) &&
                   this.myProjectFileToSymbolsOptionMap.ContainsKey(sourceFile);
        }

        private static bool Accepts(IPsiSourceFile sourceFile)
        {
            return sourceFile.GetAllPossiblePsiLanguages().Any(x => x.Is<SecretLanguage>());
        }

        private class PsiPersistentCache<T> : SimplePersistentCache<T>
        {
            public PsiPersistentCache(
                IShellLocks locks, int formatVersion, string cacheDirectoryName, IPsiConfiguration psiConfiguration)
                : base(locks, formatVersion, cacheDirectoryName, psiConfiguration)
            {
            }

            protected override string LoadSaveProgressText
            {
                get
                {
                    return "Psi Caches";
                }
            }
        }
    }

    public class CachePair
    {
        private readonly IList<SecretPrefixSymbol> myOptions;
        private readonly IList<SecretRuleSymbol> myRules;

        public CachePair(IList<SecretRuleSymbol> rules, IList<SecretPrefixSymbol> options)
        {
            this.myRules = rules;
            this.myOptions = options;
        }

        public IList<SecretPrefixSymbol> Options
        {
            get
            {
                return this.myOptions;
            }
        }

        public IList<SecretRuleSymbol> Rules
        {
            get
            {
                return this.myRules;
            }
        }
    }
}
