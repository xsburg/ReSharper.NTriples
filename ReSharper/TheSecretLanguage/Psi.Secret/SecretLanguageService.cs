using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Asp.Impl.Tree;
using JetBrains.ReSharper.Psi.BuildScripts.NAnt;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Html;
using JetBrains.ReSharper.Psi.Html.Impl.Parsing;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Impl.PsiManagerImpl;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret
{
    //TODO:[Language(typeof(SecretLanguage))]
    public class SecretLanguageService : LanguageService
    {
        //private readonly FSharpWordIndexLanguageProvider wordIndexLanguageProvider = new FSharpWordIndexLanguageProvider();

        public SecretLanguageService(PsiLanguageType psiLanguageType, IConstantValueService constantValueService)
            : base(psiLanguageType, constantValueService)
        {
        }

        public override ILexerFactory GetPrimaryLexerFactory()
        {
            return new SecretLexerFactory();
        }

        public override ILexer CreateFilteringLexer(ILexer lexer)
        {
            return new SecretFilteringLexer(lexer);
        }

        public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
        {
            return null;
            /*return CreateParser(
                lexer,
                module == null || sourceFile == null
                    ? EmptyList<PreProcessingDirective>.InstanceList
                    : sourceFile.Properties.GetDefines());*/
        }

        public override bool IsFilteredNode(ITreeNode node)
        {
            // todo: filter out preprocessor, error elements, doc comment blocks, whitespace nodes as well
            // as whitespace and preprocessor token

            return false;
        }

        public override IWordIndexLanguageProvider WordIndexLanguageProvider
        {
            get
            {
                return null;
            }
        }

        public override ILanguageCacheProvider CacheProvider
        {
            get { return null; }
        }

        public override bool SupportTypeMemberCache
        {
            get { return false; }
        }

        public override ITypePresenter TypePresenter
        {
            get { return null; }
        }

        internal static readonly NodeTypeSet WHITESPACE_OR_COMMENT = new NodeTypeSet(
            new[]
                {
                    SecretTokenType.WHITE_SPACE,
                    SecretTokenType.NEW_LINE,
                    SecretTokenType.END_OF_LINE_COMMENT
                });

        private class SecretFilteringLexer : FilteringLexer
        {
            public SecretFilteringLexer(ILexer lexer)
                : base(lexer)
            {
            }

            protected override bool Skip(TokenNodeType tokenType)
            {
                return WHITESPACE_OR_COMMENT[tokenType];
            }
        }
    }
}