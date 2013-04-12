// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretErrorElementHighlighting.cs
// </summary>
// ***********************************************************************

using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

/*[assembly: RegisterConfigurableSeverity("SyntaxError", null, HighlightingGroupIds.LanguageUsage, "Syntax Error", @"
          Syntax error", Severity.ERROR, false, Internal = false)]*/

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings
{
    [ConfigurableSeverityHighlighting("SyntaxError", "Secret", OverlapResolve = OverlapResolveKind.ERROR,
        ToolTipFormatString = Error)]
    internal class SecretErrorElementHighlighting : IHighlightingWithRange, ICustomAttributeIdHighlighting
    {
        private const string AtributeId = HighlightingAttributeIds.ERROR_ATTRIBUTE;
        private const string Error = "Syntax error";
        private readonly ITreeNode myElement;

        public SecretErrorElementHighlighting(ITreeNode element)
        {
            this.myElement = element;
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
                var errorMessage = this.myElement is IErrorElement
                                       ? string.Format("Syntax error: {0}.", (this.myElement as IErrorElement).ErrorDescription)
                                       : "Syntax error";
                return errorMessage;
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
                return ErrorStripeToolTip;
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
