using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Tree.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    internal class SecretParser : SecretParserGenerated, ISecretParser
    {
        private readonly ILexer<int> originalLexer;

        public SecretParser(ILexer<int> lexer)
        {
            this.originalLexer = lexer;

            this.setLexer(this.originalLexer);
        }

        public IFile ParseFile()
        {
            return (SecretFile)parseSecretFile();
        }
    }
}