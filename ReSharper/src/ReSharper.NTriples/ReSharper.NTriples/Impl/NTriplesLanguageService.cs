// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesLanguageService.cs
// </summary>
// ***********************************************************************

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

namespace ReSharper.NTriples.Impl
{
    [Language(typeof(NTriplesLanguage))]
    public class NTriplesLanguageService : LanguageService
    {
        //private readonly NTriplesCodeFormatter formatter;
        internal static readonly NodeTypeSet WHITESPACE_OR_COMMENT = new NodeTypeSet(
            new[]
                {
                    NTriplesTokenType.WHITE_SPACE,
                    NTriplesTokenType.NEW_LINE,
                    NTriplesTokenType.END_OF_LINE_COMMENT
                });

        private readonly NTriplesWordIndexLanguageProvider wordIndexLanguageProvider = new NTriplesWordIndexLanguageProvider();

        public NTriplesLanguageService(
            PsiLanguageType psiLanguageType, IConstantValueService constantValueService /*, NTriplesCodeFormatter formatter*/)
            : base(psiLanguageType, constantValueService)
        {
            //this.formatter = formatter;
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
            //get { return formatter; }
            get
            {
                return null;
            }
        }

        public override bool SupportTypeMemberCache
        {
            get
            {
                return false;
            }
        }

        public override ITypePresenter TypePresenter
        {
            get
            {
                return null;
            }
        }

        public override IWordIndexLanguageProvider WordIndexLanguageProvider
        {
            get
            {
                return this.wordIndexLanguageProvider;
            }
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

        public override ILexer CreateFilteringLexer(ILexer lexer)
        {
            return new NTriplesFilteringLexer(lexer);
        }

        public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
        {
            var typedLexer = (lexer as ILexer<int>) ?? lexer.ToCachingLexer();
            return new Parser(typedLexer, sourceFile);
        }

        public override ILexerFactory GetPrimaryLexerFactory()
        {
            return new NTriplesLexerFactory();
        }

        public override bool IsFilteredNode(ITreeNode node)
        {
            return node.IsWhitespaceToken();
        }

        public override bool IsValidName(DeclaredElementType elementType, string name)
        {
            return NamingUtil.IsIdentifier(name) || Uri.IsWellFormedUriString(name, UriKind.Absolute);
        }

        private class Parser : NTriplesParser
        {
            public Parser(ILexer lexer, IPsiSourceFile sourceFile)
                : base(lexer)
            {
                this.SourceFile = sourceFile;
            }
        }
    }
}
