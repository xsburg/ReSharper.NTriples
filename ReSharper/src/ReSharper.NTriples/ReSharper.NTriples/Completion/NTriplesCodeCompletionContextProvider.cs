// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCodeCompletionContextProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Impl.Tree;

namespace ReSharper.NTriples.Completion
{
    [IntellisensePart]
    internal class NTriplesCodeCompletionContextProvider : CodeCompletionContextProviderBase
    {
        public override ISpecificCodeCompletionContext GetCompletionContext(CodeCompletionContext context)
        {
            var unterminatedContext = new NTriplesReparsedCompletionContext(context.File, context.SelectedTreeRange, "aaa");
            unterminatedContext.Init();
            IReference referenceToComplete = unterminatedContext.Reference;
            ITreeNode elementToComplete = unterminatedContext.TreeNode;
            if (elementToComplete == null)
            {
                return null;
            }

            TreeTextRange referenceRange = referenceToComplete != null
                                               ? referenceToComplete.GetTreeTextRange()
                                               : GetElementRange(elementToComplete);
            TextRange referenceDocumentRange = unterminatedContext.ToDocumentRange(referenceRange);
            if (!referenceDocumentRange.IsValid)
            {
                return null;
            }

            if (!referenceDocumentRange.Contains(context.CaretDocumentRange.TextRange))
            {
                return null;
            }

            TextLookupRanges ranges = GetTextLookupRanges(context, referenceDocumentRange);
            return new NTriplesCodeCompletionContext(context, ranges, unterminatedContext);
        }

        public override bool IsApplicable(CodeCompletionContext context)
        {
            var psiFile = context.File as SecretFile;
            return psiFile != null;
        }

        private static TreeTextRange GetElementRange(ITreeNode element)
        {
            var tokenNode = element as ITokenNode;

            if (tokenNode != null)
            {
                if (tokenNode.GetTokenType().IsIdentifier || tokenNode.GetTokenType().IsKeyword)
                {
                    return tokenNode.GetTreeTextRange();
                }
            }

            return new TreeTextRange(element.GetTreeTextRange().EndOffset);
        }
    }
}
