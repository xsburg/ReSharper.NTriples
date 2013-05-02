// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCodeCompletionItemsProvider.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Resolve;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [Language(typeof(SecretLanguage))]
    internal class SecretCodeCompletionItemsProvider
        : ItemsProviderWithReference<SecretCodeCompletionContext, SecretReferenceBase, SecretFile>
    {
        protected override bool IsAvailable(SecretCodeCompletionContext context)
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

        protected override string GetDisplayNameByDeclaredElement(DeclaredElementInstance<IDeclaredElement> declaredElementInstance, SecretReferenceBase reference, SecretCodeCompletionContext context)
        {
            var name = base.GetDisplayNameByDeclaredElement(declaredElementInstance, reference, context);
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return name.TrimEnd(':');
        }

        protected override bool AddLookupItems(SecretCodeCompletionContext context, GroupedItemsCollector collector)
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
            if (reference is SecretPrefixReference && context.ReparsedContext.Reference is SecretLocalNameReference)
            {
                // A special case for localName suggestion in the end of prefix
                reference = context.ReparsedContext.Reference as SecretReferenceBase;
                ranges = context.Ranges;
                collector.AddRanges(ranges);
                flag = this.EvaluateLookupItems(reference, null, context, collector, ranges);
            }
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
    }
}
