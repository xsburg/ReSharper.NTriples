// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCompletionItemsProviderKeywords.cs
// </summary>
// ***********************************************************************

using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [Language(typeof(SecretLanguage))]
    public class SecretCompletionItemsProviderKeywords : ItemsProviderOfSpecificContext<SecretCodeCompletionContext>
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
                KeywordCompletionUtil.GetAplicableKeywords(secretFile, context.BasicContext.SelectedTreeRange)
                                     .Select(CreateKeyworkLookupItem);
            foreach (TextLookupItemBase textLookupItem in keywords)
            {
                textLookupItem.InitializeRanges(context.Ranges, context.BasicContext);
                collector.AddAtDefaultPlace(textLookupItem);
            }

            return true;
        }

        protected override bool IsAvailable(SecretCodeCompletionContext context)
        {
            CodeCompletionType type = context.BasicContext.CodeCompletionType;
            return type == CodeCompletionType.AutomaticCompletion || type == CodeCompletionType.BasicCompletion;
        }

        private static TextLookupItemBase CreateKeyworkLookupItem(string x)
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
