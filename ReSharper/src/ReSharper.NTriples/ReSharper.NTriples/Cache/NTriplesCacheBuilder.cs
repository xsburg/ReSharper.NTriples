// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCacheBuilder.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Cache
{
    internal class NTriplesCacheBuilder : IRecursiveElementProcessor
    {
        private readonly INTriplesFile file;
        private readonly List<INTriplesSymbol> mySymbols = new List<INTriplesSymbol>();

        private NTriplesCacheBuilder(INTriplesFile file)
        {
            this.file = file;
        }

        public bool ProcessingIsFinished
        {
            get
            {
                return false;
            }
        }

        [CanBeNull]
        public static ICollection<INTriplesSymbol> Build(IPsiSourceFile sourceFile)
        {
            var file = sourceFile.GetPsiFile<NTriplesLanguage>(new DocumentRange(sourceFile.Document, 0)) as INTriplesFile;
            if (file == null)
            {
                return null;
            }
            return Build(file);
        }

        public static NTriplesFileCache Read(BinaryReader reader, IPsiSourceFile sourceFile)
        {
            var uriIdentifierSymbols = ReadSymbolsOfType<NTriplesUriIdentifierSymbol>(reader, sourceFile);
            var prefixDeclarationSymbols = ReadSymbolsOfType<NTriplesPrefixDeclarationSymbol>(reader, sourceFile);
            return new NTriplesFileCache(uriIdentifierSymbols, prefixDeclarationSymbols);
        }

        public static void Write(NTriplesFileCache fileCache, BinaryWriter writer)
        {
            writer.Write(fileCache.UriIdentifiers.Count);
            fileCache.UriIdentifiers.Apply(i => i.Write(writer));
            writer.Write(fileCache.PrefixDeclarations.Count);
            fileCache.PrefixDeclarations.Apply(i => i.Write(writer));
        }

        public bool InteriorShouldBeProcessed(ITreeNode element)
        {
            return true;
        }

        public void ProcessAfterInterior(ITreeNode element)
        {
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
            var uriIdentifier = element as IUriIdentifierDeclaredElement;
            if (uriIdentifier != null)
            {
                var ns = uriIdentifier.GetNamespace() ?? "";
                var ln = uriIdentifier.GetLocalName();
                var info = uriIdentifier.GetInfo();
                int offset = element.GetNavigationRange().TextRange.StartOffset;
                var psiSourceFile = element.GetSourceFile();
                this.mySymbols.Add(new NTriplesUriIdentifierSymbol(ns, ln, info, offset, psiSourceFile));
                return;
            }

            var prefixDeclaration = element as IPrefixDeclaration;
            if (prefixDeclaration != null)
            {
                var name = prefixDeclaration.PrefixName != null
                               ? prefixDeclaration.PrefixName.GetText()
                               : "";

                if (prefixDeclaration.UriString != null)
                {
                    var uri = prefixDeclaration.UriString.GetText();
                    int offset = element.GetNavigationRange().TextRange.StartOffset;
                    var psiSourceFile = element.GetSourceFile();
                    this.mySymbols.Add(new NTriplesPrefixDeclarationSymbol(uri, name, offset, psiSourceFile));
                }
            }
        }

        [CanBeNull]
        private static ICollection<INTriplesSymbol> Build(INTriplesFile file)
        {
            var ret = new NTriplesCacheBuilder(file);
            file.ProcessDescendants(ret);
            return ret.GetSymbols();
        }

        private static IList<TSymbol> ReadSymbolsOfType<TSymbol>(BinaryReader reader, IPsiSourceFile sourceFile)
            where TSymbol : NTriplesSymbolBase, new()
        {
            int count = reader.ReadInt32();
            var ret = new List<TSymbol>();

            for (int i = 0; i < count; i++)
            {
                var symbol = new TSymbol();
                symbol.SetSourceFile(sourceFile);
                symbol.Read(reader);
                ret.Add(symbol);
            }

            return ret;
        }

        private ICollection<INTriplesSymbol> GetSymbols()
        {
            return this.mySymbols;
        }
    }
}
