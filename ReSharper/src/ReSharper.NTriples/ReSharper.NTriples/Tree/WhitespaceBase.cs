// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   WhitespaceBase.cs
// </summary>
// ***********************************************************************

using System;

namespace ReSharper.NTriples.Tree
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
