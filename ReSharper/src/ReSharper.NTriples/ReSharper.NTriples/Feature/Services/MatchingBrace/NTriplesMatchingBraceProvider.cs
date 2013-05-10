// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesMatchingBraceProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.MatchingBrace;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.Feature.Services.MatchingBrace
{
    [Language(typeof(NTriplesLanguage))]
    internal class NTriplesMatchingBraceProvider : ISimpleMatchingBraceProvider
    {
        public int Compare(TokenNodeType x, TokenNodeType y)
        {
            if ((this.IsLeftBracket(x) && this.IsLeftBracket(y)) || (this.IsRightBracket(x) && this.IsRightBracket(y)))
            {
                if (x == NTriplesTokenType.L_BRACE)
                {
                    return 1;
                }
                if (y == NTriplesTokenType.L_BRACE)
                {
                    return -1;
                }
                if (x == NTriplesTokenType.R_BRACE)
                {
                    return 1;
                }
                if (y == NTriplesTokenType.R_BRACE)
                {
                    return -1;
                }
            }
            return 0;
        }

        public bool IsLeftBracket(TokenNodeType tokenType)
        {
            if (((tokenType != NTriplesTokenType.L_BRACE) && (tokenType != NTriplesTokenType.L_PARENTHESES)) &&
                (tokenType != NTriplesTokenType.L_BRACKET))
            {
                return false;
            }
            return true;
        }

        public bool IsRightBracket(TokenNodeType tokenType)
        {
            if (((tokenType != NTriplesTokenType.R_BRACE) && (tokenType != NTriplesTokenType.R_PARENTHESES)) &&
                (tokenType != NTriplesTokenType.R_BRACKET))
            {
                return false;
            }
            return true;
        }

        public bool Match(TokenNodeType token1, TokenNodeType token2)
        {
            if (token1 == NTriplesTokenType.L_BRACE)
            {
                return (token2 == NTriplesTokenType.R_BRACE);
            }
            if (token1 == NTriplesTokenType.L_PARENTHESES)
            {
                return (token2 == NTriplesTokenType.R_PARENTHESES);
            }
            if (token1 == NTriplesTokenType.L_BRACKET)
            {
                return (token2 == NTriplesTokenType.R_BRACKET);
            }
            if (token1 == NTriplesTokenType.R_BRACE)
            {
                return (token2 == NTriplesTokenType.L_BRACE);
            }
            if (token1 == NTriplesTokenType.R_PARENTHESES)
            {
                return (token2 == NTriplesTokenType.L_PARENTHESES);
            }
            if (token1 == NTriplesTokenType.R_BRACKET)
            {
                return (token2 == NTriplesTokenType.L_BRACKET);
            }

            return false;
        }
    }
}
