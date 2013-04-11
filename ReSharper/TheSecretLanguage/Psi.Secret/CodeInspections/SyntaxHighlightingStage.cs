// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   KeywordHighlightingStage.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
    [DaemonStage]
    public class SyntaxHighlightingStage : SecretDaemonStageBase
    {
        public override IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            if (!this.IsSupported(process.SourceFile))
            {
                return EmptyList<IDaemonStageProcess>.InstanceList;
            }

            return new[]
                {
                    new SyntaxHighlightingProcess(process, settings)
                };
        }

        public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        private class SyntaxHighlightingProcess : SecretIncrementalDaemonStageProcessBase
        {
            public SyntaxHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
                : base(process, settingsStore)
            {
            }

            public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
            {
                var token = node as SecretGenericToken;
                if (token != null)
                {
                    if (token.GetTokenType().IsStringLiteral)
                    {
                        this.AddSyntaxHighlighting(consumer, node, "String");
                    }
                    else if (token.GetTokenType().IsComment)
                    {
                        this.AddSyntaxHighlighting(consumer, node, "Comment");
                    }
                    else if (token.GetTokenType().IsKeyword)
                    {
                        this.AddSyntaxHighlighting(consumer, node, "Keyword");
                    }
                    else if (token.GetTokenType().IsIdentifier)
                    {
                        this.AddSyntaxHighlighting(consumer, node, HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE);
                    }
                    else if (token.GetTokenType().IsConstantLiteral)
                    {
                        this.AddSyntaxHighlighting(consumer, node, HighlightingAttributeIds.NAMESPACE_IDENTIFIER_ATTRIBUTE);
                    }
                }
            }

            private void AddHighlighting([NotNull] IHighlightingConsumer consumer, IHighlighting highlighting)
            {
                consumer.AddHighlighting(highlighting, this.File);
            }

            private void AddSyntaxHighlighting([NotNull] IHighlightingConsumer consumer, ITreeNode node, string attributeId)
            {
                consumer.AddHighlighting(new SecretSyntaxHighlighting(node, attributeId), this.File);
            }
        }
    }
}
