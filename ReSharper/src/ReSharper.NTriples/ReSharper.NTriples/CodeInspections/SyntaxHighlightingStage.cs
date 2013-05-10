// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SyntaxHighlightingStage.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.CodeInspections
{
    [DaemonStage]
    public class SyntaxHighlightingStage : NTriplesDaemonStageBase
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

        private class SyntaxHighlightingProcess : NTriplesIncrementalDaemonStageProcessBase
        {
            public SyntaxHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
                : base(process, settingsStore)
            {
            }

            public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
            {
                // Tree level highlighting
                //
                const string prefixHighlighting = HighlightingAttributeIds.NAMESPACE_IDENTIFIER_ATTRIBUTE;
                if (node is IPrefix || node is IPrefixName)
                {
                    this.AddSyntaxHighlighting(consumer, node, prefixHighlighting);
                }
                else if (node is ILocalName)
                {
                    this.AddSyntaxHighlighting(consumer, node, HighlightingAttributeIds.METHOD_IDENTIFIER_ATTRIBUTE);
                }
                else if (node is IVariableIdentifier)
                {
                    this.AddSyntaxHighlighting(consumer, node, HighlightingAttributeIds.FIELD_IDENTIFIER_ATTRIBUTE);
                }
                else if (node is ITokenNode)
                {
                    // Token level highlighting
                    // 
                    var token = node as ITokenNode;
                    if (token.GetTokenType().IsStringLiteral)
                    {
                        if (token.Parent is IDataLiteral && token.Parent.LastChild != token)
                        {
                            this.AddSyntaxHighlighting(consumer, node, VsPredefinedHighlighterIds.String);
                        }
                        else
                        {
                            this.AddSyntaxHighlighting(consumer, node, VsPredefinedHighlighterIds.String);
                        }
                    }
                    else if (token.GetTokenType().IsComment)
                    {
                        this.AddSyntaxHighlighting(consumer, node, VsPredefinedHighlighterIds.Comment);
                    }
                    else if (token.GetTokenType().IsKeyword)
                    {
                        this.AddSyntaxHighlighting(consumer, node, VsPredefinedHighlighterIds.Keyword);
                    }
                    else if (token.GetTokenType().IsConstantLiteral)
                    {
                        //this.AddSyntaxHighlighting(consumer, node, VsPredefinedHighlighterIds.Literal);
                    }
                    else if (token.GetTokenType() == SecretTokenType.NAMESPACE_SEPARATOR)
                    {
                        this.AddSyntaxHighlighting(consumer, node, prefixHighlighting);
                    }
                }
            }

            private void AddHighlighting([NotNull] IHighlightingConsumer consumer, IHighlighting highlighting)
            {
                consumer.AddHighlighting(highlighting, this.File);
            }

            private void AddSyntaxHighlighting([NotNull] IHighlightingConsumer consumer, ITreeNode node, string attributeId)
            {
                consumer.AddHighlighting(new NTriplesSyntaxHighlighting(node, attributeId), this.File);
            }
        }
    }
}
