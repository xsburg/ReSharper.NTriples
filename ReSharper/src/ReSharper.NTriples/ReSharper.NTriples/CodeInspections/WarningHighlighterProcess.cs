// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   WarningHighlighterProcess.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.CodeInspections
{
    internal class WarningHighlighterProcess : NTriplesIncrementalDaemonStageProcessBase
    {
        public WarningHighlighterProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, settingsStore)
        {
        }

        public override void VisitPrefixUri(IPrefixUri prefixUriParam, IHighlightingConsumer consumer)
        {
            var uriString = prefixUriParam.GetText();
            if (string.IsNullOrEmpty(uriString))
            {
                return;
            }

            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute) || new Uri(uriString).Scheme != Uri.UriSchemeHttp)
            {
                this.AddHighlighting(consumer, "The URI is not well-formed", prefixUriParam);
                return;
            }

            if (!uriString.EndsWith("#"))
            {
                this.AddHighlighting(consumer, "The prefix URI must end with the fragment symbol", prefixUriParam);
            }
        }

        public override void VisitUriString(IUriString uriStringParam, IHighlightingConsumer consumer)
        {
            var uriString = uriStringParam.GetText();
            if (string.IsNullOrEmpty(uriString))
            {
                return;
            }

            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute) || new Uri(uriString).Scheme != Uri.UriSchemeHttp)
            {
                this.AddHighlighting(consumer, "The URI is not well-formed", uriStringParam);
                return;
            }

            var uri = new Uri(uriString);
            if (string.IsNullOrEmpty(uri.Fragment))
            {
                this.AddHighlighting(consumer, "The URI does not have a fragment part", uriStringParam);
            }
        }

        private void AddHighlighting<TTreeNode>(IHighlightingConsumer consumer, string message, TTreeNode node)
            where TTreeNode : ITreeNode
        {
            var highlighting = new WarningRangeHighlighting<TTreeNode>(node, node, message);
            IFile file = node.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(highlighting, file);
            }
        }
    }
}
