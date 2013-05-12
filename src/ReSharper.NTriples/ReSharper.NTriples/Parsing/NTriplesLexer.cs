// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesLexer.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace ReSharper.NTriples.Parsing
{
    public class NTriplesLexer : NTriplesLexerGenerated
    {
        public NTriplesLexer(IBuffer buffer) : base(buffer)
        {
        }

        public static string GetTokenText(TokenNodeType token)
        {
            return GetKeywordTextByTokenType(token);
        }

        public static bool IsWhitespace(string s)
        {
            var lexer = new NTriplesLexer(new StringBuffer(s));
            lexer.Start();
            return lexer.TokenType != null && lexer.TokenType.IsWhitespace && lexer.TokenEnd == s.Length;
        }

        protected static string GetKeywordTextByTokenType(NodeType tokenType)
        {
            return tokenTypesToText[tokenType];
        }
    }
}
