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
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;
using JetBrains.ProjectModel;

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
                this.AddErrorHighlighting(consumer, "The URI is not well-formed", prefixUriParam);
                return;
            }

            if (!uriString.EndsWith("#"))
            {
                this.AddErrorHighlighting(consumer, "The prefix URI must end with the fragment symbol", prefixUriParam);
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
                this.AddErrorHighlighting(consumer, "The URI is not well-formed", uriStringParam);
                return;
            }

            var uri = new Uri(uriString);
            if (string.IsNullOrEmpty(uri.Fragment))
            {
                this.AddErrorHighlighting(consumer, "The URI does not have a fragment part", uriStringParam);
            }
        }

        public override void VisitUriIdentifier(IUriIdentifier uriIdentifierParam, IHighlightingConsumer consumer)
        {
            var cache = uriIdentifierParam.GetSolution().GetComponent<NTriplesCache>();
            var kind = uriIdentifierParam.GetKind();
            string uri;
            if (kind != IdentifierKind.Subject && !string.IsNullOrEmpty(uri = uriIdentifierParam.GetUri()) && !cache.HasSubjects(uri))
            {
                this.AddWarnHighlighting(consumer, string.Format("There are no statements where the URI {0} is a subject", uri), uriIdentifierParam);
            }
        }

        public override void VisitPrefixDeclaration(IPrefixDeclaration prefixDeclarationParam, IHighlightingConsumer consumer)
        {
            var cache = prefixDeclarationParam.GetSolution().GetComponent<NTriplesCache>();
            string prefixName;
            if (prefixDeclarationParam.PrefixName == null ||
                string.IsNullOrEmpty(prefixName = prefixDeclarationParam.PrefixName.GetText()))
            {
                return;
            }

            var sourceFile = prefixDeclarationParam.GetSourceFile();
            if (sourceFile == null)
            {
                return;
            }

            var occurences = cache.GetPrefixDeclarationSymbols(prefixName).Count(s => s.SourceFile.Equals(sourceFile));
            if (occurences > 1)
            {
                this.AddWarnHighlighting(consumer, string.Format("The prefix '{0}' is already declared", prefixName), prefixDeclarationParam.PrefixName);
            }
        }

        public override void VisitStatement(Tree.IStatement statementParam, IHighlightingConsumer consumer)
        {
            if (statementParam.KeywordStatement != null)
            {
                return;
            }

            var subjectUri = statementParam.Subject.ToUri();
            if (subjectUri == null)
            {
                return;
            }

            var cache = statementParam.GetSolution().GetComponent<NTriplesCache>();
            var types = cache.GetInstanceTypes(subjectUri).ToArray();
            var properties = cache.GetAvailableProperties(types).ToList();
            if (!properties.Any())
            {
                return;
            }

            foreach (var fact in statementParam.FactsEnumerable)
            {
                var propertyUri = fact.Predicate.ToUri();
                if (propertyUri != null && !properties.Contains(propertyUri))
                {
                    var classList = (types.Length > 1 ? "classes " : "class ") + string.Join(", ", types.Select(t => string.Format("<{0}>", t)));
                    this.AddWarnHighlighting(
                        consumer,
                        string.Format("Suspicious property declaration: the property is not defined in {0}.", classList),
                        fact.Predicate);
                }
            }
        }

        private void AddErrorHighlighting<TTreeNode>(IHighlightingConsumer consumer, string message, TTreeNode node)
            where TTreeNode : ITreeNode
        {
            var highlighting = new ErrorRangeHighlighting<TTreeNode>(node, node, message);
            IFile file = node.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(highlighting, file);
            }
        }

        private void AddWarnHighlighting<TTreeNode>(IHighlightingConsumer consumer, string message, TTreeNode node)
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
