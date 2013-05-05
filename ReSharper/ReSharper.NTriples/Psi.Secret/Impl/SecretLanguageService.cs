using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.PsiManagerImpl;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Resolve;

//using JetBrains.ReSharper.Psi.Secret.Formatter;

namespace ReSharper.NTriples.Impl
{
    [Language(typeof(SecretLanguage))]
    public class SecretLanguageService : LanguageService
    {
        //private readonly SecretCodeFormatter formatter;
        private readonly SecretWordIndexLanguageProvider wordIndexLanguageProvider = new SecretWordIndexLanguageProvider();

        public SecretLanguageService(PsiLanguageType psiLanguageType, IConstantValueService constantValueService/*, SecretCodeFormatter formatter*/)
            : base(psiLanguageType, constantValueService)
        {
            //this.formatter = formatter;
        }

        public override ILexerFactory GetPrimaryLexerFactory()
        {
            return new SecretLexerFactory();
        }

        public override bool IsValidName(DeclaredElementType elementType, string name)
        {
            return NamingUtil.IsIdentifier(name) || Uri.IsWellFormedUriString(name, UriKind.Absolute);
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

        public override ICodeFormatter CodeFormatter
        {
            //get { return formatter; }
            get { return null; }
        }

        public override bool IsFilteredNode(ITreeNode node)
        {
            return node.IsWhitespaceToken();
        }

        public override IWordIndexLanguageProvider WordIndexLanguageProvider
        {
            get
            {
                return this.wordIndexLanguageProvider;
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

        public override IDeclaredElementPointer<T> CreateElementPointer<T>(T declaredElement)
        {
            var element = declaredElement as IUriIdentifierDeclaredElement;
            if (element == null)
            {
                return null;
            }

            var node = element as ITreeNode;
            var file = node != null
                           ? node.GetContainingFile()
                           : null;
            IDeclaredElement fakeDeclaredElement = new UriIdentifierDeclaredElement(
                file,
                element.GetNamespace(),
                element.GetLocalName(),
                element.GetKind(),
                element.GetPsiServices(),
                true,
                element);

            var sourceElementPointer = new SourceElementPointer<T>((T)fakeDeclaredElement);
            return sourceElementPointer;
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
                this.SourceFile = sourceFile;
            }
        }
    }
}