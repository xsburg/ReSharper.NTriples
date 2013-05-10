// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretGenericToken.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tree
{
    public class SecretGenericToken : SecretTokenBase
    {
        private readonly TokenNodeType myNodeType;
        private readonly string myText;

        public SecretGenericToken(TokenNodeType nodeType, string text)
        {
            this.myNodeType = nodeType;
            this.myText = text;
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
                return this.myNodeType;
            }
        }

        public override string GetText()
        {
            return this.myText;
        }

        public override int GetTextLength()
        {
            return this.myText.Length;
        }
    }
}
