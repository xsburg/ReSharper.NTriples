// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   Identifier.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Secret.Parsing;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    internal class Identifier : SecretTokenBase, ISecretIdentifier
    {
        private readonly string myText = null;

        public Identifier(string text)
        {
            this.myText = text;
        }

        public override PsiLanguageType Language
        {
            get
            {
                return SecretLanguage.Instance;
            }
        }

        public string Name
        {
            // todo: verify it's ok to use c#
            get
            {
                return CSharpResolveUtil.ReferenceName(this.myText);
            }
        }

        public override NodeType NodeType
        {
            get
            {
                return SecretTokenType.IDENTIFIER;
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
