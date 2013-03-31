// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   Whitespace.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Secret.Parsing;

namespace JetBrains.ReSharper.Psi.Secret.Tree
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
