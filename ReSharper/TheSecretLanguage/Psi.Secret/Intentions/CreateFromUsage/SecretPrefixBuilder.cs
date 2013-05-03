// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretPrefixBuilder.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.LiveTemplates;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Intentions.CreateFromUsage
{
    public static class SecretPrefixBuilder
    {
        public static SecretIntentionResult Create(CreateSecretPrefixContext context)
        {
            var sentence = context.Declaration;

            sentence = SecretIntentionsUtil.AddToTarget(sentence, context.Target);

            var holders = new List<ITemplateFieldHolder>();

            var prefixDeclaration = (IPrefixDeclaration)sentence.Directive.FirstChild;
            var uriString = prefixDeclaration.UriString;
            var uri = uriString.GetText();
            var initialRange = uriString.GetNavigationRange().TextRange.StartOffset;
            holders.Add(
                new FindersTemplateFieldHolder(new TemplateField(uri, initialRange), new SecretBasicTemplateFinder(uriString)));

            return new SecretIntentionResult(
                holders,
                prefixDeclaration,
                context.Anchor,
                new DocumentRange(context.Document, sentence.GetNavigationRange().TextRange));
        }
    }

    public class SecretBasicTemplateFinder : ITemplateFieldFinder
    {
        private readonly ITreeNode myNode;

        public SecretBasicTemplateFinder(ITreeNode node)
        {
            this.myNode = node;
        }

        public IEnumerable<ITreeNode> Find(IDeclaration declaration)
        {
            yield return this.myNode;
        }
    }
}
