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

namespace JetBrains.ReSharper.Psi.Secret
{
    public class N3LexerFactory : ILexerFactory
    {
        public static readonly N3LexerFactory Instance = new N3LexerFactory();

        public ILexer CreateLexer(IBuffer buffer)
        {
            return new N3Lexer(buffer);
        }
    }
}
