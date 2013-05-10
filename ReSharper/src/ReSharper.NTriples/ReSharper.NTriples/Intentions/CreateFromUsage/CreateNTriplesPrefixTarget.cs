// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CreateSecretPrefixTarget.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    public class CreateNTriplesPrefixTarget : ICreationTarget
    {
        private readonly ISentence myDeclaration;
        private readonly ITreeNode myElement;

        public CreateNTriplesPrefixTarget(NTriplesPrefixReference reference)
        {
            this.myElement = reference.GetTreeNode();
            string name = reference.GetName();

            var uri = this.TryFindUri(reference);
            this.myDeclaration =
                NTriplesElementFactory.GetInstance(this.myElement.GetPsiModule()).CreatePrefixDeclarationSentence(name, uri);
            this.Anchor = this.FindAnchor();
        }

        public ITreeNode Anchor { get; private set; }

        public ISentence Declaration
        {
            get
            {
                return this.myDeclaration;
            }
        }

        public IEnumerable<ITreeNode> GetPossibleTargetDeclarations()
        {
            yield return this.myDeclaration.Parent;
        }

        public ITreeNode GetTargetDeclaration()
        {
            return this.myDeclaration.Parent;
        }

        public IFile GetTargetDeclarationFile()
        {
            return this.myElement.GetContainingFile();
        }

        private ITreeNode FindAnchor()
        {
            var secretFile = (ISecretFile)this.myElement.GetContainingFile();
            if (secretFile == null)
            {
                throw new FormatException("The element has no file assigned.");
            }

            // Last prefix declaration or simply first sentence
            var anchor = secretFile.SentencesEnumerable.Reverse().FirstOrDefault(
                s =>
                {
                    var directive = s.FirstChild as IDirective;
                    return directive != null && directive.FirstChild is IPrefixDeclaration;
                }) ?? secretFile.SentencesEnumerable.First();

            return anchor;
        }

        private string TryFindUri(NTriplesPrefixReference reference)
        {
            var file = (ISecretFile)this.myElement.GetContainingFile();
            if (file == null)
            {
                throw new FormatException("The element has no file assigned.");
            }

            var name = reference.GetName();

            if (!string.IsNullOrEmpty(name))
            {
                // Looking for declarations of the same name
                var cache = this.myElement.GetSolution().GetComponent<NTriplesCache>();
                var uris = cache.GetPrefixDeclarationSymbols(name).Select(s => s.Uri).Distinct().ToArray();
                if (uris.Length == 0)
                {
                    return null;
                }

                if (uris.Length == 1)
                {
                    return uris[0];
                }

                // narrowing the search using local names
                var prefixesWithSameName =
                    new RecursiveElementCollector<IPrefixName>(p => p.GetText() == name).ProcessElement(file).GetResults();
                var localNamesInThePrefix =
                    prefixesWithSameName.Select(p => ((IUriIdentifier)p.Parent).GetLocalName())
                                        .Distinct()
                                        .Where(n => !string.IsNullOrEmpty(n))
                                        .ToArray();

                if (localNamesInThePrefix.Length == 0)
                {
                    return uris[0];
                }

                var bestUri = uris[0];
                int bestMetric = 0;
                foreach (var uri in uris)
                {
                    var metric =
                        cache.GetAllUriIdentifiersInNamespace(uri)
                             .Select(s => s.LocalName)
                             .Intersect(localNamesInThePrefix)
                             .Count();
                    if (metric > bestMetric)
                    {
                        bestMetric = metric;
                        bestUri = uri;
                    }
                }

                return bestUri;
            }

            return null;
        }
    }
}
