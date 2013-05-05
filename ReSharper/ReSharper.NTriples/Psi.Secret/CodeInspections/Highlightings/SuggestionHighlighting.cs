using System.Collections.Generic;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings
{
    [StaticSeverityHighlighting(Severity.SUGGESTION, HighlightingGroupIds.BestPractice, OverlapResolve = OverlapResolveKind.WARNING)]
    internal class SuggestionHighlighting : IHighlightingWithRange, ICustomAttributeIdHighlighting
    {
        private const string AtributeId = HighlightingAttributeIds.SUGGESTION_ATTRIBUTE;
        private readonly ITreeNode startElement;
        private readonly ITreeNode endElement;
        private readonly string myMessage;

        public SuggestionHighlighting(ITreeNode startElement, ITreeNode endElement, string myMessage)
        {
            this.startElement = startElement;
            this.endElement = endElement;
            this.myMessage = myMessage;
        }

        public string AttributeId
        {
            get
            {
                return AtributeId;
            }
        }

        public string ErrorStripeToolTip
        {
            get
            {
                return myMessage;
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
                return this.myMessage;
            }
        }

        public DocumentRange CalculateRange()
        {
            var sourceFile = startElement.GetSourceFile();
            if (sourceFile != null)
            {
                var range = startElement.GetNavigationRange().JoinRight(endElement.GetNavigationRange());
                return range;
            }

            return DocumentRange.InvalidRange;
        }

        public bool IsValid()
        {
            return true;
        }
    }
}