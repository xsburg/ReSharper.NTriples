using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    internal class SecretParser : SecretParserGenerated, ISecretParser
    {
        private readonly ILexer<int> originalLexer;
        private readonly SeldomInterruptChecker myCheckForInterrupt;

        public SecretParser(ILexer<int> lexer)
        {
            this.originalLexer = lexer;
            myCheckForInterrupt = new SeldomInterruptChecker();
            setLexer(new SecretFilteringLexer(lexer));
        }

        public IFile ParseFile()
        {
            var file = (SecretFile)parseSecretFile();
            InsertMissingTokens(file, false);
            return file;
        }

        protected override TreeElement createToken()
        {
            LeafElementBase element = TreeElementFactory.CreateLeafElement(myLexer.TokenType, myLexer.Buffer, myLexer.TokenStart, myLexer.TokenEnd);
            SetOffset(element, myLexer.TokenStart);
            myLexer.Advance();
            return element;
        }

        private void InsertMissingTokens(TreeElement result, bool trimMissingTokens)
        {
            SecretMissingTokensInserter.Run(result, originalLexer, this, trimMissingTokens, myCheckForInterrupt);
        }
    }
}