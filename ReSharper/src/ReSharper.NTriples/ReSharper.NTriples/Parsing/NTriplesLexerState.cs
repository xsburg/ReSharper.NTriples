// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesLexerState.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Parsing;

namespace ReSharper.NTriples.Parsing
{
    public struct NTriplesLexerState
    {
        public TokenNodeType currTokenType;
        public int yy_buffer_end;
        public int yy_buffer_index;
        public int yy_buffer_start;
        public int yy_lexical_state;
    }
}
