using JetBrains.ReSharper.Psi.Parsing;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    internal class SecretFilteringLexer : FilteringLexer, ILexer<int>
    {
        public SecretFilteringLexer(ILexer lexer) : base(lexer)
        {
        }

        protected override bool Skip(TokenNodeType tokenType)
        {
            return SecretLanguageService.WHITESPACE_OR_COMMENT[tokenType];
        }

        public ILexer OriginalLexer
        {
            get { return this.myLexer; }
        }

        int ILexer<int>.CurrentPosition
        {
            get { return ((ILexer<int>)this.myLexer).CurrentPosition; }
            set { ((ILexer<int>)this.myLexer).CurrentPosition = value; }
        }
    }
}