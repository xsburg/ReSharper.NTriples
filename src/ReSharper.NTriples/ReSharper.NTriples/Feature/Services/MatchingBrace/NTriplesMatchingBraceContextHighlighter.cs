// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesMatchingBraceContextHighlighter.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.DataFlow;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon.CaretDependentFeatures;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextHighlighters;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.UI.RichText;
using JetBrains.Util;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.Feature.Services.MatchingBrace
{
#if !UNITTEST

    [ContainsContextConsumer]
    public class CSharpMatchingBraceContextHighlighter : MatchingBraceContextHighlighterBase
    {
        [AsyncContextConsumer]
        public static Action ProcessDataContext(
            Lifetime lifetime,
            [ContextKey(typeof(NTriplesContextActionDataProvider.ContextKey))] IContextActionDataProvider dataProvider,
            InvisibleBraceHintManager invisibleBraceHintManager,
            MatchingBraceSuggester matchingBraceSuggester)
        {
            return new CSharpMatchingBraceContextHighlighter().ProcessDataContextImpl(
                lifetime,
                dataProvider,
                invisibleBraceHintManager,
                matchingBraceSuggester);
        }

        protected override bool IsLeftBracket(TokenNodeType tokenType)
        {
            return tokenType == NTriplesTokenType.L_BRACE || tokenType == NTriplesTokenType.L_PARENTHESES ||
                   tokenType == NTriplesTokenType.L_BRACKET || tokenType == NTriplesTokenType.URI_BEGIN;
        }

        protected override bool IsRightBracket(TokenNodeType tokenType)
        {
            return tokenType == NTriplesTokenType.R_BRACE || tokenType == NTriplesTokenType.R_PARENTHESES ||
                   tokenType == NTriplesTokenType.R_BRACKET || tokenType == NTriplesTokenType.URI_END;
        }

        protected override bool Match(TokenNodeType token1, TokenNodeType token2)
        {
            if (token1 == NTriplesTokenType.L_BRACE)
            {
                return token2 == NTriplesTokenType.R_BRACE;
            }
            if (token1 == NTriplesTokenType.L_PARENTHESES)
            {
                return token2 == NTriplesTokenType.R_PARENTHESES;
            }
            if (token1 == NTriplesTokenType.L_BRACKET)
            {
                return token2 == NTriplesTokenType.R_BRACKET;
            }
            if (token1 == NTriplesTokenType.URI_BEGIN)
            {
                return token2 == NTriplesTokenType.URI_END;
            }
            if (token1 == NTriplesTokenType.R_BRACE)
            {
                return token2 == NTriplesTokenType.L_BRACE;
            }
            if (token1 == NTriplesTokenType.R_PARENTHESES)
            {
                return token2 == NTriplesTokenType.L_PARENTHESES;
            }
            if (token1 == NTriplesTokenType.R_BRACKET)
            {
                return token2 == NTriplesTokenType.L_BRACKET;
            }
            if (token1 == NTriplesTokenType.URI_END)
            {
                return token2 == NTriplesTokenType.URI_BEGIN;
            }
            return false;
        }


        private RichTextBlock GetHintText(ITextControl textControl, ITreeNode lBraceNode)
        {
            // TODO: Place here logic to display tooltip bubles that appears on the editor top side to display the opening statements like 'if (...) {' etc
            // implement logic to display @in and @out declaration texts
            // how to implement? Look at C# implementation for a start.
            return null;
        }

        protected override void TryHighlightToLeft(MatchingHighlightingsConsumer consumer, ITokenNode selectedToken, TreeOffset treeOffset)
        {
            TokenNodeType tokenType = selectedToken.GetTokenType();
            if (this.IsRightBracket(tokenType))
            {
                ITokenNode matchedToken;
                if (this.FindMatchingLeftBracket(selectedToken, out matchedToken))
                {
                    consumer.ConsumeMatchingBracesHighlighting(matchedToken.GetDocumentRange(), selectedToken.GetDocumentRange());
                    this.myLBraceDocumentRangeForIbh = matchedToken.GetDocumentRange();
                    this.myRBraceDocumentRangeForIbh = selectedToken.GetDocumentRange();
                    this.myHintTextGetter = textControl => this.GetHintText(textControl, matchedToken);
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
                if (tokenType != NTriplesTokenType.STRING_LITERAL)
                {
                    return;
                }
                if (selectedToken.GetText()[0] == 64)
                {
                    if (!(treeOffset == selectedToken.GetTreeTextRange().EndOffset))
                    {
                        return;
                    }

                    consumer.ConsumeMatchingBracesHighlighting(
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1).Shift(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
                else
                {
                    if (treeOffset != selectedToken.GetTreeTextRange().EndOffset)
                    {
                        return;
                    }

                    consumer.ConsumeMatchingBracesHighlighting(
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
            }
        }

        protected override void TryHighlightToRight(MatchingHighlightingsConsumer consumer, ITokenNode selectedToken, TreeOffset treeOffset)
        {
            TokenNodeType tokenType = selectedToken.GetTokenType();
            if (this.IsLeftBracket(tokenType))
            {
                ITokenNode matchedToken;
                if (this.FindMatchingRightBracket(selectedToken, out matchedToken))
                {
                    consumer.ConsumeMatchingBracesHighlighting(selectedToken.GetDocumentRange(), matchedToken.GetDocumentRange());
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
                if (tokenType != NTriplesTokenType.STRING_LITERAL)
                {
                    return;
                }
                if (selectedToken.GetText()[0] == 64)
                {
                    if (treeOffset != selectedToken.GetTreeTextRange().StartOffset.Shift(1))
                    {
                        return;
                    }

                    consumer.ConsumeMatchingBracesHighlighting(
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1).Shift(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
                else
                {
                    if (treeOffset != selectedToken.GetTreeTextRange().StartOffset)
                    {
                        return;
                    }

                    consumer.ConsumeMatchingBracesHighlighting(
                        selectedToken.GetDocumentRange().StartOffsetRange().ExtendRight(1),
                        selectedToken.GetDocumentRange().EndOffsetRange().ExtendLeft(1));
                }
            }
        }
    }
#endif
}
