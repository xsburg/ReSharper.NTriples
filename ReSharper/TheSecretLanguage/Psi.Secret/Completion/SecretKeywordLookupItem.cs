// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretKeywordLookupItem.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    internal sealed class SecretKeywordLookupItem : TextLookupItemBase, IKeywordLookupItem
    {
        public SecretKeywordLookupItem(string text, string suffix)
        {
            this.InsertText = suffix;
            this.ReplaceText = suffix;
            this.InsertCaretOffset = suffix.Length;
            this.ReplaceCaretOffset = suffix.Length;
            this.Text = text;
        }

        public override IconId Image
        {
            get
            {
                return null;
            }
        }

        protected override RichText GetDisplayName()
        {
            var richText = base.GetDisplayName();
            LookupUtil.AddEmphasize(richText, new TextRange(0, richText.Length));
            return richText;
        }
    }
}
