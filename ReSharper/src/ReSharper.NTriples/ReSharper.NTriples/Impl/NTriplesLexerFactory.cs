// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   N3LexerFactory.cs
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
            return new SecretLexer(buffer);
        }
    }
}
