// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCacheBuilder.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    internal class SecretCacheBuilder : IRecursiveElementProcessor
    {
        private readonly ISecretFile file;
        private readonly List<ISecretSymbol> mySymbols = new List<ISecretSymbol>();

        private SecretCacheBuilder(ISecretFile file)
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
        public static ICollection<ISecretSymbol> Build(IPsiSourceFile sourceFile)
        {
            var file = sourceFile.GetPsiFile<SecretLanguage>(new DocumentRange(sourceFile.Document, 0)) as ISecretFile;
            if (file == null)
            {
                return null;
            }
            return Build(file);
        }

        public static SecretFileCache Read(BinaryReader reader, IPsiSourceFile sourceFile)
        {
            var uriIdentifierSymbols = ReadSymbolsOfType<SecretUriIdentifierSymbol>(reader, sourceFile);
            return new SecretFileCache(uriIdentifierSymbols);
        }

        public static void Write(SecretFileCache fileCache, BinaryWriter writer)
        {
            var uriIdentifiers = fileCache.UriIdentifiers;
            writer.Write(uriIdentifiers.Count);
            uriIdentifiers.Apply(i => i.Write(writer));
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
                var ns = uriIdentifier.GetNamespace();
                if (!string.IsNullOrEmpty(ns))
                {
                    var ln = uriIdentifier.GetLocalName();
                    var kind = uriIdentifier.GetKind();
                    int offset = element.GetNavigationRange().TextRange.StartOffset;
                    var psiSourceFile = element.GetSourceFile();
                    this.mySymbols.Add(new SecretUriIdentifierSymbol(ns, ln, kind, offset, psiSourceFile));
                }
            }
        }

        [CanBeNull]
        private static ICollection<ISecretSymbol> Build(ISecretFile file)
        {
            var ret = new SecretCacheBuilder(file);
            file.ProcessDescendants(ret);
            return ret.GetSymbols();
        }

        private static IList<TSymbol> ReadSymbolsOfType<TSymbol>(BinaryReader reader, IPsiSourceFile sourceFile)
            where TSymbol : SecretSymbolBase, new()
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

        private ICollection<ISecretSymbol> GetSymbols()
        {
            return this.mySymbols;
        }
    }
}
