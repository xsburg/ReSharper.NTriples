// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   N3Parser.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret
{
    public class N3Parser : IParser
    {
        public N3Parser(ILexer lexer, IPsiSourceFile sourceFile) // : base(lexer)
        {
        }

        public IFile ParseFile()
        {
            throw new NotImplementedException();
        }
    }
}
