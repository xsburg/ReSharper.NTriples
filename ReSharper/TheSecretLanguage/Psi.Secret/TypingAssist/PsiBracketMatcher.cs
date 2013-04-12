// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiBracketMatcher.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.TypingAssist;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.TypingAssist
{
    public class PsiBracketMatcher : BracketMatcher
    {
        public PsiBracketMatcher()
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
