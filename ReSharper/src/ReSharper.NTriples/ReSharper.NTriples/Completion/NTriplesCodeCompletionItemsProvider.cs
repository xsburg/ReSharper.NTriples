// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCodeCompletionItemsProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using System.Linq;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Impl.Tree;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Completion
{
    [Language(typeof(SecretLanguage))]
    internal class NTriplesCodeCompletionItemsProvider
        : ItemsProviderWithReference<NTriplesCodeCompletionContext, SecretReferenceBase, SecretFile>
    {
        protected override void AddItemsGroups(
            NTriplesCodeCompletionContext context, GroupedItemsCollector collector, IntellisenseManager intellisenseManager)
        {
            collector.AddFilter(new ReferencesBetterFilter());
        }

        protected override void DecorateItems(NTriplesCodeCompletionContext context, System.Collections.Generic.IEnumerable<ILookupItem> items)
        {
            foreach (var item in items.OfType<DeclaredElementLookupItem>())
            {
                item.OrderingString = item.Text.ToLowerInvariant();
            }
        }

        protected override bool AddLookupItems(NTriplesCodeCompletionContext context, GroupedItemsCollector collector)
        {
            TextLookupRanges ranges;
            CodeCompletionContext basicContext = context.BasicContext;
            var file = basicContext.File as SecretFile;
            if (file == null)
            {
                return false;
            }

            if (context.BasicContext.CodeCompletionType != CodeCompletionType.AutomaticCompletion)
            {
                context.BasicContext.CompletionManager.PsiServices.PsiManager.UpdateCaches();
            }

            bool flag = false;
            SecretReferenceBase reference = this.GetReference(file, basicContext.SelectedTreeRange);
            if (reference != null)
            {
                ranges = this.EvaluateRanges(reference, null, context);
                collector.AddRanges(ranges);
                flag = this.EvaluateLookupItems(reference, null, context, collector, ranges);
            }
            else if (context.ReparsedContext.Reference is SecretPrefixReference)
            {
                // A special case for prefix suggestions without context (among whitespaces between sentences)
                reference = context.ReparsedContext.Reference as SecretReferenceBase;
                ranges = context.Ranges;
                collector.AddRanges(ranges);
                flag = this.EvaluateLookupItems(reference, null, context, collector, ranges);
            }
            else
            {
                ranges = this.EvaluateRangesWithoutReference(context);
            }

            var enumerable = this.GetReparseParameters(basicContext.SelectedRange.TextRange, reference, context);
            if (context.BasicContext.CodeCompletionType != CodeCompletionType.AutomaticCompletion)
            {
                foreach (var parameters in enumerable)
                {
                    if (this.EvaluateLookupItemsAfterReparse(parameters, context, collector, ranges))
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        protected override bool IsAvailable(NTriplesCodeCompletionContext context)
        {
            if (
                !((context.BasicContext.CodeCompletionType == CodeCompletionType.BasicCompletion) ||
                  (context.BasicContext.CodeCompletionType == CodeCompletionType.AutomaticCompletion)))
            {
                return false;
            }

            IReference reference = context.ReparsedContext.Reference;
            if (reference == null)
            {
                return false;
            }

            return true;
        }
    }
}
