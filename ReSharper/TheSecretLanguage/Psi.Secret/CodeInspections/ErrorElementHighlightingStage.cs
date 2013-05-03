// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ErrorElementHighlightingStage.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.CodeAnnotations;
using JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class ErrorElementHighlightingStage : SecretDaemonStageBase
    {
        public ErrorElementHighlightingStage(CodeAnnotationsCache codeAnnotationsCache)
        {
        }

        public override IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            if (!this.IsSupported(process.SourceFile))
            {
                return EmptyList<IDaemonStageProcess>.InstanceList;
            }

            return new List<IDaemonStageProcess>
                {
                    new KeywordHighlightingProcess(process, settings)
                };
        }

        public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        private class KeywordHighlightingProcess : SecretIncrementalDaemonStageProcessBase
        {
            public KeywordHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
                : base(process, settingsStore)
            {
            }

            public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
            {
                var element = node as IErrorElement;
                if (element != null)
                {
                    if (element.GetTextLength() == 0)
                    {
                        ITreeNode parent = element.Parent;
                        while ((parent != null) && (parent.GetTextLength() == 0))
                        {
                            parent = parent.Parent;
                        }
                        if (parent != null)
                        {
                            this.AddHighlighting(consumer, parent);
                        }
                    }
                    else
                    {
                        this.AddHighlighting(consumer, element);
                    }
                }
            }

            private void AddHighlighting([NotNull] IHighlightingConsumer consumer, [NotNull] ITreeNode expression)
            {
                consumer.AddHighlighting(new SecretErrorElementHighlighting(expression), this.File);
            }
        }
    }
}
