// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLexer.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    public class SecretLexer : SecretLexerGenerated
    {
        public SecretLexer(IBuffer buffer) : base(buffer)
        {
        }

        public static string GetTokenText(TokenNodeType token)
        {
            return GetKeywordTextByTokenType(token);
        }

        protected static string GetKeywordTextByTokenType(NodeType tokenType)
        {
            return tokenTypesToText[tokenType];
        }

        public static bool IsWhitespace(string s)
        {
            var lexer = new SecretLexer(new StringBuffer(s));
            lexer.Start();
            return lexer.TokenType != null && lexer.TokenType.IsWhitespace && lexer.TokenEnd == s.Length;
        }
    }
}
