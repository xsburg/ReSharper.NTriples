// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NewLine.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.Tree
{
    internal class NewLine : WhitespaceBase
    {
        public NewLine(string text)
            : base(text)
        {
        }

        public override bool IsNewLine
        {
            get
            {
                return true;
            }
        }

        public override PsiLanguageType Language
        {
            get
            {
                return NTriplesLanguage.Instance;
            }
        }

        public override NodeType NodeType
        {
            get
            {
                return SecretTokenType.NEW_LINE;
            }
        }
    }
}
