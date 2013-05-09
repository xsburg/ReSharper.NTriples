// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretKeywordLookupItem.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace ReSharper.NTriples.Completion
{
    internal sealed class NTriplesKeywordLookupItem : TextLookupItemBase, IKeywordLookupItem
    {
        public NTriplesKeywordLookupItem(string text, string suffix)
        {
            this.InsertText = suffix;
            this.ReplaceText = suffix;
            this.InsertCaretOffset = suffix.Length;
            this.ReplaceCaretOffset = suffix.Length;
            this.Text = text;
            this.OrderingString = char.MaxValue + text.ToLowerInvariant();
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
