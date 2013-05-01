using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.BuildScripts.NAnt;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Html;
using JetBrains.ReSharper.Psi.Html.Impl.Parsing;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Impl.PsiManagerImpl;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
//using JetBrains.ReSharper.Psi.Secret.Formatter;
using JetBrains.ReSharper.Psi.Secret.Impl;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Secret
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
                true);

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
                SourceFile = sourceFile;
            }
        }
    }
}