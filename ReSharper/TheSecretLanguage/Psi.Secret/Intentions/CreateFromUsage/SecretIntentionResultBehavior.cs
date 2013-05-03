// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretIntentionResultBehavior.cs
// </summary>
// ***********************************************************************

using JetBrains.Application;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.Psi.Secret.Intentions.CreateFromUsage
{
    [ShellComponent]
    public class SecretIntentionResultBehavior
    {
        public SecretIntentionResultBehavior(HotspotSessionExecutor hotspotSessionExecutor)
        {
            this.HotspotSessionExecutor = hotspotSessionExecutor;
        }

        private HotspotSessionExecutor HotspotSessionExecutor { get; set; }

        public void OnHotspotSessionExecutionStarted(SecretIntentionResult result, ITextControl textControl)
        {
            this.OnHotspotSessionExecutionStartedInternal(result, textControl);
        }

        protected static void SetCaretPosition(ITextControl textControl, SecretIntentionResult result)
        {
            if (result.PreferredSelection != DocumentRange.InvalidRange)
            {
                textControl.Selection.SetRange(result.PreferredSelection.TextRange);
            }
        }

        protected virtual void OnHotspotSessionExecutionStartedInternal(SecretIntentionResult result, ITextControl textControl)
        {
            var hotspotSessionUi = this.HotspotSessionExecutor.CurrentSession;
            if (hotspotSessionUi == null)
            {
                SetCaretPosition(textControl, result);
            }
            else
            {
                hotspotSessionUi.HotspotSession.Closed += (session, type) =>
                {
                    if (type != TerminationType.Finished)
                    {
                        return;
                    }
                    SetCaretPosition(textControl, result);
                };
            }
        }
    }
}
