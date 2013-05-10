// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesBracketMatcher.cs
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
                        new Pair<TokenNodeType, TokenNodeType>(NTriplesTokenType.L_BRACE, NTriplesTokenType.R_BRACE),
                        new Pair<TokenNodeType, TokenNodeType>(NTriplesTokenType.L_BRACKET, NTriplesTokenType.R_BRACKET),
                        new Pair<TokenNodeType, TokenNodeType>(NTriplesTokenType.L_PARENTHESES, NTriplesTokenType.R_PARENTHESES)
                    })
        {
        }
    }
}
