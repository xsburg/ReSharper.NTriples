// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   Whitespace.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Secret;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.Tree
{
    internal class Whitespace : WhitespaceBase
    {
        public Whitespace(string text)
            : base(text)
        {
        }

        public override bool IsNewLine
        {
            get
            {
                return false;
            }
        }

        public override PsiLanguageType Language
        {
            get
            {
                return SecretLanguage.Instance;
            }
        }

        public override NodeType NodeType
        {
            get
            {
                return SecretTokenType.WHITE_SPACE;
            }
        }
    }
}
