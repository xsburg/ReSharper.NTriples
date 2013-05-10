// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretBracketMatcher.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.TypingAssist;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Util;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.TypingAssist
{
    public class NTriplesBracketMatcher : BracketMatcher
    {
        public NTriplesBracketMatcher()
            : base(
                new[]
                    {
                        new Pair<TokenNodeType, TokenNodeType>(SecretTokenType.L_BRACE, SecretTokenType.R_BRACE),
                        new Pair<TokenNodeType, TokenNodeType>(SecretTokenType.L_BRACKET, SecretTokenType.R_BRACKET),
                        new Pair<TokenNodeType, TokenNodeType>(SecretTokenType.L_PARENTHESES, SecretTokenType.R_PARENTHESES)
                    })
        {
        }
    }
}
