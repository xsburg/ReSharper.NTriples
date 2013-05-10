// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ContextErrorHighlighterProcess.cs
// </summary>
// ***********************************************************************

using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Impl.Tree;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.CodeInspections
{
    internal class ContextErrorHighlighterProcess : NTriplesIncrementalDaemonStageProcessBase
    {
        public ContextErrorHighlighterProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, settingsStore)
        {
        }

        public override void VisitPrefix(IPrefix prefixParam, IHighlightingConsumer consumer)
        {
            DocumentRange range = prefixParam.GetDocumentRange();
            var prefix = prefixParam as Prefix;
            if (prefix != null)
            {
                ResolveResultWithInfo resolve = prefix.Resolve();
                if (resolve == null ||
                    resolve.Result.DeclaredElement is UnresolvedNamespacePrefixDeclaredElement ||
                    ((resolve.Result.DeclaredElement == null) && (resolve.Result.Candidates.Count == 0)))
                {
                    this.AddHighLighting(
                        range,
                        prefixParam,
                        consumer,
                        new NTriplesUnresolvedReferenceHighlighting<NTriplesPrefixReference>(
                            prefix, prefix.PrefixReference, string.Format("Unresolved prefix '{0}'", prefix.GetText())));
                }
            }
        }

        private void AddHighLighting(
            DocumentRange range, ITreeNode element, IHighlightingConsumer consumer, IHighlighting highlighting)
        {
            var info = new HighlightingInfo(range, highlighting, new Severity?());
            IFile file = element.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(info.Highlighting, file);
            }
        }
    }
}
