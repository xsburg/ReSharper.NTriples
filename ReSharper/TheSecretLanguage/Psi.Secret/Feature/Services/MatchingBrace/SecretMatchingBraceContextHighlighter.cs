// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretMatchingBraceContextHighlighter.cs
// </summary>
// ***********************************************************************

namespace JetBrains.ReSharper.Psi.Secret.Feature.Services.MatchingBrace
{
#if !UNITTEST
    [CaretDependentFeature]
    public class SecretMatchingBraceContextHighlighter : MatchingBraceContextHighlighterBase
    {
        private readonly IContextActionDataProvider myDataProvider;

        public SecretMatchingBraceContextHighlighter(ISecretContextActionDataProvider dataProvider)
            : base(dataProvider)
        {
            this.myDataProvider = dataProvider;
        }

        protected override RichTextBlock GetHintText(ITextControl textControl, ITreeNode lBraceNode)
        {
            // TODO: Place here logic to display @in and @out declaration texts
            // the text below is CSharp highlighter example
            return null;
            /*if (lBraceNode == null || lBraceNode.GetTokenType() != SecretTokenType.L_BRACE)
                return null;
            /*if (lBraceNode.GetTokenType() == SecretTokenType.PP_SHARP)
            {
                ITreeNode treeNode = lBraceNode;
                do
                {
                    treeNode = treeNode.GetNextToken();
                    if (treeNode == null)
                        return null;
                }
                while (treeNode.GetTokenType() == SecretTokenType.WHITE_SPACE);
                if (treeNode.GetTokenType() == SecretTokenType.PP_START_REGION)
                    return MatchingBraceUtil.PrepareRichText(textControl, treeNode, lBraceNode, true);
                else
                    return null;
            }
            else#1#
            {
                ITreeNode parent1 = lBraceNode.Parent;
                if (parent1 is INamespaceBody)
                    return MatchingBraceUtil.PrepareRichText(textControl, parent1.Parent, lBraceNode, true);
                if (parent1 is IPropertyDeclaration || parent1 is IIndexerDeclaration || parent1 is IEventDeclaration)
                    return MatchingBraceUtil.PrepareRichText(textControl, parent1, lBraceNode, true);
                if (parent1.Parent is IClassLikeDeclaration && ((IClassLikeDeclaration)parent1.Parent).Body == parent1 || parent1.Parent is IEnumDeclaration && ((IEnumDeclaration)parent1.Parent).EnumBody == parent1 || (parent1 is IObjectInitializer || parent1 is ICollectionInitializer))
                    return MatchingBraceUtil.PrepareRichText(textControl, parent1.Parent, lBraceNode, true);
                if (parent1 is IBlock)
                {
                    IBlock block = (IBlock)parent1;
                    ITreeNode parent2 = parent1.Parent;
                    if (parent2 is ICSharpTypeMemberDeclaration || parent2 is IAccessorDeclaration)
                        return MatchingBraceUtil.PrepareRichText(textControl, parent2, lBraceNode, true);
                    if (BlockNavigator.GetByStatement((ICSharpStatement)block) == null)
                    {
                        /*if (parent2 is IIfStatement)
                        {
                            if (IfStatementNavigator.GetByThen((ICSharpStatement)block) != null)
                                return MatchingBraceUtil.PrepareRichText(textControl, parent2, lBraceNode, true);
                            if (IfStatementNavigator.GetByElse((ICSharpStatement)block) != null)
                            {
                                IIfStatement ifStatement = (IIfStatement)parent2;
                                RichTextBlock richTextBlock = MatchingBraceUtil.PrepareRichText(textControl, parent2, (ITreeNode)ifStatement.RPar, false);
                                RichTextBlock lines = MatchingBraceUtil.PrepareRichText(textControl, (ITreeNode)ifStatement.ElseKeyword, lBraceNode, true);
                                if (richTextBlock != null && lines != null)
                                {
                                    richTextBlock.AddLines(lines);
                                    return richTextBlock;
                                }
                            }
                        }
                        else#1#
                        {
                            if (!(parent2 is ITryStatement) || TryStatementNavigator.GetByTry(block) != null)
                                return MatchingBraceUtil.PrepareRichText(textControl, parent2, lBraceNode, true);
                            if (TryStatementNavigator.GetByFinallyBlock(block) != null)
                                return MatchingBraceUtil.PrepareRichText(textControl, (ITreeNode)((ITryStatement)parent2).FinallyKeyword, lBraceNode, true);
                        }
                    }
                }
                return (RichTextBlock)null;
            }*/
        }

        protected override bool IsLeftBracket(TokenNodeType tokenType)
        {
            return tokenType == SecretTokenType.L_BRACE || tokenType == SecretTokenType.L_PARENTHESES ||
                   tokenType == SecretTokenType.L_BRACKET || tokenType == SecretTokenType.URI_BEGIN;
        }

        protected override bool IsRightBracket(TokenNodeType tokenType)
        {
            return tokenType == SecretTokenType.R_BRACE || tokenType == SecretTokenType.R_PARENTHESES ||
                   tokenType == SecretTokenType.R_BRACKET || tokenType == SecretTokenType.URI_END;
        }

        protected override bool Match(TokenNodeType token1, TokenNodeType token2)
        {
            if (token1 == SecretTokenType.L_BRACE)
            {
                return token2 == SecretTokenType.R_BRACE;
            }
            if (token1 == SecretTokenType.L_PARENTHESES)
            {
                return token2 == SecretTokenType.R_PARENTHESES;
            }
            if (token1 == SecretTokenType.L_BRACKET)
            {
                return token2 == SecretTokenType.R_BRACKET;
            }
            if (token1 == SecretTokenType.URI_BEGIN)
            {
                return token2 == SecretTokenType.URI_END;
            }
            if (token1 == SecretTokenType.R_BRACE)
            {
                return token2 == SecretTokenType.L_BRACE;
            }
            if (token1 == SecretTokenType.R_PARENTHESES)
            {
                return token2 == SecretTokenType.L_PARENTHESES;
            }
            if (token1 == SecretTokenType.R_BRACKET)
            {
                return token2 == SecretTokenType.L_BRACKET;
            }
            if (token1 == SecretTokenType.URI_END)
            {
                return token2 == SecretTokenType.URI_BEGIN;
            }
            return false;
        }

        protected override void TryHighlightToLeft(MatchingHighlightingsConsumer consumer, ITokenNode selectedToken)
        {
            TokenNodeType tokenType = selectedToken.GetTokenType();
            if (this.IsRightBracket(tokenType))
            {
                ITokenNode matchedToken;
                if (this.FindMatchingLeftBracket(selectedToken, out matchedToken))
                {
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(
                        consumer, matchedToken.GetDocumentRange(), selectedToken.GetDocumentRange());
                    this.myLBraceForInvisibleBraceHint = matchedToken;
                    this.myRBraceForInvisibleBraceHint = selectedToken;
                }
                else
                {
                    consumer.ConsumeHighlighting(
                        "ReSharper Unmatched Brace", selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                    if (matchedToken == null)
                    {
                        return;
                    }

                    consumer.ConsumeHighlighting(
                        "ReSharper Unmatched Brace", matchedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1));
                }
            }
            else
            {
                if (tokenType != SecretTokenType.STRING_LITERAL)
                {
                    return;
                }
                if (selectedToken.GetText()[0] == 64)
                {
                    if (!(this.myDataProvider.TreeOffset == selectedToken.GetTreeTextRange().EndOffset))
                    {
                        return;
                    }
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(
                        consumer,
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1).Shift(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
                else
                {
                    if (!(this.myDataProvider.TreeOffset == selectedToken.GetTreeTextRange().EndOffset))
                    {
                        return;
                    }
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(
                        consumer,
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
            }
        }

        protected override void TryHighlightToRight(MatchingHighlightingsConsumer consumer, ITokenNode selectedToken)
        {
            TokenNodeType tokenType = selectedToken.GetTokenType();
            if (this.IsLeftBracket(tokenType))
            {
                ITokenNode matchedToken;
                if (this.FindMatchingRightBracket(selectedToken, out matchedToken))
                {
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(
                        consumer, selectedToken.GetDocumentRange(), matchedToken.GetDocumentRange());
                }
                else
                {
                    consumer.ConsumeHighlighting(
                        "ReSharper Unmatched Brace", selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1));
                    if (matchedToken == null)
                    {
                        return;
                    }
                    consumer.ConsumeHighlighting(
                        "ReSharper Unmatched Brace", matchedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
            }
            else
            {
                if (tokenType != SecretTokenType.STRING_LITERAL)
                {
                    return;
                }
                if (selectedToken.GetText()[0] == 64)
                {
                    if (!(this.myDataProvider.TreeOffset == selectedToken.GetTreeTextRange().StartOffset.Shift(1)))
                    {
                        return;
                    }
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(
                        consumer,
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1).Shift(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
                else
                {
                    if (!(this.myDataProvider.TreeOffset == selectedToken.GetTreeTextRange().StartOffset))
                    {
                        return;
                    }
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(
                        consumer,
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
            }
        }
    }
#endif
}
