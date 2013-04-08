using JetBrains.Annotations;
using JetBrains.Application;
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
            return (SecretFile)parseSecretFile();
        }
    }
}