// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesPrefixBuilder.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.LiveTemplates;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    public static class NTriplesPrefixBuilder
    {
        public static NTriplesIntentionResult Create(CreateNTriplesPrefixContext context)
        {
            var sentence = context.Declaration;

            sentence = NTriplesIntentionsUtil.AddToTarget(sentence, context.Target);

            var holders = new List<ITemplateFieldHolder>();

            var prefixDeclaration = (IPrefixDeclaration)sentence.Directive.FirstChild;
            var uriString = prefixDeclaration.UriString;
            var uri = uriString.GetText();
            var initialRange = uriString.GetNavigationRange().TextRange.StartOffset;
            holders.Add(
                new FindersTemplateFieldHolder(new TemplateField(uri, initialRange), new NTriplesBasicTemplateFinder(uriString)));

            return new NTriplesIntentionResult(
                holders,
                prefixDeclaration,
                context.Anchor,
                new DocumentRange(context.Document, sentence.GetNavigationRange().TextRange));
        }
    }

    public class NTriplesBasicTemplateFinder : ITemplateFieldFinder
    {
        private readonly ITreeNode myNode;

        public NTriplesBasicTemplateFinder(ITreeNode node)
        {
            this.myNode = node;
        }

        public IEnumerable<ITreeNode> Find(IDeclaration declaration)
        {
            yield return this.myNode;
        }
    }
}
