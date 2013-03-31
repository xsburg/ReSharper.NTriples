// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   N3LanguageService.cs
// </summary>
// ***********************************************************************

using System.Diagnostics;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret
{
    [Language(typeof(SecretLanguage))]
    public class N3LanguageService : LanguageService
    {
        //private readonly N3CodeFormatter myFormatter;

        internal static readonly NodeTypeSet TokensToSkip = new NodeTypeSet(new NodeType[] { });

        public N3LanguageService(PsiLanguageType psiLanguageType, IConstantValueService constantValueService)
            : base(psiLanguageType, constantValueService)
        {
            //this.myFormatter = formatter;
        }

        public override ILanguageCacheProvider CacheProvider
        {
            get
            {
                return null;
            }
        }

        public override ICodeFormatter CodeFormatter
        {
            get
            {
                return null /*myFormatter*/;
            }
        }

        public override bool SupportTypeMemberCache
        {
            get
            {
                return true;
            }
        }

        public override ITypePresenter TypePresenter
        {
            get
            {
                return DefaultTypePresenter.Instance;
            }
        }

        public override IWordIndexLanguageProvider WordIndexLanguageProvider
        {
            get
            {
                return new N3WordIndexProvider();
            }
        }

        public override ILexer CreateFilteringLexer(ILexer lexer)
        {
            Debugger.Break();
            return null;
            //return new FilteringN3Lexer(lexer);
        }

        public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
        {
            Debugger.Break();
            return new N3Parser(lexer, sourceFile);
        }

        public override ILexerFactory GetPrimaryLexerFactory()
        {
            Debugger.Break();
            return N3LexerFactory.Instance;
        }

        public override bool IsFilteredNode(ITreeNode node)
        {
            Debugger.Break();
            var tokenNode = node as ITokenNode;
            return tokenNode != null && SimpleFilteringLexer.IS_WHITESPACE(tokenNode.GetTokenType());
        }
    }
}
