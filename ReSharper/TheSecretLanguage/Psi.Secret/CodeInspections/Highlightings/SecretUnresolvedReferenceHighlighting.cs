// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretUnresolvedReferenceHighlighting.cs
// </summary>
// ***********************************************************************

using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Secret.Tree;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings
{
    [ConfigurableSeverityHighlighting(
        "UnresolvedReference",
        SecretLanguage.LanguageName,
        OverlapResolve = OverlapResolveKind.UNRESOLVED_ERROR,
        ToolTipFormatString = Error)]
    internal class SecretUnresolvedReferenceHighlighting<TReference> : ICustomAttributeIdHighlighting, IHighlightingWithRange
    {
        private const string Error = "Unresolved prefix";
        private readonly string errorMessage;
        private readonly ISecretTreeNode myElement;

        public SecretUnresolvedReferenceHighlighting(ISecretTreeNode element, TReference reference, string errorMessage)
        {
            this.myElement = element;
            this.Reference = reference;
            this.errorMessage = errorMessage;
        }

        public string AttributeId
        {
            get
            {
                return HighlightingAttributeIds.UNRESOLVED_ERROR_ATTRIBUTE;
            }
        }

        public string ErrorStripeToolTip
        {
            get
            {
                return this.errorMessage;
            }
        }

        public int NavigationOffsetPatch
        {
            get
            {
                return 0;
            }
        }

        public TReference Reference { get; private set; }

        public string ToolTip
        {
            get
            {
                return this.errorMessage;
            }
        }

        public DocumentRange CalculateRange()
        {
            return this.myElement.GetNavigationRange();
        }

        public bool IsValid()
        {
            return true;
        }
    }
}