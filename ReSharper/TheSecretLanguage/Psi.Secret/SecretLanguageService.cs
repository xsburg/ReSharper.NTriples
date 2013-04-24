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
using JetBrains.ReSharper.Psi.Secret.Impl;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret
{
    [Language(typeof(SecretLanguage))]
    public class SecretLanguageService : LanguageService
    {
        private readonly SecretWordIndexLanguageProvider wordIndexLanguageProvider = new SecretWordIndexLanguageProvider();

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
            var typedLexer = (lexer as ILexer<int>) ?? lexer.ToCachingLexer();
            return new Parser(typedLexer, sourceFile);
        }

        public override bool IsFilteredNode(ITreeNode node)
        {
            return node.IsWhitespaceToken();
        }

        public override IWordIndexLanguageProvider WordIndexLanguageProvider
        {
            get
            {
                return wordIndexLanguageProvider;
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

        private class Parser : SecretParser
        {
            public Parser(ILexer lexer, IPsiSourceFile sourceFile)
                : base(lexer)
            {
                SourceFile = sourceFile;
            }
        }
    }
}