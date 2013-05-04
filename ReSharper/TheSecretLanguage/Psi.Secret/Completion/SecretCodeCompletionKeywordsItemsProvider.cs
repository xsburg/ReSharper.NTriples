// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCodeCompletionKeywordsItemsProvider.cs
// </summary>
// ***********************************************************************

using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [Language(typeof(SecretLanguage))]
    public class SecretCodeCompletionKeywordsItemsProvider : ItemsProviderOfSpecificContext<SecretCodeCompletionContext>
    {
        protected override void AddItemsGroups(
            SecretCodeCompletionContext context, GroupedItemsCollector collector, IntellisenseManager intellisenseManager)
        {
            collector.AddRanges(this.EvaluateRanges(context));
            collector.AddFilter(new KeywordsBetterFilter());
        }

        protected override bool AddLookupItems(SecretCodeCompletionContext context, GroupedItemsCollector collector)
        {
            var secretFile = context.BasicContext.File as ISecretFile;
            if (secretFile == null)
            {
                return false;
            }

            var keywords =
                KeywordCompletionUtil.GetAplicableKeywords(secretFile, context.BasicContext.SelectedTreeRange, context)
                                     .Select(CreateKeywordLookupItem);
            foreach (TextLookupItemBase textLookupItem in keywords)
            {
                textLookupItem.InitializeRanges(context.Ranges, context.BasicContext);
                collector.AddToBottom(textLookupItem);
            }

            return true;
        }

        protected override bool IsAvailable(SecretCodeCompletionContext context)
        {
            var type = context.BasicContext.CodeCompletionType;
            var correctCompletionType = type == CodeCompletionType.AutomaticCompletion ||
                                        type == CodeCompletionType.BasicCompletion;
            if (!correctCompletionType)
            {
                return false;
            }

            var correctContext = context.ReparsedContext.Reference == null ||
                                 context.ReparsedContext.Reference is SecretPrefixReference;
            return correctContext;
        }

        private static TextLookupItemBase CreateKeywordLookupItem(string x)
        {
            return new SecretKeywordLookupItem(x, GetSuffix());
        }

        private static string GetSuffix()
        {
            return " ";
        }

        private TextLookupRanges EvaluateRanges(ISpecificCodeCompletionContext context)
        {
            var file = context.BasicContext.File as ISecretFile;

            DocumentRange selectionRange = context.BasicContext.SelectedRange;

            if (file != null)
            {
                var token = file.FindNodeAt(selectionRange) as ITokenNode;

                if (token != null)
                {
                    DocumentRange tokenRange = token.GetNavigationRange();

                    var insertRange = new TextRange(tokenRange.TextRange.StartOffset, selectionRange.TextRange.EndOffset);
                    var replaceRange = new TextRange(
                        tokenRange.TextRange.StartOffset,
                        Math.Max(tokenRange.TextRange.EndOffset, selectionRange.TextRange.EndOffset));

                    return new TextLookupRanges(insertRange, false, replaceRange);
                }
            }

            return new TextLookupRanges(TextRange.InvalidRange, false, TextRange.InvalidRange);
        }
    }
}
