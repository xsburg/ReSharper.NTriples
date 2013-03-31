using JetBrains.ReSharper.Psi.Parsing;

namespace ReSharperSecretLanguage
{
    public class FilteringN3Lexer : FilteringLexer
    {
        public FilteringN3Lexer(ILexer lexer)
            : base(lexer)
        {
        }

        protected override bool Skip(TokenNodeType tokenType)
        {
            return N3LanguageService.TokensToSkip[tokenType];
        }
    }
}