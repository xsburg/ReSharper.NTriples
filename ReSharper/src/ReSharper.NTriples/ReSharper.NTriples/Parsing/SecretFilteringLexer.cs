// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretFilteringLexer.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Parsing
{
    internal class SecretFilteringLexer : FilteringLexer, ILexer<int>
    {
        public SecretFilteringLexer(ILexer lexer) : base(lexer)
        {
        }

        public ILexer OriginalLexer
        {
            get
            {
                return this.myLexer;
            }
        }

        int ILexer<int>.CurrentPosition
        {
            get
            {
                return ((ILexer<int>)this.myLexer).CurrentPosition;
            }
            set
            {
                ((ILexer<int>)this.myLexer).CurrentPosition = value;
            }
        }

        protected override bool Skip(TokenNodeType tokenType)
        {
            return SecretLanguageService.WHITESPACE_OR_COMMENT[tokenType];
        }
    }
}
