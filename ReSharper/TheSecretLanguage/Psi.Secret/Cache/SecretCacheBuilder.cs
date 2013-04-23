// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiCacheBuilder.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.ReSharper.Psi.Secret.Util;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    internal class SecretCacheBuilder : IRecursiveElementProcessor
    {
        private readonly List<ISecretSymbol> mySymbols = new List<ISecretSymbol>();

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

        public static void Write(SecretFileCache pair, BinaryWriter writer)
        {
            var uriIdentifiers = pair.UriIdentifiers;
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

        private readonly List<IUriIdentifier> myUriIdentifiers = new List<IUriIdentifier>();
        private readonly ISecretFile file;

        private SecretCacheBuilder(ISecretFile file)
        {
            this.file = file;
        }

        public void ProcessBeforeInterior(ITreeNode element)
        {
            var uriIdentifier = element as IUriIdentifier;
            if (uriIdentifier != null)
            {
                if (uriIdentifier.Prefix != null)
                {
                    var psiSourceFile = uriIdentifier.GetSourceFile();
                    var prefixElement = ((SecretFile)file).GetDeclaredElements(uriIdentifier.Prefix.GetText()).FirstOrDefault();
                    if (prefixElement != null)
                    {
                        var prefixDeclaration = prefixElement.GetDeclarationsIn(psiSourceFile).OfType<IPrefixDeclaration>().FirstOrDefault();
                        if (prefixDeclaration != null)
                        {
                            int offset = uriIdentifier.GetNavigationRange().TextRange.StartOffset;
                            var ns = prefixDeclaration.UriString.GetText().TrimEnd('#');
                            mySymbols.Add(new SecretUriIdentifierSymbol(ns, uriIdentifier.LocalName.GetText(), offset, psiSourceFile));
                        }
                    }
                }
                else
                {
                    var uriString = uriIdentifier.UriString.GetText();
                    var separatorIndex = uriString.LastIndexOf('#');
                    if (separatorIndex > 0)
                    {
                        var ns = uriString.Substring(0, separatorIndex);
                        var length = uriString.Length - (separatorIndex + 1);
                        if (length > 0)
                        {
                            int offset = uriIdentifier.GetNavigationRange().TextRange.StartOffset;
                            var psiSourceFile = uriIdentifier.GetSourceFile();
                            var localName = uriString.Substring(separatorIndex + 1, length);
                            mySymbols.Add(new SecretUriIdentifierSymbol(ns, localName, offset, psiSourceFile));
                        }
                    }
                }
            }
        }

        /*private void VisitPrefix(IPrefixDeclaration prefix)
        {
            var name = prefix.PrefixName.GetText();
            int offset = prefix.GetNavigationRange().TextRange.StartOffset;
            var psiSourceFile = prefix.GetSourceFile();
            var uri = prefix.UriString.GetText();
            var isStandard = prefix.Prefix.GetTokenType() == SecretTokenType.STD_PREFIX_KEYWORD;
            mySymbols.Add(new SecretPrefixSymbol(name, offset, uri, isStandard, psiSourceFile));
        }*/

        /*private void VisitOptionDefinition(IOptionDefinition optionDefinition)
    {
      string name = optionDefinition.OptionName.GetText();
      int offset = optionDefinition.GetNavigationRange().TextRange.StartOffset;
      IPsiSourceFile psiSourceFile = optionDefinition.GetSourceFile();
      string value = "";
      IOptionStringValue valueNode = optionDefinition.OptionStringValue;
      if (valueNode != null)
      {
        value = valueNode.GetText();
        if ("\"".Equals(value.Substring(0, 1)))
        {
          value = value.Substring(1, value.Length - 1);
        }
        if ("\"".Equals(value.Substring(value.Length - 1, 1)))
        {
          value = value.Substring(0, value.Length - 1);
        }
      }
      mySymbols.Add(new SecretOptionSymbol(name, offset, value, psiSourceFile));
    }

    private void VisitRuleDeclaration(IRuleDeclaration ruleDeclaration)
    {
      string name = ruleDeclaration.DeclaredName;
      int offset = ruleDeclaration.GetNavigationRange().TextRange.StartOffset;
      IPsiSourceFile psiSourceFile = ruleDeclaration.GetSourceFile();
      mySymbols.Add(new SecretRuleSymbol(name, offset, psiSourceFile));
    }*/

        [CanBeNull]
        private static ICollection<ISecretSymbol> Build(ISecretFile file)
        {
            var ret = new SecretCacheBuilder(file);
            file.ProcessDescendants(ret);
            return ret.GetSymbols();
        }

        private static IList<TSymbol> ReadSymbolsOfType<TSymbol>(BinaryReader reader, IPsiSourceFile sourceFile) where TSymbol : SecretSymbolBase, new()
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
