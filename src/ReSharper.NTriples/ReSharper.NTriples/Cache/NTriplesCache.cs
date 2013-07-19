// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCache.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.Util;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Cache
{
    [PsiComponent]
    public class NTriplesCache : NTriplesCacheBase
    {
        private readonly OneToSetMap<string, NTriplesPrefixDeclarationSymbol> myNameToPrefixDeclarationSymbolsMap =
            new OneToSetMap<string, NTriplesPrefixDeclarationSymbol>();

        private readonly OneToSetMap<string, NTriplesUriIdentifierSymbol> myUriToUriIdentifierSymbolsMap =
            new OneToSetMap<string, NTriplesUriIdentifierSymbol>();

        private readonly OneToSetMap<string, NTriplesUriIdentifierSymbol> myUriToTypePropertyDeclarationSymbolsMap =
            new OneToSetMap<string, NTriplesUriIdentifierSymbol>();

        private readonly OneToSetMap<string, NTriplesUriIdentifierSymbol> myUriToTypeDeclarationSymbolsMap =
            new OneToSetMap<string, NTriplesUriIdentifierSymbol>();

        private readonly OneToSetMap<string, NTriplesUriIdentifierSymbol> myUriToTypeInstantiationSymbolsMap =
            new OneToSetMap<string, NTriplesUriIdentifierSymbol>();

        private readonly OneToSetMap<string, NTriplesUriIdentifierSymbol> myUriToSubjectSymbolsMap =
            new OneToSetMap<string, NTriplesUriIdentifierSymbol>();

        private readonly OneToListMap<IPsiSourceFile, NTriplesPrefixDeclarationSymbol> myProjectFileToSymbolsPrefixDeclarationMap
            =
            new OneToListMap<IPsiSourceFile, NTriplesPrefixDeclarationSymbol>();

        private readonly OneToListMap<IPsiSourceFile, NTriplesUriIdentifierSymbol> myProjectFileToSymbolsUriIdentifierMap =
            new OneToListMap<IPsiSourceFile, NTriplesUriIdentifierSymbol>();

        public NTriplesCache(
            Lifetime lifetime,
            IShellLocks shellLocks,
            IPsiConfiguration psiConfiguration,
            IPersistentIndexManager persistentIdIndex)
            : base(lifetime, shellLocks, psiConfiguration, persistentIdIndex)
        {
        }

        public IEnumerable<NTriplesPrefixDeclarationSymbol> GetAllPrefixDeclarationSymbols()
        {
            return this.myNameToPrefixDeclarationSymbolsMap.SelectMany(x => x.Value);
        }

        public IEnumerable<NTriplesUriIdentifierSymbol> GetAllUriIdentifierSymbols()
        {
            return this.myUriToUriIdentifierSymbolsMap.SelectMany(x => x.Value);
        }

        public OneToListMap<IPsiSourceFile, NTriplesUriIdentifierSymbol> GetAllUriIdentifierSymbolsByFile()
        {
            return this.myProjectFileToSymbolsUriIdentifierMap;
        }

        public IEnumerable<NTriplesUriIdentifierSymbol> GetAllUriIdentifiersInNamespace(string @namespace)
        {
            @namespace = FixNamespace(@namespace);
            return this.myUriToUriIdentifierSymbolsMap.SelectMany(x => x.Value).Where(s => s.Namespace == @namespace);
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

        public IEnumerable<string> GetInstanceTypes(string subjectUri)
        {
            return this.myUriToTypeInstantiationSymbolsMap[subjectUri].SelectMany(s => s.Info.DeclaredTypeNames);
        }

        public IEnumerable<NTriplesUriIdentifierSymbol> GetTypeDeclarations(string subjectUri)
        {
            return
                this.myUriToTypeInstantiationSymbolsMap[subjectUri].Concat(this.myUriToTypeDeclarationSymbolsMap[subjectUri])
                                                                   .Where(
                                                                       s =>
                                                                       s.Info.IsClassDeclaration || s.Info.DeclaredTypeNames.Any());
        }

        public bool HasTypeDeclarations(string subjectUri)
        {
            return this.myUriToTypeInstantiationSymbolsMap[subjectUri]
                .Concat(this.myUriToTypeDeclarationSymbolsMap[subjectUri])
                .Any(s => s.Info.IsClassDeclaration || s.Info.DeclaredTypeNames.Any());
        }

        public IEnumerable<string> GetAvailableProperties(IEnumerable<string> typesUri)
        {
            return typesUri.SelectMany(uri => this.myUriToTypePropertyDeclarationSymbolsMap[uri])
                           .SelectMany(s => s.Info.DeclaredTypePropertyNames);
        }

        public IEnumerable<NTriplesUriIdentifierSymbol> GetSubjects(string uri)
        {
            return this.myUriToSubjectSymbolsMap[uri];
        }

        public bool HasSubjects(string uri)
        {
            return this.myUriToSubjectSymbolsMap.ContainsKey(uri);
        }

        /*public IEnumerable<IUriIdentifierDeclaredElement> GetTypeDeclarationSubjects()
        {
            bool foundImportant = false;
            foreach (var pair in this.myProjectFileToSymbolsUriIdentifierMap)
            {
                var sourceFile = pair.Key;
                foreach (var symbol in pair.Value.Where(_ => _.Info.IsTypeDeclaration))
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

                    foundImportant = true;
                    yield return declaredElement;
                }
            }

            // Fallback to return simple subjects
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
        }*/

        public IEnumerable<NTriplesPrefixDeclarationSymbol> GetPrefixDeclarationSymbols(string name)
        {
            return this.myNameToPrefixDeclarationSymbolsMap.SelectMany(x => x.Value).Where(s => s.Name == name);
        }

        public IEnumerable<INTriplesSymbol> GetUriIdentifierSymbolsDeclaredInFile(IPsiSourceFile sourceFile)
        {
            var symbols = this.myProjectFileToSymbolsUriIdentifierMap.GetValuesCollection(sourceFile);
            return symbols;
        }

        protected override void ClearCache(IPsiSourceFile sourceFile)
        {
            // uri identifiers
            if (this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(sourceFile))
            {
                foreach (NTriplesUriIdentifierSymbol oldDeclaration in this.myProjectFileToSymbolsUriIdentifierMap[sourceFile])
                {
                    string oldUri = oldDeclaration.Namespace + oldDeclaration.LocalName;
                    this.myUriToUriIdentifierSymbolsMap.Remove(oldUri, oldDeclaration);
                    this.myUriToTypePropertyDeclarationSymbolsMap.Remove(oldUri, oldDeclaration);
                    this.myUriToTypeDeclarationSymbolsMap.Remove(oldUri, oldDeclaration);
                    this.myUriToTypeInstantiationSymbolsMap.Remove(oldUri, oldDeclaration);
                    this.myUriToSubjectSymbolsMap.Remove(oldUri, oldDeclaration);
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
                    this.myNameToPrefixDeclarationSymbolsMap.Remove(oldName, oldDeclaration);
                }
            }

            this.myProjectFileToSymbolsUriIdentifierMap.RemoveKey(sourceFile);
            this.myProjectFileToSymbolsPrefixDeclarationMap.RemoveKey(sourceFile);
        }

        protected override NTriplesFileCache ComputeCache(IPsiSourceFile sourceFile, IList<INTriplesSymbol> data)
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

            this.myProjectFileToSymbolsUriIdentifierMap.AddValueRange(sourceFile, uriIdentifierData);
            foreach (NTriplesUriIdentifierSymbol declaration in uriIdentifierData)
            {
                var uri = declaration.Namespace + declaration.LocalName;
                this.myUriToUriIdentifierSymbolsMap.Add(uri, declaration);

                if (declaration.Info.Kind == IdentifierKind.Subject)
                {
                    myUriToSubjectSymbolsMap.Add(uri, declaration);
                }

                if (declaration.Info.IsTypePropertyDeclaration)
                {
                    this.myUriToTypePropertyDeclarationSymbolsMap.Add(uri, declaration);
                }

                if (declaration.Info.IsClassDeclaration)
                {
                    this.myUriToTypeDeclarationSymbolsMap.Add(uri, declaration);
                } else if (declaration.Info.DeclaredTypeNames.Any())
                {
                    this.myUriToTypeInstantiationSymbolsMap.Add(uri, declaration);
                }
            }

            this.myProjectFileToSymbolsPrefixDeclarationMap.AddValueRange(sourceFile, prefixDeclarationData);
            foreach (NTriplesPrefixDeclarationSymbol declaration in prefixDeclarationData)
            {
                this.myNameToPrefixDeclarationSymbolsMap.Add(declaration.Name, declaration);
            }

            return new NTriplesFileCache(uriIdentifierData, prefixDeclarationData);
        }

        protected override bool SymbolsUpToDate(IPsiSourceFile psiSourceFile)
        {
            return this.myProjectFileToSymbolsUriIdentifierMap.ContainsKey(psiSourceFile) &&
                   this.myProjectFileToSymbolsPrefixDeclarationMap.ContainsKey(psiSourceFile);
        }

        private static string FixNamespace(string @namespace)
        {
            return @namespace ?? "";
        }
    }
}
