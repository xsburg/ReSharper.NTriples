using System.Diagnostics;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextHighlighters;
using JetBrains.ReSharper.Feature.Services.MatchingBrace;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.UI.RichText;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Services
{
    [CaretDependentFeature]
    public class SecretMatchingBraceContextHighlighter : MatchingBraceContextHighlighterBase
    {
        private readonly IContextActionDataProvider myDataProvider;

        public SecretMatchingBraceContextHighlighter(ICSharpContextActionDataProvider dataProvider)
            : base((IContextActionDataProvider)dataProvider)
        {
            this.myDataProvider = dataProvider;
        }

        protected override void TryHighlightToLeft(MatchingHighlightingsConsumer consumer, ITokenNode selectedToken)
        {
            TokenNodeType tokenType = selectedToken.GetTokenType();
            /*if (tokenType == SecretTokenType.PP_SHARP)
            {
                do
                {
                    selectedToken = TokenNodeExtesions.GetNextToken(selectedToken);
                    if (selectedToken == null)
                        return;
                }
                while (selectedToken.GetTokenType() == SecretTokenType.WHITE_SPACE);
                tokenType = selectedToken.GetTokenType();
            }*/
            if (this.IsRightBracket(tokenType))
            {
                ITokenNode matchedToken;
                if (this.FindMatchingLeftBracket(selectedToken, out matchedToken))
                {
                    /*if (matchedToken.GetTokenType() == SecretTokenType.PP_START_REGION)
                    {
                        do
                        {
                            matchedToken = TokenNodeExtesions.GetPrevToken(matchedToken);
                        }
                        while (matchedToken.GetTokenType() == SecretTokenType.WHITE_SPACE);
                    }*/
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(consumer, TreeNodeExtensions.GetDocumentRange((ITreeNode)matchedToken), TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken));
                    this.myLBraceForInvisibleBraceHint = matchedToken;
                    this.myRBraceForInvisibleBraceHint = selectedToken;
                }
                else
                {
                    consumer.ConsumeHighlighting("ReSharper Unmatched Brace", TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).EndOffsetRange().ExtendLeft(1));
                    if (matchedToken == null)
                        return;
                    /*if (matchedToken.GetTokenType() == SecretTokenType.PP_START_REGION)
                    {
                        do
                        {
                            matchedToken = TokenNodeExtesions.GetPrevToken(matchedToken);
                        }
                        while (matchedToken.GetTokenType() == SecretTokenType.WHITE_SPACE);
                    }*/
                    consumer.ConsumeHighlighting("ReSharper Unmatched Brace", TreeNodeExtensions.GetDocumentRange((ITreeNode)matchedToken).StartOffsetRange().ExtendRight(1));
                }
            }
            else
            {
                if (tokenType != SecretTokenType.STRING_LITERAL)
                    return;
                if ((int)selectedToken.GetText()[0] == 64)
                {
                    if (!(this.myDataProvider.TreeOffset == TreeNodeExtensions.GetTreeTextRange((ITreeNode)selectedToken).EndOffset))
                        return;
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(consumer, TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).StartOffsetRange().ExtendRight(1).Shift(1), TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).EndOffsetRange().ExtendLeft(1));
                }
                else
                {
                    if (!(this.myDataProvider.TreeOffset == TreeNodeExtensions.GetTreeTextRange((ITreeNode)selectedToken).EndOffset))
                        return;
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(consumer, TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).StartOffsetRange().ExtendRight(1), TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).EndOffsetRange().ExtendLeft(1));
                }
            }
        }

        protected override void TryHighlightToRight(MatchingHighlightingsConsumer consumer, ITokenNode selectedToken)
        {
            TokenNodeType tokenType = selectedToken.GetTokenType();
            /*if (tokenType == SecretTokenType.PP_SHARP)
            {
                do
                {
                    selectedToken = TokenNodeExtesions.GetNextToken(selectedToken);
                    if (selectedToken == null)
                        return;
                }
                while (selectedToken.GetTokenType() == SecretTokenType.WHITE_SPACE);
                tokenType = selectedToken.GetTokenType();
            }*/
            if (this.IsLeftBracket(tokenType))
            {
                ITokenNode matchedToken;
                if (this.FindMatchingRightBracket(selectedToken, out matchedToken))
                {
                    /*if (selectedToken.GetTokenType() == SecretTokenType.PP_START_REGION)
                    {
                        do
                        {
                            selectedToken = TokenNodeExtesions.GetPrevToken(selectedToken);
                        }
                        while (selectedToken.GetTokenType() == SecretTokenType.WHITE_SPACE);
                    }*/
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(consumer, TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken), TreeNodeExtensions.GetDocumentRange((ITreeNode)matchedToken));
                }
                else
                {
                    /*if (selectedToken.GetTokenType() == SecretTokenType.PP_START_REGION)
                    {
                        do
                        {
                            selectedToken = TokenNodeExtesions.GetPrevToken(selectedToken);
                        }
                        while (selectedToken.GetTokenType() == SecretTokenType.WHITE_SPACE);
                    }*/
                    consumer.ConsumeHighlighting("ReSharper Unmatched Brace", TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).StartOffsetRange().ExtendRight(1));
                    if (matchedToken == null)
                        return;
                    consumer.ConsumeHighlighting("ReSharper Unmatched Brace", TreeNodeExtensions.GetDocumentRange((ITreeNode)matchedToken).EndOffsetRange().ExtendLeft(1));
                }
            }
            else
            {
                if (tokenType != SecretTokenType.STRING_LITERAL)
                    return;
                if ((int)selectedToken.GetText()[0] == 64)
                {
                    if (!(this.myDataProvider.TreeOffset == TreeNodeExtensions.GetTreeTextRange((ITreeNode)selectedToken).StartOffset.Shift(1)))
                        return;
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(consumer, TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).StartOffsetRange().ExtendRight(1).Shift(1), TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).EndOffsetRange().ExtendLeft(1));
                }
                else
                {
                    if (!(this.myDataProvider.TreeOffset == TreeNodeExtensions.GetTreeTextRange((ITreeNode)selectedToken).StartOffset))
                        return;
                    MatchingBracesContextHighlightersUtil.ConsumeMatchingBracesHighlighting(consumer, TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).StartOffsetRange().ExtendRight(1), TreeNodeExtensions.GetDocumentRange((ITreeNode)selectedToken).EndOffsetRange().ExtendLeft(1));
                }
            }
        }

        protected override bool IsLeftBracket(TokenNodeType tokenType)
        {
            return tokenType == SecretTokenType.L_BRACE || tokenType == SecretTokenType.L_PARENTHESES || tokenType == SecretTokenType.L_BRACKET;
        }

        protected override bool IsRightBracket(TokenNodeType tokenType)
        {
            return tokenType == SecretTokenType.R_BRACE || tokenType == SecretTokenType.R_PARENTHESES || tokenType == SecretTokenType.R_BRACKET;
        }

        protected override bool Match(TokenNodeType token1, TokenNodeType token2)
        {
            if (token1 == SecretTokenType.L_BRACE)
                return token2 == SecretTokenType.R_BRACE;
            if (token1 == SecretTokenType.L_PARENTHESES)
                return token2 == SecretTokenType.R_PARENTHESES;
            if (token1 == SecretTokenType.L_BRACKET)
                return token2 == SecretTokenType.R_BRACKET;
            if (token1 == SecretTokenType.R_BRACE)
                return token2 == SecretTokenType.L_BRACE;
            if (token1 == SecretTokenType.R_PARENTHESES)
                return token2 == SecretTokenType.L_PARENTHESES;
            if (token1 == SecretTokenType.R_BRACKET)
                return token2 == SecretTokenType.L_BRACKET;
            else
                return false;
        }

        protected override RichTextBlock GetHintText(ITextControl textControl, ITreeNode lBraceNode)
        {
            return null;
            /*if (lBraceNode == null || TreeNodeExtensions.GetTokenType(lBraceNode) != SecretTokenType.L_BRACE)
                return (RichTextBlock)null;
            if (TreeNodeExtensions.GetTokenType(lBraceNode) == SecretTokenType.PP_SHARP)
            {
                ITreeNode treeNode = lBraceNode;
                do
                {
                    treeNode = (ITreeNode)TreeNodeExtensions.GetNextToken(treeNode);
                    if (treeNode == null)
                        return (RichTextBlock)null;
                }
                while (TreeNodeExtensions.GetTokenType(treeNode) == SecretTokenType.WHITE_SPACE);
                if (TreeNodeExtensions.GetTokenType(treeNode) == SecretTokenType.PP_START_REGION)
                    return MatchingBraceUtil.PrepareRichText(textControl, treeNode, lBraceNode, true);
                else
                    return (RichTextBlock)null;
            }
            else
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
                        if (parent2 is IIfStatement)
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
                        else
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
    }
}