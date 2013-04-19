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
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

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

        public static CachePair Read(BinaryReader reader, IPsiSourceFile sourceFile)
        {
            IList<SecretRuleSymbol> ruleData = ReadRules(reader, sourceFile);
            IList<SecretPrefixSymbol> optionData = ReadOptions(reader, sourceFile);

            return new CachePair(ruleData, optionData);
        }

        public static void Write(CachePair pair, BinaryWriter writer)
        {
            IList<SecretRuleSymbol> ruleItems = pair.Rules;
            writer.Write(ruleItems.Count);

            foreach (SecretRuleSymbol ruleItem in ruleItems)
            {
                ruleItem.Write(writer);
            }

            IList<SecretPrefixSymbol> optionItems = pair.Options;
            writer.Write(optionItems.Count);

            foreach (SecretPrefixSymbol optionItem in optionItems)
            {
                optionItem.Write(writer);
            }
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
            var prefix = element as IPrefixDeclaration;
            if (prefix != null)
            {
                VisitPrefix(prefix);
            }
            /*var optionDefinition = element as IOptionDefinition;
      if (optionDefinition != null)
      {
        VisitOptionDefinition(optionDefinition);
        return;
      }
      var ruleDeclaration = element as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        VisitRuleDeclaration(ruleDeclaration);
      }*/
        }

        private void VisitPrefix(IPrefixDeclaration prefix)
        {
            var name = prefix.PrefixName.GetText();
            int offset = prefix.GetNavigationRange().TextRange.StartOffset;
            var psiSourceFile = prefix.GetSourceFile();
            var uri = prefix.UriString.GetText();
            var isStandard = prefix.Prefix.GetTokenType() == SecretTokenType.STD_PREFIX_KEYWORD;
            mySymbols.Add(new SecretPrefixSymbol(name, offset, uri, isStandard, psiSourceFile));
        }

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
            var ret = new SecretCacheBuilder();
            file.ProcessDescendants(ret);
            return ret.GetSymbols();
        }

        private static IList<SecretPrefixSymbol> ReadOptions(BinaryReader reader, IPsiSourceFile sourceFile)
        {
            int count = reader.ReadInt32();
            var ret = new List<SecretPrefixSymbol>();

            for (int i = 0; i < count; i++)
            {
                var symbol = new SecretPrefixSymbol(sourceFile);
                symbol.Read(reader);
                ret.Add(symbol);
            }

            return ret;
        }

        private static IList<SecretRuleSymbol> ReadRules(BinaryReader reader, IPsiSourceFile sourceFile)
        {
            int count = reader.ReadInt32();
            var ret = new List<SecretRuleSymbol>();

            for (int i = 0; i < count; i++)
            {
                var symbol = new SecretRuleSymbol(sourceFile);
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
