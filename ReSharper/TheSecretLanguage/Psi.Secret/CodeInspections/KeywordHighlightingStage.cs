// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   Class1.cs
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
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class KeywordHighlightingStage : SecretDaemonStageBase
    {
        public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
        {
            if (!IsSupported(process.SourceFile))
            {
                return EmptyList<IDaemonStageProcess>.InstanceList;
            }

            return new List<IDaemonStageProcess> { new KeywordHighlightingProcess(process, settings) };
        }

        #region Nested type: KeywordHighlightingProcess

        private class KeywordHighlightingProcess : SecretIncrementalDaemonStageProcessBase
        {
            public KeywordHighlightingProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
                : base(process, settingsStore)
            {
            }

            public override void VisitNode(ITreeNode node, IHighlightingConsumer consumer)
            {
                var keywordToken = node as ITokenNode;
                if ((keywordToken != null) && (SecretTokenType.KEYWORDS.Contains(keywordToken.GetTokenType())))
                {
                    //AddHighlighting(consumer, node);
                }
                else
                {
                    var token = node as SecretGenericToken;
                    if (token != null)
                    {
                        if (token.GetTokenType().IsStringLiteral)
                        {
                            AddHighlighting(consumer, new SecretStringLiteralHighlighting(node));
                        }
                        else if (token.GetTokenType().IsComment)
                        {
                            //AddHighlighting(consumer, new PsiCommentHighlighting(node));
                        }
                    }
                }
            }

            private void AddHighlighting([NotNull] IHighlightingConsumer consumer, [NotNull] ITreeNode expression)
            {
                //consumer.AddHighlighting(new PsiKeywordHighlighting(expression), File);
            }

            private void AddHighlighting([NotNull] IHighlightingConsumer consumer, IHighlighting highlighting)
            {
                consumer.AddHighlighting(highlighting, File);
            }
        }

        #endregion
    }
}