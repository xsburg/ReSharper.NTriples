// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCodeCompletionKeywordsProvider.cs
// </summary>
// ***********************************************************************

using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Completion
{
    [Language(typeof(NTriplesLanguage))]
    public class NTriplesCodeCompletionKeywordsProvider : ItemsProviderOfSpecificContext<NTriplesCodeCompletionContext>
    {
        protected override void AddItemsGroups(
            NTriplesCodeCompletionContext context, GroupedItemsCollector collector, IntellisenseManager intellisenseManager)
        {
            collector.AddRanges(this.EvaluateRanges(context));
            collector.AddFilter(new KeywordsBetterFilter());
        }

        protected override bool AddLookupItems(NTriplesCodeCompletionContext context, GroupedItemsCollector collector)
        {
            var psiFile = context.BasicContext.File as INTriplesFile;
            if (psiFile == null)
            {
                return false;
            }

            var keywords =
                KeywordCompletionUtil.GetAplicableKeywords(psiFile, context.BasicContext.SelectedTreeRange, context)
                                     .Select(CreateKeywordLookupItem);
            foreach (TextLookupItemBase textLookupItem in keywords)
            {
                textLookupItem.InitializeRanges(context.Ranges, context.BasicContext);
                collector.AddToBottom(textLookupItem);
            }

            return true;
        }

        protected override bool IsAvailable(NTriplesCodeCompletionContext context)
        {
            var type = context.BasicContext.CodeCompletionType;
            var correctCompletionType = type == CodeCompletionType.AutomaticCompletion ||
                                        type == CodeCompletionType.BasicCompletion;
            if (!correctCompletionType)
            {
                return false;
            }

            var correctContext = context.ReparsedContext.Reference == null ||
                                 context.ReparsedContext.Reference is NTriplesPrefixReference;
            return correctContext;
        }

        private static TextLookupItemBase CreateKeywordLookupItem(string x)
        {
            return new NTriplesKeywordLookupItem(x, GetSuffix());
        }

        private static string GetSuffix()
        {
            return " ";
        }

        private TextLookupRanges EvaluateRanges(ISpecificCodeCompletionContext context)
        {
            var file = context.BasicContext.File as INTriplesFile;

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
