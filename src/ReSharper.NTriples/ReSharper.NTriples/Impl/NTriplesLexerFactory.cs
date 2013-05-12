// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesLexerFactory.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.Impl
{
    public class NTriplesLexerFactory : ILexerFactory
    {
        public static readonly NTriplesLexerFactory Instance = new NTriplesLexerFactory();

        public ILexer CreateLexer(IBuffer buffer)
        {
            return new NTriplesLexer(buffer);
        }
    }
}
