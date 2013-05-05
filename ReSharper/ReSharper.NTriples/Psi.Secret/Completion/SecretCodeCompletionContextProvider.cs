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
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [IntellisensePart]
    internal class SecretCodeCompletionContextProvider : CodeCompletionContextProviderBase
    {
        public override ISpecificCodeCompletionContext GetCompletionContext(CodeCompletionContext context)
        {
            var unterminatedContext = new SecretReparsedCompletionContext(context.File, context.SelectedTreeRange, "aaa");
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
            return new SecretCodeCompletionContext(context, ranges, unterminatedContext);
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
