// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesSyntaxHighlighting.cs
// </summary>
// ***********************************************************************

using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.NTriples.CodeInspections.Highlightings
{
    [StaticSeverityHighlighting(Severity.INFO, HighlightingGroupIds.LanguageUsage, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = false)]
    internal class NTriplesSyntaxHighlighting : ICustomAttributeIdHighlighting, IHighlightingWithRange
    {
        private readonly ITreeNode myElement;

        /// <summary>Constructor.</summary>
        /// <param name="element">The tree node element.</param>
        /// <param name="attributeId">The text from the 'Environment -> Fonts and colors -> display items' list.</param>
        public NTriplesSyntaxHighlighting(ITreeNode element, string attributeId)
        {
            this.myElement = element;
            this.AttributeId = attributeId;
        }

        public string AttributeId { get; private set; }

        public string ErrorStripeToolTip
        {
            get
            {
                return null;
            }
        }

        public int NavigationOffsetPatch
        {
            get
            {
                return 0;
            }
        }

        public string ToolTip
        {
            get
            {
                return null;
            }
        }

        public DocumentRange CalculateRange()
        {
            return this.myElement.GetDocumentRange();
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
