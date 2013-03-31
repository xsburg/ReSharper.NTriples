// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   WhitespaceBase.cs
// </summary>
// ***********************************************************************

using System;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    internal abstract class WhitespaceBase : SecretTokenBase, IWhitespaceNode
    {
        private readonly string myText;

        protected WhitespaceBase(string text)
        {
            this.myText = text;
        }

        public abstract bool IsNewLine { get; }

        public override string GetText()
        {
            return this.myText;
        }

        public override int GetTextLength()
        {
            return this.myText.Length;
        }

        public override String ToString()
        {
            return base.ToString() + " spaces:" + "\"" + this.GetText() + "\"";
        }
    }
}
