// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   N3Lexer.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Psi.Secret
{
    public class N3Lexer : ILexer
    {
        public N3Lexer(IBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IBuffer Buffer { get; private set; }

        public object CurrentPosition { get; set; }

        public int TokenEnd { get; private set; }
        public int TokenStart { get; private set; }
        public TokenNodeType TokenType { get; private set; }

        public void Advance()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
