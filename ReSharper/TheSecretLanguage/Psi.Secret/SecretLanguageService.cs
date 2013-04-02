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
    public interface ISecretParser : IParser
    {
    }

    /*internal class SecretParser : SecretParserGenerated, ISecretParser
    {
        private readonly ILexer<int> originalLexer;

        public SecretParser(ILexer<int> lexer, IEnumerable<PreProcessingDirective> defines)
        {
            originalLexer = lexer;

            setLexer(originalLexer);
        }

        public IFile ParseFile()
        {
            return (SecretFile)parseSecretFile();
        }

        // TODO: Not really sure why this has to be abstract
        // but changing it changes the interface that gets generated
        public override TreeElement parseIdentifier()
        {
            if (myLexer.TokenType != FSharpTokenType.IDENTIFIER)
                throw new UnexpectedToken(ParserMessages.GetExpectedMessage(ParserMessages.GetString(ParserMessages.IDS__IDENTIFIER)));
            return createToken();
        }
    }*/

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
            return new FSharpFilteringLexerWithoutPreprocessorState(lexer);
        }

        private class MockedParser : IParser
        {
            public IFile ParseFile()
            {
                return new MockedFile();
            }
        }

        private class MockedFile : FileElementBase, IFile, IFileImpl
        {
            public IPsiServices GetPsiServices()
            {
                throw new NotImplementedException();
            }

            public IPsiModule GetPsiModule()
            {
                return null;
            }

            public IPsiSourceFile GetSourceFile()
            {
                return null;
            }

            public ReferenceCollection GetFirstClassReferences()
            {
                throw new NotImplementedException();
            }

            public void ProcessDescendantsForResolve(IRecursiveElementProcessor processor)
            {
            }

            public ITreeNode GetContainingNode(Type t, bool returnThis = false)
            {
                throw new NotImplementedException();
            }

            public T GetContainingNode<T>(bool returnThis = false) where T : ITreeNode
            {
                throw new NotImplementedException();
            }

            public bool Contains(ITreeNode other)
            {
                throw new NotImplementedException();
            }

            public bool IsPhysical()
            {
                throw new NotImplementedException();
            }

            public bool IsValid()
            {
                throw new NotImplementedException();
            }

            public DocumentRange GetNavigationRange()
            {
                throw new NotImplementedException();
            }

            public TreeOffset GetTreeStartOffset()
            {
                throw new NotImplementedException();
            }

            public int GetTextLength()
            {
                throw new NotImplementedException();
            }

            public StringBuilder GetText(StringBuilder to)
            {
                throw new NotImplementedException();
            }

            public IBuffer GetTextAsBuffer()
            {
                throw new NotImplementedException();
            }

            public string GetText()
            {
                throw new NotImplementedException();
            }

            public ITreeNode FindNodeAt(TreeTextRange treeTextRange)
            {
                throw new NotImplementedException();
            }

            public ICollection<ITreeNode> FindNodesAt(TreeOffset treeTextOffset)
            {
                throw new NotImplementedException();
            }

            public ITreeNode FindTokenAt(TreeOffset treeTextOffset)
            {
                throw new NotImplementedException();
            }

            public MockedFile()
            {
                UserData = (NodeUserData)Activator.CreateInstance(
                    typeof(NodeUserData),
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[]
                        {
                            new AspTag(new NAntAttributeNodeType(null), new HtmlTokenNodeTypes(HtmlLanguage.Instance)),
                            new Dictionary<ITreeNode, UserDataHolder>()
                        },
                    CultureInfo.CurrentCulture);
            }

            public ITreeNode Parent { get; private set; }

            public override NodeType NodeType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ITreeNode FirstChild { get; private set; }
            public ITreeNode LastChild { get; private set; }
            public ITreeNode NextSibling { get; private set; }
            public ITreeNode PrevSibling { get; private set; }
            public NodeUserData UserData { get; private set; }
            public NodeUserData PersistentUserData { get; private set; }
            public override PsiLanguageType Language { get
            {
                return SecretLanguage.Instance;
            }}
            public IFile ReParse(TreeTextRange modifiedRange, string text)
            {
                throw new NotImplementedException();
            }

            public PsiFileModificationInfo GetReParseResult(TreeTextRange modifiedRange, string text)
            {
                throw new NotImplementedException();
            }

            public bool IsInjected()
            {
                throw new NotImplementedException();
            }

            public CachingLexer CachingLexer { get; private set; }
            public int ModificationCounter { get; private set; }
            public bool CanContainCaseInsensitiveReferences { get; private set; }
            public NodeUserDataHolder NodeUserDataHolder { get; private set; }
            public void SetSourceFile(IPsiSourceFile sourceFile)
            {
            }

            public IChameleonNode FindChameleonWhichCoversRange(TreeTextRange range)
            {
                throw new NotImplementedException();
            }

            public TokenBuffer TokenBuffer { get; set; }
            public ILexerFactory LexerFactory { get; set; }
            public ISecondaryRangeTranslator SecondaryRangeTranslator { get; set; }
            public IDocumentRangeTranslator DocumentRangeTranslator { get; set; }
            public IReferenceProvider ReferenceProvider { get; set; }
        }

        public override IParser CreateParser(ILexer lexer, IPsiModule module, IPsiSourceFile sourceFile)
        {
            return new MockedParser();
            /*return CreateParser(
                lexer,
                module == null || sourceFile == null
                    ? EmptyList<PreProcessingDirective>.InstanceList
                    : sourceFile.Properties.GetDefines());*/
        }

        /*public ISecretParser CreateParser(ILexer lexer, IEnumerable<PreProcessingDirective> defines)
        {
            var typedLexer = (lexer as ILexer<int>) ?? lexer.ToCachingLexer();
            return new SecretParser(typedLexer, defines);
        }*/

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
                    SecretTokenType.END_OF_LINE_COMMENT,
                    SecretTokenType.C_STYLE_COMMENT
                });

        private class FSharpFilteringLexerWithoutPreprocessorState : FilteringLexer
        {
            public FSharpFilteringLexerWithoutPreprocessorState(ILexer lexer)
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