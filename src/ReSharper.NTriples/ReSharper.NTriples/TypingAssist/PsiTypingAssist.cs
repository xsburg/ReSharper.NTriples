// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiTypingAssist.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.Application;
using JetBrains.Application.CommandProcessing;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Options;
using JetBrains.ReSharper.Feature.Services.TypingAssist;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.TextControl.DataConstants;
using JetBrains.TextControl.Util;
using JetBrains.Util;
using JetBrains.Util.Logging;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Parsing;

namespace ReSharper.NTriples.TypingAssist
{
    /*public abstract class CSharpTypingAssistBase : TypingAssistForCLikeLanguage<NTriplesLanguage, NTriplesCodeFormatter>, ITypingHandler
    {
        private static readonly NodeTypeSet TypeParamTokens = new NodeTypeSet(new NodeType[10]
    {
      (NodeType) NTriplesTokenType.IDENTIFIER,
      (NodeType) NTriplesTokenType.LT,
      (NodeType) NTriplesTokenType.GT,
      (NodeType) NTriplesTokenType.ASTERISK,
      (NodeType) NTriplesTokenType.LBRACKET,
      (NodeType) NTriplesTokenType.RBRACKET,
      (NodeType) NTriplesTokenType.DOUBLE_COLON,
      (NodeType) NTriplesTokenType.DOT,
      (NodeType) NTriplesTokenType.COMMA,
      (NodeType) NTriplesTokenType.QUEST
    });

        static CSharpTypingAssistBase()
        {
        }

        protected CSharpTypingAssistBase(Lifetime lifetime, ISolution solution, ICommandProcessor commandProcessor, CachingLexerService cachingLexerService, ISettingsStore settingsStore, ITypingAssistManager typingAssistManager, IPsiServices psiServices)
            : base(solution, settingsStore, cachingLexerService, commandProcessor, psiServices)
        {
            typingAssistManager.AddTypingHandler(lifetime, '{', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleLeftBraceTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartLBraceHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '}', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleRightBraceTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '(', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleLeftBracketOrParenthTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '[', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleLeftBracketOrParenthTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '>', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleRightBracketTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, ']', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleRightBracketTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, ')', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleRightBracketTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '"', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleQuoteTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '\'', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleQuoteTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingSmartParenthesisHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, ';', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleSemicolonTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingHandlerAvailable));
            typingAssistManager.AddTypingHandler(lifetime, '/', (ITypingHandler)this, new Func<ITypingContext, bool>(this.HandleDivideTyped), new Predicate<ITypingContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsTypingHandlerAvailable));
            typingAssistManager.AddActionHandler(lifetime, "TextControl.Enter", (ITypingHandler)this, new Func<IActionContext, bool>(this.HandleEnterPressed), new Predicate<IActionContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsActionHandlerAvailabile));
            typingAssistManager.AddActionHandler(lifetime, "TextControl.Backspace", (ITypingHandler)this, new Func<IActionContext, bool>(this.HandleBackspacePressed), new Predicate<IActionContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsActionHandlerAvailabile));
            typingAssistManager.AddActionHandler(lifetime, "TextControl.Delete", (ITypingHandler)this, new Func<IActionContext, bool>(this.HandleDelPressed), new Predicate<IActionContext>(((TypingAssistLanguageBase<CSharpLanguage, ICSharpCodeFormatter>)this).IsActionHandlerAvailabile));
        }

        protected virtual bool NeedAutoinsertCloseBracket(ITextControl textControl, CachingLexer lexer)
        {
            using (LexerStateCookie.Create<int>((ILexer<int>)lexer))
            {
                TokenNodeType tokenType1 = lexer.TokenType;
                TokenNodeType tokenType2 = tokenType1;
                int currentTokenIndex = lexer.CurrentTokenIndex;
                int currentPosition = lexer.CurrentPosition;
                if (tokenType1 == NTriplesTokenType.LBRACE)
                {
                    var csharpBraceMatcher = new NTriplesBraceMatcher();
                    lexer.Start();
                    bool flag = false;
                    do
                    {
                        NTriplesBraceMatcher.ProceedStack(lexer.TokenType);
                        if (lexer.CurrentTokenIndex == currentTokenIndex)
                            flag = true;
                        lexer.Advance();
                    }
                    while (lexer.TokenType != null && (!flag || !csharpBraceMatcher.IsStackEmpty()));
                    lexer.CurrentPosition = currentPosition;
                    return !flag || !csharpBraceMatcher.IsStackEmpty();
                }
                else
                {
                    BracketMatcher bracketMatcher = (BracketMatcher)new CSharpBracketMatcher();
                    do
                    {
                        if (tokenType2 == tokenType1 && bracketMatcher.IsStackEmpty())
                            currentPosition = lexer.CurrentPosition;
                        else if (!bracketMatcher.ProceedStack(tokenType2))
                            break;
                        lexer.Advance(-1);
                    }
                    while ((tokenType2 = lexer.TokenType) != null);
                    lexer.CurrentPosition = currentPosition;
                    if (tokenType1 == NTriplesTokenType.LBRACE)
                        bracketMatcher = (BracketMatcher)new CSharpBraceMatcher();
                    return !bracketMatcher.FindMatchingBracket(lexer);
                }
            }
        }

        protected bool HandleLeftBraceTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart LBRACE"))
            {
                this.InsertLeftBrace(typingContext);
                int pos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret) - 1);
                CachingLexer cachingLexer = this.GetCachingLexer(textControl);
                if (pos < 0 || !cachingLexer.FindTokenAt(pos) || cachingLexer.TokenStart != pos || !this.NeedAutoinsertCloseBracket(textControl, cachingLexer) || typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                    return true;
                this.AutoinsertRBrace(textControl, cachingLexer);
                int offset = this.LexerToTextControl(textControl, pos + 1);
                if (offset >= 0)
                    ITextControlCaretEx.MoveTo(textControl.Caret, offset, CaretVisualPlacement.DontScrollIfVisible);
            }
            return true;
        }

        protected virtual void InsertLeftBrace(ITypingContext typingContext)
        {
            typingContext.CallNext();
        }

        private bool AutoinsertRBrace(ITextControl textControl, CachingLexer lexer)
        {
            int offset1 = lexer.TokenEnd - 1;
            if (!this.IsLBrace(textControl, lexer) || !this.NeedAutoinsertCloseBracket(textControl, lexer))
                return false;
            IDocument document = textControl.Document;
            int startOffset = this.LexerToTextControl(textControl, offset1);
            if (startOffset < 0)
                return false;
            document.InsertText(startOffset + 1, "}");
            IFile file = this.CommitPsi(textControl);
            if (file == null)
                return false;
            TreeTextRange range = FileExtensions.Translate(file, new DocumentRange(document, new TextRange(offset1 + 1)));
            if (!range.IsValid())
                return true;
            ITokenNode tokenNode1 = file.FindTokenAt(range.StartOffset) as ITokenNode;
            if (tokenNode1 == null || !base.IsRBrace(textControl, (ITreeNode)tokenNode1))
                return false;
            if (tokenNode1.GetTokenType() != NTriplesTokenType.RBRACE)
                return true;
            TreeOffset treeOffset = TreeNodeExtensions.GetTreeTextRange((ITreeNode)tokenNode1).EndOffset;
            string text = "}";
            DocumentRange parsedDocumentRange = FileExtensions.GetParsedDocumentRange(file, range);
            if (!parsedDocumentRange.IsValid())
                return false;
            Int32<DocLine> line = parsedDocumentRange.Document.GetCoordsByOffset(parsedDocumentRange.TextRange.StartOffset).Line;
            if (tokenNode1.Parent is IArrayInitializer)
            {
                IDeclarationStatement variableDeclaration = DeclarationStatementNavigator.GetByVariableDeclaration(LocalVariableDeclarationNavigator.GetByInitial(tokenNode1.Parent as IVariableInitializer));
                if (variableDeclaration != null && variableDeclaration.Semicolon == null)
                {
                    ITreeNode parent = variableDeclaration.Parent;
                    ITreeNode child = (ITreeNode)variableDeclaration;
                    ITokenNode semicolon;
                    while (true)
                    {
                        child = TreeNodeExtensions.GetNextMeaningfulChild(parent, child);
                        IExpressionStatement expressionStatement = child as IExpressionStatement;
                        if (expressionStatement != null)
                        {
                            semicolon = expressionStatement.Semicolon;
                            if (semicolon == null)
                                treeOffset = TreeNodeExtensions.GetTreeTextRange((ITreeNode)expressionStatement).EndOffset;
                            else
                                break;
                        }
                        else
                            goto label_40;
                    }
                    treeOffset = semicolon.GetTreeStartOffset();
                }
            }
            else if (LambdaExpressionNavigator.GetByBodyBlock(tokenNode1.Parent as IBlock) != null)
            {
                int endOffset = this.LexerToTextControl(textControl, offset1 + 2);
                if (endOffset < 0)
                    return false;
                TextRange fromRange = new TextRange(startOffset, endOffset);
                document.DeleteText(fromRange);
                try
                {
                    file = this.CommitPsi(textControl);
                    if (file == null)
                        return false;
                    ITokenNode tokenNode2 = file.FindTokenAt(new TreeOffset(offset1 + 1)) as ITokenNode;
                    if (tokenNode2 == null)
                        return false;
                    ILambdaExpression containingNode = tokenNode2.GetContainingNode<ILambdaExpression>(true);
                    if (containingNode == null)
                        return false;
                    ICSharpExpression bodyExpression = containingNode.BodyExpression;
                    if (bodyExpression == null || TreeNodeExtensions.GetDocumentRange((ITreeNode)bodyExpression).Document.GetCoordsByOffset(bodyExpression.GetTreeStartOffset().Offset).Line != line)
                        return false;
                    treeOffset = TreeNodeExtensions.GetTreeTextRange((ITreeNode)bodyExpression).EndOffset + 2;
                }
                finally
                {
                    document.InsertText(fromRange.StartOffset, "{}");
                }
            }
            else
            {
                ICSharpStatement statementOnSameLine;
                do
                {
                    statementOnSameLine = CSharpTypingAssistBase.FindNextStatementOnSameLine(parsedDocumentRange.Document, file, treeOffset.Offset);
                    if (statementOnSameLine != null)
                    {
                        if (!(statementOnSameLine is IEmptyStatement))
                        {
                            treeOffset = TreeNodeExtensions.GetTreeTextRange((ITreeNode)statementOnSameLine).EndOffset;
                            ITokenNode lastTokenIn = TreeNodeExtensions.FindLastTokenIn((ITreeNode)statementOnSameLine);
                            if (lastTokenIn != null && lastTokenIn.GetTokenType() == NTriplesTokenType.END_OF_LINE_COMMENT)
                            {
                                text = Environment.NewLine + "}";
                                break;
                            }
                        }
                        else
                            break;
                    }
                }
                while (statementOnSameLine != null && parsedDocumentRange.Document.GetCoordsByOffset(treeOffset.Offset).Line == line);
            }
        label_40:
            DocumentRange documentRange = FileExtensions.GetDocumentRange(file, treeOffset);
            if (documentRange.IsValid() && documentRange.TextRange.StartOffset != offset1 + 1)
            {
                int offset2 = this.LexerToTextControl(textControl, documentRange.TextRange.StartOffset);
                if (offset2 >= 0)
                {
                    document.InsertText(offset2, text);
                    document.DeleteText(new TextRange(this.LexerToTextControl(textControl, offset1 + 1), this.LexerToTextControl(textControl, offset1 + 2)));
                }
            }
            return true;
        }

        private static ICSharpStatement FindNextStatementOnSameLine(IDocument document, IFile file, int pos)
        {
            Int32<DocLine> line = document.GetCoordsByOffset(pos).Line;
            ITokenNode tokenNode = file.FindTokenAt(new TreeOffset(pos)) as ITokenNode;
            while (tokenNode != null && (document.GetCoordsByOffset(tokenNode.GetTreeStartOffset().Offset).Line == line && (tokenNode is IWhitespaceNode || tokenNode is ICSharpCommentNode)))
                tokenNode = TokenNodeExtesions.GetNextToken(tokenNode);
            if (tokenNode == null || document.GetCoordsByOffset(tokenNode.GetTreeStartOffset().Offset).Line != line)
                return (ICSharpStatement)null;
            ICSharpStatement csharpStatement = (ICSharpStatement)null;
            for (ITreeNode treeNode = (ITreeNode)tokenNode; treeNode != null && treeNode.GetTreeStartOffset().Offset >= pos; treeNode = treeNode.Parent)
            {
                if (treeNode is ICSharpStatement)
                    csharpStatement = (ICSharpStatement)treeNode;
            }
            return csharpStatement;
        }

        protected bool HandleLeftBracketOrParenthTyped(ITypingContext typingContext)
        {
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart " + (object)typingContext.Char))
            {
                typingContext.CallNext();
                using (WriteLockCookie.Create())
                {
                    ITextControl textControl = typingContext.TextControl;
                    CachingLexer cachingLexer = this.GetCachingLexer(textControl);
                    int num1 = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret) - 1);
                    if (num1 < 0 || !cachingLexer.FindTokenAt(num1) || cachingLexer.TokenStart != num1 || cachingLexer.TokenType != NTriplesTokenType.LBRACKET && cachingLexer.TokenType != NTriplesTokenType.LPARENTH && !this.IsCustomLParenth(textControl, cachingLexer))
                        return true;
                    TokenNodeType nextTokenType = LexerUtil.LookaheadToken<int>((ILexer<int>)cachingLexer, 1);
                    if (nextTokenType != null && nextTokenType != NTriplesTokenType.WHITE_SPACE && (nextTokenType != NTriplesTokenType.NEW_LINE && nextTokenType != NTriplesTokenType.C_STYLE_COMMENT) && (nextTokenType != NTriplesTokenType.END_OF_LINE_COMMENT && nextTokenType != NTriplesTokenType.SEMICOLON && (nextTokenType != NTriplesTokenType.COMMA && nextTokenType != NTriplesTokenType.RBRACKET)) && (nextTokenType != NTriplesTokenType.RBRACE && nextTokenType != NTriplesTokenType.RPARENTH && !this.IsCustomTokenSuitableForCloseParenth(nextTokenType, textControl, cachingLexer)) || !this.NeedAutoinsertCloseBracket(textControl, cachingLexer) || typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                        return true;
                    char @char = typingContext.Char;
                    int num2 = this.LexerToTextControl(textControl, num1);
                    if (num2 >= 0)
                    {
                        textControl.Document.InsertText(num2 + 1, (int)@char == 40 ? ")" : ((int)@char == 91 ? "]" : "}"));
                        ITextControlCaretEx.MoveTo(textControl.Caret, num2 + 1, CaretVisualPlacement.DontScrollIfVisible);
                    }
                }
            }
            return true;
        }

        protected virtual bool IsCustomTokenSuitableForCloseParenth(TokenNodeType nextTokenType, ITextControl textControl, CachingLexer lexer)
        {
            return false;
        }

        protected virtual bool IsCustomLParenth(ITextControl textControl, CachingLexer lexer)
        {
            return false;
        }

        protected bool HandleRightBraceTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart RBRACE"))
            {
                TextControlUtil.DeleteSelection(textControl);
                if (this.GetTypingAssistOption<TypingAssistSettings, SmartBraceInsertType>(textControl, TypingAssistOptions.BraceInsertTypeExpression) != SmartBraceInsertType.DISABLED)
                {
                    int pos;
                    if (this.CheckSpecialRightBraceCase(textControl, out pos))
                    {
                        int offset = this.LexerToTextControl(textControl, pos);
                        if (offset >= 0)
                            ITextControlCaretEx.MoveTo(textControl.Caret, offset, CaretVisualPlacement.DontScrollIfVisible);
                    }
                    else if (!this.HandleRightBracketTyped(typingContext))
                        typingContext.CallNext();
                }
                else
                    typingContext.CallNext();
                if (this.GetTypingAssistOption<TypingAssistSettings, bool>(textControl, TypingAssistOptions.FormatBlockOnRBraceExpression))
                {
                    int num = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret) - 1);
                    if (num < 0)
                        return true;
                    IFile file = this.CommitPsi(textControl);
                    if (file == null)
                        return true;
                    ITokenNode tokenNode = FileExtensions.FindTokenAt(file, textControl.Document, num) as ITokenNode;
                    if (tokenNode == null)
                    {
                        this.FormatNonCSharpRBrace(textControl, file, num, CodeFormatProfile.SOFT);
                        return true;
                    }
                    else if (tokenNode.GetTokenType() != NTriplesTokenType.RBRACE)
                    {
                        this.FormatCustomRBrace(textControl, tokenNode, CodeFormatProfile.SOFT, (IContextBoundSettingsStore)null);
                        return true;
                    }
                    else
                    {
                        if (!this.FormatIfTypesMatch<IBlock>(tokenNode.Parent, (Func<IBlock, ITreeNode>)(n => (ITreeNode)n.LBrace), (Func<IBlock, ITreeNode>)(n => (ITreeNode)n.RBrace), (Func<IBlock, ITreeNode>)(n => (ITreeNode)StatementUtil.GetBlockParentNode(n) ?? (ITreeNode)n)) && !this.FormatIfTypesMatch<IMemberOwnerBody>(tokenNode.Parent, (Func<IMemberOwnerBody, ITreeNode>)(n => (ITreeNode)n.LBrace), (Func<IMemberOwnerBody, ITreeNode>)(n => (ITreeNode)n.RBrace), (Func<IMemberOwnerBody, ITreeNode>)(n => n.Parent)) && !this.FormatIfTypesMatch<IAccessorOwnerDeclaration>(tokenNode.Parent, (Func<IAccessorOwnerDeclaration, ITreeNode>)(n => (ITreeNode)n.LBrace), (Func<IAccessorOwnerDeclaration, ITreeNode>)(n => (ITreeNode)n.RBrace), (Func<IAccessorOwnerDeclaration, ITreeNode>)(n => (ITreeNode)n)))
                        {
                            if (!this.FormatIfTypesMatch<INamespaceBody>(tokenNode.Parent, (Func<INamespaceBody, ITreeNode>)(n => (ITreeNode)n.LBrace), (Func<INamespaceBody, ITreeNode>)(n => (ITreeNode)n.RBrace), (Func<INamespaceBody, ITreeNode>)(n => n.Parent)))
                                goto label_26;
                        }
                        DocumentRange documentRange = TreeNodeExtensions.GetDocumentRange((ITreeNode)tokenNode);
                        if (documentRange.IsValid())
                            ITextControlCaretEx.MoveTo(textControl.Caret, documentRange.TextRange.EndOffset, CaretVisualPlacement.DontScrollIfVisible);
                    }
                }
            }
        label_26:
            return true;
        }

        protected virtual void FormatNonCSharpRBrace(ITextControl textControl, IFile file, int offset, CodeFormatProfile profile)
        {
        }

        protected virtual void FormatCustomRBrace(ITextControl textControl, ITokenNode tokenNode, CodeFormatProfile profile, IContextBoundSettingsStore boundSettingsStore)
        {
        }

        protected virtual bool IsLBrace(ITextControl textControl, CachingLexer lexer)
        {
            return lexer.TokenType == NTriplesTokenType.LBRACE;
        }

        protected virtual bool IsRBrace(ITextControl textControl, CachingLexer lexer)
        {
            return lexer.TokenType == NTriplesTokenType.RBRACE;
        }

        protected virtual bool ShouldSkipToken(CachingLexer lexer)
        {
            return false;
        }

        private bool CheckSpecialRightBraceCase(ITextControl textControl, out int pos)
        {
            pos = -1;
            int pos1 = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (pos1 < 0 || !cachingLexer.FindTokenAt(pos1))
                return false;
            TokenNodeType tokenType1 = cachingLexer.TokenType;
            if (tokenType1 != NTriplesTokenType.WHITE_SPACE && tokenType1 != NTriplesTokenType.NEW_LINE && !this.ShouldSkipToken(cachingLexer))
                return false;
            for (; tokenType1 == NTriplesTokenType.WHITE_SPACE || tokenType1 == NTriplesTokenType.NEW_LINE || this.ShouldSkipToken(cachingLexer); tokenType1 = cachingLexer.TokenType)
                cachingLexer.Advance(-1);
            if (!this.IsLBrace(textControl, cachingLexer))
                return false;
            TokenNodeType tokenType2;
            do
            {
                cachingLexer.Advance();
                tokenType2 = cachingLexer.TokenType;
            }
            while (tokenType2 == NTriplesTokenType.WHITE_SPACE || tokenType2 == NTriplesTokenType.NEW_LINE || this.ShouldSkipToken(cachingLexer));
            if (!this.IsRBrace(textControl, cachingLexer))
                return false;
            pos = cachingLexer.TokenEnd;
            return this.NeedSkipCloseBracket(textControl, cachingLexer, '}');
        }

        protected virtual bool NeedSkipCloseBracket(ITextControl textControl, CachingLexer lexer, char charTyped)
        {
            TokenNodeType tokenType1 = lexer.TokenType;
            if ((int)charTyped == 41 && tokenType1 != NTriplesTokenType.RPARENTH || (int)charTyped == 93 && tokenType1 != NTriplesTokenType.RBRACKET || ((int)charTyped == 125 && tokenType1 != NTriplesTokenType.RBRACE || (int)charTyped == 62 && tokenType1 != NTriplesTokenType.GT))
                return false;
            TokenNodeType tokenNodeType = (int)charTyped == 62 ? NTriplesTokenType.LT : ((int)charTyped == 41 ? NTriplesTokenType.LPARENTH : ((int)charTyped == 93 ? NTriplesTokenType.LBRACKET : NTriplesTokenType.LBRACE));
            bool flag = tokenNodeType == NTriplesTokenType.LT;
            BracketMatcher bracketMatcher = flag ? (BracketMatcher)new CSharpAngleBracketMatcher() : (BracketMatcher)new CSharpBracketMatcher();
            int? nullable = new int?();
            lexer.Advance(-1);
            TokenNodeType tokenType2;
            while ((tokenType2 = lexer.TokenType) != null && (!flag || tokenType2.IsComment || (tokenType2.IsWhitespace || CSharpTypingAssistBase.TypeParamTokens[(NodeType)tokenType2]) || NTriplesTokenType.TYPE_KEYWORDS[(NodeType)tokenType2]))
            {
                if (bracketMatcher.Direction(tokenType2) == 1 && bracketMatcher.IsStackEmpty())
                {
                    if (tokenType2 == tokenNodeType)
                        nullable = new int?(lexer.CurrentPosition);
                }
                else if (!bracketMatcher.ProceedStack(tokenType2))
                    break;
                lexer.Advance(-1);
            }
            if (!nullable.HasValue)
                return false;
            lexer.CurrentPosition = nullable.Value;
            return bracketMatcher.FindMatchingBracket(lexer);
        }

        private bool HandleRightBracketTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart bracket"))
            {
                TextControlUtil.DeleteSelection(textControl);
                int pos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
                CachingLexer cachingLexer = this.GetCachingLexer(textControl);
                if (pos < 0 || !cachingLexer.FindTokenAt(pos) || cachingLexer.TokenStart != pos || !this.NeedSkipCloseBracket(textControl, cachingLexer, typingContext.Char))
                    return false;
                int offset = this.LexerToTextControl(textControl, pos + 1);
                if (offset >= 0)
                    ITextControlCaretEx.MoveTo(textControl.Caret, offset, CaretVisualPlacement.DontScrollIfVisible);
                return true;
            }
        }

        private bool HandleQuoteTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart quote"))
            {
                TextControlUtil.DeleteSelection(textControl);
                textControl.FillVirtualSpaceUntilCaret();
                CachingLexer cachingLexer1 = this.GetCachingLexer(textControl);
                IBuffer buffer = cachingLexer1.Buffer;
                int charPos1 = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
                TokenNodeType correspondingTokenType = (int)typingContext.Char == 39 ? NTriplesTokenType.CHARACTER_LITERAL : NTriplesTokenType.STRING_LITERAL;
                if (charPos1 < 0 || !cachingLexer1.FindTokenAt(charPos1))
                    return false;
                TokenNodeType tokenType = cachingLexer1.TokenType;
                if (correspondingTokenType == NTriplesTokenType.CHARACTER_LITERAL || (int)buffer[cachingLexer1.TokenStart] != 64)
                {
                    int index = charPos1 - 1;
                    while (index >= 0 && (int)buffer[index] == 92)
                        --index;
                    if ((charPos1 - index) % 2 == 0 && tokenType == correspondingTokenType)
                        return false;
                }
                if (charPos1 < buffer.Length && (int)buffer[charPos1] == (int)typingContext.Char && (tokenType == correspondingTokenType && cachingLexer1.TokenEnd == charPos1 + 1) && (cachingLexer1.TokenStart != charPos1 && (int)buffer[cachingLexer1.TokenStart] != 64 || cachingLexer1.TokenStart != charPos1 - 1 && (int)buffer[cachingLexer1.TokenStart] == 64))
                {
                    int num = this.LexerToTextControl(textControl, charPos1);
                    if (num >= 0)
                        ITextControlCaretEx.MoveTo(textControl.Caret, num + 1, CaretVisualPlacement.DontScrollIfVisible);
                    return true;
                }
                else
                {
                    if (tokenType != null && !this.IsStopperTokenForStringLiteral(tokenType))
                        return false;
                    while (cachingLexer1.TokenType == NTriplesTokenType.WHITE_SPACE)
                        cachingLexer1.Advance();
                    bool flag = cachingLexer1.TokenType == correspondingTokenType && (cachingLexer1.TokenEnd > cachingLexer1.TokenStart + 1 && (int)cachingLexer1.Buffer[cachingLexer1.TokenStart] == (int)typingContext.Char && (int)cachingLexer1.Buffer[cachingLexer1.TokenEnd - 1] == (int)typingContext.Char);
                    if (flag)
                    {
                        DocumentCoords coordsByOffset = textControl.Document.GetCoordsByOffset(charPos1);
                        int lineEndPos = textControl.Document.GetLineEndOffsetNoLineBreak(coordsByOffset.Line) - 1;
                        int charPos2 = charPos1;
                        if (this.CheckThatCSharpLineEndsInOpenStringLiteral(textControl, cachingLexer1, lineEndPos, typingContext.Char, correspondingTokenType, false, ref charPos2, true))
                            return false;
                    }
                    else
                    {
                        cachingLexer1.FindTokenAt(charPos1 - 1);
                        if (cachingLexer1.TokenType == correspondingTokenType && cachingLexer1.TokenEnd == charPos1 && ((int)cachingLexer1.Buffer[cachingLexer1.TokenStart] == 64 && cachingLexer1.TokenEnd - cachingLexer1.TokenStart >= 3) && ((int)cachingLexer1.Buffer[cachingLexer1.TokenStart + 1] == (int)typingContext.Char && (int)cachingLexer1.Buffer[cachingLexer1.TokenEnd - 1] == (int)typingContext.Char))
                            flag = true;
                    }
                    typingContext.CallNext();
                    CachingLexer cachingLexer2 = this.GetCachingLexer(textControl);
                    if (!flag)
                    {
                        if (!cachingLexer2.FindTokenAt(charPos1))
                            return true;
                        bool isStringWithAt = cachingLexer2.TokenType == NTriplesTokenType.STRING_LITERAL && cachingLexer2.TokenStart == charPos1 - 1 && (int)cachingLexer2.Buffer[cachingLexer2.TokenStart] == 64;
                        if (cachingLexer2.TokenStart != charPos1 && !isStringWithAt)
                            return true;
                        int offset = this.LexerToTextControl(textControl, charPos1);
                        if (offset < 0)
                            return true;
                        DocumentCoords coordsByOffset = textControl.Document.GetCoordsByOffset(offset);
                        int lineEndPos = textControl.Document.GetLineEndOffsetNoLineBreak(coordsByOffset.Line) - 1;
                        flag = this.CheckThatCSharpLineEndsInOpenStringLiteral(textControl, cachingLexer2, lineEndPos, typingContext.Char, correspondingTokenType, isStringWithAt, ref charPos1, false);
                    }
                    if (flag)
                    {
                        int offset1 = charPos1 + 1;
                        int offset2 = this.LexerToTextControl(textControl, offset1);
                        if (offset2 >= 0)
                        {
                            textControl.Document.InsertText(offset2, (int)typingContext.Char == 39 ? "'" : "\"");
                            ITextControlCaretEx.MoveTo(textControl.Caret, offset2, CaretVisualPlacement.DontScrollIfVisible);
                        }
                    }
                }
            }
            return true;
        }

        protected virtual bool CheckThatCSharpLineEndsInOpenStringLiteral(ITextControl textControl, CachingLexer lexer, int lineEndPos, char c, TokenNodeType correspondingTokenType, bool isStringWithAt, ref int charPos, bool defaultReturnValue)
        {
            if (lineEndPos >= 0)
            {
                int pos1 = this.TextControlToLexer(textControl, lineEndPos);
                if (pos1 >= 0)
                {
                    lexer.FindTokenAt(pos1);
                }
                else
                {
                    while (lineEndPos > charPos)
                    {
                        --lineEndPos;
                        int pos2 = this.TextControlToLexer(textControl, lineEndPos);
                        if (pos2 >= 0 && lexer.FindTokenAt(pos2) && lexer.TokenType is ICSharpTokenNodeType)
                            break;
                    }
                }
            }
            if (lineEndPos < 0 || lexer.TokenType == null)
            {
                charPos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret) - 1);
                if (charPos < 0)
                    return false;
                lexer.FindTokenAt(charPos);
            }
            if (lexer.TokenType == null)
                return false;
            if (lexer.TokenType.IsComment)
                return StringUtil.Count(LexerUtil.GetCurrTokenText((ILexer)lexer), c) % 2 == 1;
            if (lexer.TokenType != correspondingTokenType)
                return false;
            if (lexer.TokenEnd == lexer.TokenStart + 1 || (int)lexer.Buffer[lexer.TokenEnd - 1] != (int)c)
                return true;
            if (isStringWithAt && lexer.TokenStart == charPos - 1)
                return lexer.TokenEnd != charPos + 1;
            else
                return false;
        }

        protected virtual bool IsStopperTokenForStringLiteral([NotNull] TokenNodeType tokenType)
        {
            if (tokenType != NTriplesTokenType.WHITE_SPACE && tokenType != NTriplesTokenType.NEW_LINE && (tokenType != NTriplesTokenType.C_STYLE_COMMENT && tokenType != NTriplesTokenType.END_OF_LINE_COMMENT) && (tokenType != NTriplesTokenType.SEMICOLON && tokenType != NTriplesTokenType.COMMA && (tokenType != NTriplesTokenType.RBRACKET && tokenType != NTriplesTokenType.RBRACE)) && (tokenType != NTriplesTokenType.RPARENTH && tokenType != NTriplesTokenType.STRING_LITERAL))
                return tokenType == NTriplesTokenType.CHARACTER_LITERAL;
            else
                return true;
        }

        protected virtual bool HandleSemicolonTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart ;"))
            {
                TextControlUtil.DeleteSelection(textControl);
                textControl.FillVirtualSpaceUntilCaret();
                int pos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
                CachingLexer cachingLexer = this.GetCachingLexer(textControl);
                if (pos < 0 || !cachingLexer.FindTokenAt(pos) || (cachingLexer.TokenStart != pos || cachingLexer.TokenType != NTriplesTokenType.SEMICOLON) || !this.GetTypingAssistOption<TypingAssistSettings, bool>(textControl, TypingAssistOptions.SmartParenthEnabledExpression))
                {
                    typingContext.CallNext();
                }
                else
                {
                    int offset = this.LexerToTextControl(textControl, pos + 1);
                    if (offset < 0)
                        return true;
                    ITextControlCaretEx.MoveTo(textControl.Caret, offset, CaretVisualPlacement.DontScrollIfVisible);
                }
                if (this.GetTypingAssistOption<TypingAssistSettings, bool>(textControl, TypingAssistOptions.FormatStatementOnSemicolonExpression))
                    this.DoFormatStatementOnSemicolon(textControl, (Predicate<ITreeNode>)(node =>
                    {
                        if (!(node is ICSharpStatement) && !(node is ICSharpDeclaration))
                            return node is IMultipleDeclaration;
                        else
                            return true;
                    }), (Func<ITreeNode, ITreeNode>)(node =>
                    {
                        if (!(node is ICSharpStatement))
                            return (ITreeNode)null;
                        else
                            return (ITreeNode)StatementUtil.GetParentStatement((ICSharpStatement)node, true);
                    }));
                return true;
            }
        }

        private bool HandleDivideTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            int num1 = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (cachingLexer.FindTokenAt(num1) && cachingLexer.TokenType == NTriplesTokenType.C_STYLE_COMMENT)
            {
                string str = LexerUtil.GetCurrTokenText((ILexer)cachingLexer).Substring(0, num1 - cachingLexer.TokenStart);
                int startIndex = str.LastIndexOf('\n');
                if (startIndex >= 0)
                {
                    int num2 = str.IndexOf('*', startIndex);
                    if (num2 >= 0)
                    {
                        bool flag = true;
                        for (int index = startIndex + 1; index < num2; ++index)
                        {
                            if (!char.IsWhiteSpace(str[index]))
                            {
                                flag = false;
                                break;
                            }
                        }
                        for (int index = num2 + 1; index < str.Length; ++index)
                        {
                            if (!flag || !char.IsWhiteSpace(str[index]))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag && typingContext.EnsureWritable() == EnsureWritableResult.SUCCESS)
                        {
                            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart /"))
                            {
                                textControl.Document.ReplaceText(new TextRange(cachingLexer.TokenStart + num2 + 1, num1), "/");
                                return true;
                            }
                        }
                    }
                }
            }
            if (Shell.Instance.ProgramConfiguration != ProgramConfigurations.STANDALONE || num1 < 2 || (!cachingLexer.FindTokenAt(num1 - 1) || cachingLexer.TokenType != NTriplesTokenType.END_OF_LINE_COMMENT) || (num1 != cachingLexer.TokenStart + 2 || cachingLexer.TokenEnd - cachingLexer.TokenStart != 2 || typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS))
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart /"))
            {
                typingContext.CallNext();
                IDocCommentNode docCommentNode = this.CommitPsi(textControl).FindTokenAt(new TreeOffset(num1)) as IDocCommentNode;
                if (docCommentNode == null)
                    return true;
                IDocCommentBlockNode commentBlockNode = docCommentNode.Parent as IDocCommentBlockNode;
                if (commentBlockNode == null || commentBlockNode.GetTextLength() != 3)
                    return true;
                int offset1 = commentBlockNode.GetTreeStartOffset().Offset;
                Int32<DocLine> line = textControl.Document.GetCoordsByOffset(offset1).Line;
                int cursor;
                string docTemplate = XmlDocTemplateUtil.GetDocTemplate((IDocCommentBlockOwnerNode)commentBlockNode.Parent, out cursor);
                string text = textControl.Document.GetText(new TextRange(textControl.Document.GetLineStartOffset(line), offset1));
                string[] strArray = docTemplate.TrimEnd(new char[0]).Split(new char[1]
        {
          '\n'
        });
                StringBuilder stringBuilder = new StringBuilder();
                int offset2 = offset1 + 3;
                for (int index = 0; index < strArray.Length; ++index)
                {
                    string str = strArray[index];
                    stringBuilder.Append(index == 0 ? " " : text + "/// ");
                    if (cursor >= 0 && cursor <= str.Length)
                        offset2 += stringBuilder.Length + cursor;
                    cursor -= str.Length + 1;
                    stringBuilder.Append(str);
                    if (index < strArray.Length - 1)
                        stringBuilder.Append(Environment.NewLine);
                }
                int offset3 = this.LexerToTextControl(textControl, offset1 + 3);
                if (offset3 < 0)
                    return true;
                textControl.Document.InsertText(offset3, ((object)stringBuilder).ToString());
                int offset4 = this.LexerToTextControl(textControl, offset2);
                if (offset4 < 0)
                    return true;
                ITextControlCaretEx.MoveTo(textControl.Caret, offset4, CaretVisualPlacement.DontScrollIfVisible);
            }
            return true;
        }

        protected bool HandleEnterPressed(IActionContext context)
        {
            ITextControl textControl = context.TextControl;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart Enter"))
            {
                if (this.GetTypingAssistOption<TypingAssistSettings, SmartBraceInsertType>(textControl, TypingAssistOptions.BraceInsertTypeExpression) != SmartBraceInsertType.DISABLED && this.DoHandleEnterAfterLBracePressed(textControl) || this.DoHandleEnterInDocCommentPressed(textControl) || this.DoHandleEnterInStringPressed(textControl))
                    return true;
                context.CallNext();
                if (this.GetTypingAssistOption<TypingAssistSettings, bool>(textControl, TypingAssistOptions.SmartIndentOnEnterExpression))
                    this.DoSmartIndentOnEnter(textControl);
                return true;
            }
        }

        private bool DoHandleEnterInStringPressed(ITextControl textControl)
        {
            int num = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            if (num <= 0)
                return false;
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (!cachingLexer.FindTokenAt(num - 1) || cachingLexer.TokenType != NTriplesTokenType.STRING_LITERAL || num == cachingLexer.TokenEnd)
                return false;
            string text1 = textControl.Document.GetText(new TextRange(cachingLexer.TokenStart, cachingLexer.TokenEnd));
            if ((int)text1[0] == 64)
                return false;
            int offset1 = cachingLexer.TokenStart + CSharpTypingAssistBase.GetValidSplitOffset((PsiLanguageType)CSharpLanguage.Instance, text1, num - cachingLexer.TokenStart);
            string text2 = "\"+" + Environment.NewLine + "\"";
            int offset2 = this.LexerToTextControl(textControl, offset1);
            if (offset2 < 0)
                return false;
            textControl.Document.InsertText(offset2, text2);
            ITextControlCaretEx.MoveTo(textControl.Caret, offset2 + text2.Length, CaretVisualPlacement.DontScrollIfVisible);
            IFile file = this.CommitPsi(textControl);
            if (file == null)
                return false;
            ITokenNode tokenNode1 = FileExtensions.FindTokenAt(file, textControl.Document, offset2 - 1) as ITokenNode;
            if (tokenNode1 == null || tokenNode1.GetTokenType() != NTriplesTokenType.STRING_LITERAL)
                return false;
            ITokenNode tokenNode2 = file.FindTokenAt(FileExtensions.Translate(file, new DocumentRange(textControl.Document, offset2 + text2.Length)).StartOffset) as ITokenNode;
            if (tokenNode2 == null || tokenNode2.GetTokenType() != NTriplesTokenType.STRING_LITERAL)
                return false;
            using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Format code"))
                CodeFormatterHelper.Format((ICodeFormatter)this.GetCodeFormatter((ITreeNode)tokenNode1), (ITreeNode)tokenNode1, (ITreeNode)tokenNode2, CodeFormatProfile.SOFT);
            return true;
        }

        /// <param name="languageType"/><param name="str">String literal, including opening and ending double-quotes</param><param name="offset">Current offset in literal</param>
        private static int GetValidSplitOffset(PsiLanguageType languageType, string str, int offset)
        {
            RangeTranslator translator;
            LiteralService.Get(languageType).ParseStringLiteral(str, out translator);
            TextRange resultRange = translator.GetResultRange(offset - 2);
            TextRange sourceRange = translator.GetSourceRange(resultRange);
            if (sourceRange.Length > 1)
                offset = sourceRange.EndOffset + 1;
            return offset;
        }

        private bool DoHandleEnterAfterLBracePressed(ITextControl textControl)
        {
            int charPos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            if (charPos <= 0)
                return false;
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (!cachingLexer.FindTokenAt(charPos - 1))
                return false;
            if (cachingLexer.TokenType == NTriplesTokenType.WHITE_SPACE)
            {
                charPos = cachingLexer.TokenStart;
                cachingLexer.Advance(-1);
            }
            if (!this.IsLBrace(textControl, cachingLexer))
                return false;
            int tokenStart = cachingLexer.TokenStart;
            bool flag = false;
            if (this.GetTypingAssistOption<TypingAssistSettings, SmartBraceInsertType>(textControl, TypingAssistOptions.BraceInsertTypeExpression) == SmartBraceInsertType.ON_ENTER)
            {
                flag = this.AutoinsertRBrace(textControl, cachingLexer);
                cachingLexer = this.GetCachingLexer(textControl);
                cachingLexer.FindTokenAt(tokenStart);
            }
            int rBracePos;
            if (!this.FindMatchingBrace(textControl, cachingLexer, out rBracePos))
                return false;
            int offset1 = this.LexerToTextControl(textControl, tokenStart);
            int offset2 = this.LexerToTextControl(textControl, rBracePos);
            if (offset1 < 0 || offset2 < 0 || !flag && textControl.Document.GetCoordsByOffset(offset1).Line != textControl.Document.GetCoordsByOffset(offset2).Line)
                return false;
            IFile file = this.CommitPsi(textControl);
            if (file == null)
                return false;
            ITokenNode rBraceNode;
            TreeOffset lBraceTreePos;
            TreeOffset rBraceTreePos;
            ITokenNode lBraceNode;
            if (!this.FindNodesForBraces(textControl, file, tokenStart, rBracePos, out lBraceNode, out rBraceNode, out lBraceTreePos, out rBraceTreePos))
                return this.TryReparseAndFormatOnEnterAfterNonCSharpLBrace(textControl, file, tokenStart, rBracePos);
            TreeOffset newCaretPos = new TreeOffset(-1);
            if (!this.PsiServices.PsiManager.DoTransaction((Action)(() => newCaretPos = this.ReparseAndFormatOnEnter(textControl, lBraceTreePos, rBraceTreePos, charPos, lBraceNode, rBraceNode, ref file)), "Typing assist").Succeded)
                return false;
            DocumentRange documentRange = FileExtensions.GetDocumentRange(file, newCaretPos);
            if (documentRange.IsValid())
                ITextControlCaretEx.MoveTo(textControl.Caret, documentRange.TextRange.StartOffset, CaretVisualPlacement.DontScrollIfVisible);
            return true;
        }

        protected virtual bool TryReparseAndFormatOnEnterAfterNonCSharpLBrace(ITextControl textControl, IFile file, int lBracePos, int rBracePos)
        {
            return false;
        }

        protected virtual TreeOffset ReparseAndFormatOnEnter(ITextControl textControl, TreeOffset lBraceTreePos, TreeOffset rBraceTreePos, int charPos, ITokenNode lBraceNode, ITokenNode rBraceNode, ref IFile file)
        {
            string str = "partial class";
            if (lBraceNode.Parent is IPropertyDeclaration || lBraceNode.Parent is IEventDeclaration || lBraceNode.Parent is IIndexerDeclaration)
                str = "get{}";
            if (SwitchStatementNavigator.GetByBlock(lBraceNode.Parent as IBlock) != null)
                str = "case 0:";
            ITokenNode nextToken = TokenNodeExtesions.GetNextToken(lBraceNode);
            while (nextToken != null && PsiLanguageTypeExtensions.LanguageService(lBraceNode.Language).IsFilteredNode((ITreeNode)nextToken) && nextToken != rBraceNode)
                nextToken = TokenNodeExtesions.GetNextToken(nextToken);
            if (nextToken != rBraceNode)
                str = "";
            string newLine = Environment.NewLine;
            TreeTextRange modifiedRange = FileExtensions.Translate(file, new DocumentRange(textControl.Document, charPos));
            file = file.ReParse(modifiedRange, newLine + str);
            lBraceNode = file.FindTokenAt(lBraceTreePos) as ITokenNode;
            ITokenNode tokenNode = file.FindTokenAt(modifiedRange.StartOffset + newLine.Length) as ITokenNode;
            while (tokenNode != null && PsiLanguageTypeExtensions.LanguageService(tokenNode.Language).IsFilteredNode((ITreeNode)tokenNode) && nextToken != rBraceNode)
                tokenNode = TokenNodeExtesions.GetNextToken(tokenNode);
            rBraceNode = file.FindTokenAt(rBraceTreePos + newLine.Length + str.Length) as ITokenNode;
            ICSharpCodeFormatter codeFormatter = this.GetCodeFormatter((ITreeNode)lBraceNode);
            if (lBraceNode.GetTokenType() == NTriplesTokenType.LBRACE && this.GetTypingAssistOption<TypingAssistSettings, bool>(textControl, TypingAssistOptions.FormatBlockOnRBraceExpression))
            {
                ICSharpTreeNode blockParentNode = StatementUtil.GetBlockParentNode(lBraceNode.Parent as IBlock);
                if (blockParentNode != null)
                    CodeFormatterHelper.Format((ICodeFormatter)codeFormatter, (ITreeNode)TreeNodeExtensions.GetFirstTokenIn((ITreeNode)blockParentNode), (ITreeNode)lBraceNode, CodeFormatProfile.SOFT);
            }
            Lifetimes.Using((Action<Lifetime>)(lifetime =>
            {
                ISettingsStore local_0 = this.SettingsStore;
                IContextBoundSettingsStore local_1 = local_0.CreateNestedTransaction(lifetime, "Aaaaaa! Help me name my transaction!!").BindToContext(JetBrains.TextControl.DataConstants.DataContextsEx.ToDataContext(textControl)(lifetime, local_0.DataContexts));
                this.OverrideSettingsForFormattingOnEnter(local_1);
                codeFormatter.Format((ITreeNode)lBraceNode, codeFormatter.GetProfile(CodeFormatProfile.GENERATOR), (IProgressIndicator)null, local_1);
                codeFormatter.Format(FormatterImplHelper.FindFormattingRangeToLeft((ITreeNode)rBraceNode, (Func<ITokenNode, bool>)null), (ITreeNode)rBraceNode, codeFormatter.GetProfile(CodeFormatProfile.GENERATOR), (IProgressIndicator)null, local_1);
                if (rBraceNode.GetTokenType() != NTriplesTokenType.RBRACE)
                    this.FormatCustomRBrace(textControl, rBraceNode, CodeFormatProfile.INDENT, local_1);
                else
                    codeFormatter.Format(lBraceNode.Parent, codeFormatter.GetProfile(CodeFormatProfile.INDENT), (IProgressIndicator)null, local_1);
            }));
            TreeOffset treeStartOffset = tokenNode.GetTreeStartOffset();
            file = file.ReParse(new TreeTextRange(treeStartOffset, treeStartOffset + str.Length), "");
            return treeStartOffset;
        }

        protected void OverrideSettingsForFormattingOnEnter(IContextBoundSettingsStore settings)
        {
            IContextBoundSettingsStore thіs1 = settings;
            ParameterExpression parameterExpression1 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression1 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression1, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.STICK_COMMENT))), new ParameterExpression[1]
      {
        parameterExpression1
      });
            int num1 = 0;
            // ISSUE: variable of the null type
            __Null local1 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs1, lambdaexpression1, num1 != 0, (IDictionary<SettingsKey, object>)local1);
            IContextBoundSettingsStore thіs2 = settings;
            ParameterExpression parameterExpression2 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression2 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression2, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.KEEP_USER_LINEBREAKS))), new ParameterExpression[1]
      {
        parameterExpression2
      });
            int num2 = 1;
            // ISSUE: variable of the null type
            __Null local2 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs2, lambdaexpression2, num2 != 0, (IDictionary<SettingsKey, object>)local2);
            IContextBoundSettingsStore thіs3 = settings;
            ParameterExpression parameterExpression3 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression3 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression3, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_CATCH_ON_NEW_LINE))), new ParameterExpression[1]
      {
        parameterExpression3
      });
            int num3 = 1;
            // ISSUE: variable of the null type
            __Null local3 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs3, lambdaexpression3, num3 != 0, (IDictionary<SettingsKey, object>)local3);
            IContextBoundSettingsStore thіs4 = settings;
            ParameterExpression parameterExpression4 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression4 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression4, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_ELSE_ON_NEW_LINE))), new ParameterExpression[1]
      {
        parameterExpression4
      });
            int num4 = 1;
            // ISSUE: variable of the null type
            __Null local4 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs4, lambdaexpression4, num4 != 0, (IDictionary<SettingsKey, object>)local4);
            IContextBoundSettingsStore thіs5 = settings;
            ParameterExpression parameterExpression5 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression5 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression5, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_FINALLY_ON_NEW_LINE))), new ParameterExpression[1]
      {
        parameterExpression5
      });
            int num5 = 1;
            // ISSUE: variable of the null type
            __Null local5 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs5, lambdaexpression5, num5 != 0, (IDictionary<SettingsKey, object>)local5);
            IContextBoundSettingsStore thіs6 = settings;
            ParameterExpression parameterExpression6 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression6 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression6, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_WHILE_ON_NEW_LINE))), new ParameterExpression[1]
      {
        parameterExpression6
      });
            int num6 = 1;
            // ISSUE: variable of the null type
            __Null local6 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs6, lambdaexpression6, num6 != 0, (IDictionary<SettingsKey, object>)local6);
            IContextBoundSettingsStore thіs7 = settings;
            ParameterExpression parameterExpression7 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression7 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression7, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_ABSTRACT_ACCESSORHOLDER_ON_SINGLE_LINE))), new ParameterExpression[1]
      {
        parameterExpression7
      });
            int num7 = 0;
            // ISSUE: variable of the null type
            __Null local7 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs7, lambdaexpression7, num7 != 0, (IDictionary<SettingsKey, object>)local7);
            IContextBoundSettingsStore thіs8 = settings;
            ParameterExpression parameterExpression8 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression8 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression8, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_ACCESSORHOLDER_ATTRIBUTE_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression8
      });
            int num8 = 0;
            // ISSUE: variable of the null type
            __Null local8 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs8, lambdaexpression8, num8 != 0, (IDictionary<SettingsKey, object>)local8);
            IContextBoundSettingsStore thіs9 = settings;
            ParameterExpression parameterExpression9 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression9 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression9, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_SIMPLE_ACCESSOR_ATTRIBUTE_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression9
      });
            int num9 = 0;
            // ISSUE: variable of the null type
            __Null local9 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs9, lambdaexpression9, num9 != 0, (IDictionary<SettingsKey, object>)local9);
            IContextBoundSettingsStore thіs10 = settings;
            ParameterExpression parameterExpression10 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression10 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression10, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_COMPLEX_ACCESSOR_ATTRIBUTE_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression10
      });
            int num10 = 0;
            // ISSUE: variable of the null type
            __Null local10 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs10, lambdaexpression10, num10 != 0, (IDictionary<SettingsKey, object>)local10);
            IContextBoundSettingsStore thіs11 = settings;
            ParameterExpression parameterExpression11 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression11 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression11, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_CONSTRUCTOR_INITIALIZER_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression11
      });
            int num11 = 0;
            // ISSUE: variable of the null type
            __Null local11 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs11, lambdaexpression11, num11 != 0, (IDictionary<SettingsKey, object>)local11);
            IContextBoundSettingsStore thіs12 = settings;
            ParameterExpression parameterExpression12 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression12 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression12, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_FIELD_ATTRIBUTE_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression12
      });
            int num12 = 0;
            // ISSUE: variable of the null type
            __Null local12 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs12, lambdaexpression12, num12 != 0, (IDictionary<SettingsKey, object>)local12);
            IContextBoundSettingsStore thіs13 = settings;
            ParameterExpression parameterExpression13 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression13 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression13, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_METHOD_ATTRIBUTE_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression13
      });
            int num13 = 0;
            // ISSUE: variable of the null type
            __Null local13 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs13, lambdaexpression13, num13 != 0, (IDictionary<SettingsKey, object>)local13);
            IContextBoundSettingsStore thіs14 = settings;
            ParameterExpression parameterExpression14 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression14 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression14, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_SIMPLE_ACCESSOR_ON_SINGLE_LINE))), new ParameterExpression[1]
      {
        parameterExpression14
      });
            int num14 = 0;
            // ISSUE: variable of the null type
            __Null local14 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs14, lambdaexpression14, num14 != 0, (IDictionary<SettingsKey, object>)local14);
            IContextBoundSettingsStore thіs15 = settings;
            ParameterExpression parameterExpression15 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression15 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression15, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_SIMPLE_METHOD_ON_SINGLE_LINE))), new ParameterExpression[1]
      {
        parameterExpression15
      });
            int num15 = 0;
            // ISSUE: variable of the null type
            __Null local15 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs15, lambdaexpression15, num15 != 0, (IDictionary<SettingsKey, object>)local15);
            IContextBoundSettingsStore thіs16 = settings;
            ParameterExpression parameterExpression16 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression16 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression16, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_SIMPLE_ACCESSORHOLDER_ON_SINGLE_LINE))), new ParameterExpression[1]
      {
        parameterExpression16
      });
            int num16 = 0;
            // ISSUE: variable of the null type
            __Null local16 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs16, lambdaexpression16, num16 != 0, (IDictionary<SettingsKey, object>)local16);
            IContextBoundSettingsStore thіs17 = settings;
            ParameterExpression parameterExpression17 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression17 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression17, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_SIMPLE_ANONYMOUSMETHOD_ON_SINGLE_LINE))), new ParameterExpression[1]
      {
        parameterExpression17
      });
            int num17 = 0;
            // ISSUE: variable of the null type
            __Null local17 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs17, lambdaexpression17, num17 != 0, (IDictionary<SettingsKey, object>)local17);
            IContextBoundSettingsStore thіs18 = settings;
            ParameterExpression parameterExpression18 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
            // ISSUE: field reference
            Expression<Func<CSharpFormatSettingsKey, bool>> lambdaexpression18 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)parameterExpression18, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.PLACE_TYPE_ATTRIBUTE_ON_SAME_LINE))), new ParameterExpression[1]
      {
        parameterExpression18
      });
            int num18 = 0;
            // ISSUE: variable of the null type
            __Null local18 = null;
            SettingsStoreEx.SetValue<CSharpFormatSettingsKey, bool>(thіs18, lambdaexpression18, num18 != 0, (IDictionary<SettingsKey, object>)local18);
        }

        protected virtual bool FindNodesForBraces(ITextControl textControl, IFile file, int lBracePos, int rBracePos, out ITokenNode lBraceNode, out ITokenNode rBraceNode, out TreeOffset lBraceTreePos, out TreeOffset rBraceTreePos)
        {
            lBraceTreePos = FileExtensions.Translate(file, new DocumentRange(textControl.Document, lBracePos)).StartOffset;
            rBraceTreePos = FileExtensions.Translate(file, new DocumentRange(textControl.Document, rBracePos)).StartOffset;
            lBraceNode = file.FindTokenAt(lBraceTreePos) as ITokenNode;
            if (lBraceNode == null || !base.IsLBrace(textControl, (ITreeNode)lBraceNode))
            {
                rBraceNode = (ITokenNode)null;
                return false;
            }
            else
            {
                rBraceNode = file.FindTokenAt(rBraceTreePos) as ITokenNode;
                return rBraceNode != null && base.IsRBrace(textControl, (ITreeNode)rBraceNode);
            }
        }

        protected override bool IsLBrace(ITextControl textControl, ITreeNode node)
        {
            return TreeNodeExtensions.GetTokenType(node) == NTriplesTokenType.L_BRACE;
        }

        protected override bool IsRBrace(ITextControl textControl, ITreeNode node)
        {
            return TreeNodeExtensions.GetTokenType(node) == NTriplesTokenType.R_BRACE;
        }

        protected override bool IsSemicolon(ITextControl textControl, ITreeNode node)
        {
            return TreeNodeExtensions.GetTokenType(node) == NTriplesTokenType.SEMICOLON;
        }

        protected virtual bool FindMatchingBrace(ITextControl textControl, CachingLexer lexer, out int rBracePos)
        {
            return new NTriplesBracketMatcher().FindMatchingBracket(lexer, out rBracePos);
        }

        protected virtual string GetIndent(ITextControl textControl, int lexerOffset)
        {
            int num = this.LexerToTextControl(textControl, lexerOffset);
            if (num < 0)
                return string.Empty;
            Int32<DocLine> line = textControl.Document.GetCoordsByOffset(num).Line;
            return textControl.Document.GetText(new TextRange(textControl.Document.GetLineStartOffset(line), num));
        }

        private bool DoHandleEnterInDocCommentPressed(ITextControl textControl)
        {
            int offset1 = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            if (offset1 <= 0)
                return false;
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (!cachingLexer.FindTokenAt(offset1 - 1) || cachingLexer.TokenType != NTriplesTokenType.END_OF_LINE_COMMENT || (offset1 - cachingLexer.TokenStart < 3 || !LexerUtil.GetCurrTokenText((ILexer)cachingLexer).StartsWith("///")))
                return false;
            string indent = this.GetIndent(textControl, cachingLexer.TokenStart);
            ILexer lexer = PsiLanguageTypeExtensions.LanguageService((PsiLanguageType)NTriplesLanguage.Instance).GetPrimaryLexerFactory().CreateLexer((IBuffer)new StringBuffer(indent));
            lexer.Start();
            while (lexer.TokenType != null)
            {
                if (lexer.TokenType != NTriplesTokenType.WHITE_SPACE)
                    return false;
                lexer.Advance();
            }
            string text = Environment.NewLine + indent + "/// ";
            int offset2 = this.LexerToTextControl(textControl, offset1);
            if (offset2 < 0)
                return false;
            textControl.Document.InsertText(offset2, text);
            ITextControlCaretEx.MoveTo(textControl.Caret, offset2 + text.Length, CaretVisualPlacement.DontScrollIfVisible);
            return true;
        }

        protected virtual void DoSmartIndentOnEnter(ITextControl textControl)
        {
            int num1 = ITextControlCaretEx.Offset(textControl.Caret);
            int offset = this.TextControlToLexer(textControl, num1);
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (offset < 0 || !cachingLexer.FindTokenAt(offset) || cachingLexer.TokenType == NTriplesTokenType.STRING_LITERAL)
                return;
            while (cachingLexer.TokenType == NTriplesTokenType.WHITE_SPACE)
                cachingLexer.Advance();
            offset = cachingLexer.TokenType == null ? offset : cachingLexer.TokenStart;
            string text = cachingLexer.TokenType == NTriplesTokenType.NEW_LINE || cachingLexer.TokenType == null ? "partial class " : string.Empty;
            IPsiSourceFile psiSourceFile = PsiSourceFileExtensions.GetPsiSourceFile(textControl.Document, this.Solution);
            if (psiSourceFile == null || !psiSourceFile.IsValid())
                return;
            int num2 = this.LexerToTextControl(textControl, offset);
            if (text.Length > 0)
                textControl.Document.InsertText(num2, text);
            this.PsiServices.PsiManager.CommitAllDocuments();
            DocumentRange range = new DocumentRange(textControl.Document, offset);
            IFile file = PsiManagerExtensions.GetPsiFile<NTriplesLanguage>(psiSourceFile, range);
            ITokenNode tokenNode = file == null ? (ITokenNode)null : FileExtensions.FindTokenAt(file, textControl.Document, offset) as ITokenNode;
            if (tokenNode == null)
            {
                if (text.Length <= 0)
                    return;
                textControl.Document.DeleteText(new TextRange(num2, num2 + text.Length));
            }
            else
            {
                int offsetInToken = offset - tokenNode.GetTreeStartOffset().Offset;
                ICSharpCodeFormatter codeFormatter = this.GetCodeFormatter((ITreeNode)tokenNode);
                using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Typing assist"))
                    Lifetimes.Using((Action<Lifetime>)(lifetime =>
                    {
                        ISettingsStore local_0 = this.SettingsStore;
                        IContextBoundSettingsStore local_1 = local_0.CreateNestedTransaction(lifetime, "Aaaaaa! Help me name my transaction!!").BindToContext(JetBrains.TextControl.DataConstants.DataContextsEx.ToDataContext(textControl)(lifetime, local_0.DataContexts));
                        IContextBoundSettingsStore temp_190 = local_1;
                        ISettingsSchema temp_192 = local_0.Schema;
                        ParameterExpression local_2 = Expression.Parameter(typeof(CSharpFormatSettingsKey), "key");
                        // ISSUE: field reference
                        Expression<Func<CSharpFormatSettingsKey, bool>> temp_207 = Expression.Lambda<Func<CSharpFormatSettingsKey, bool>>((Expression)Expression.Field((Expression)local_2, FieldInfo.GetFieldFromHandle(__fieldref(CSharpFormatSettingsKey.STICK_COMMENT))), new ParameterExpression[1]
            {
              local_2
            });
                        SettingsScalarEntry temp_208 = temp_192.GetScalarEntry<CSharpFormatSettingsKey, bool>(temp_207);
                        // ISSUE: variable of a boxed type
                        __Boxed<bool> temp_210 = (ValueType)false;
                        // ISSUE: variable of the null type
                        __Null temp_211 = null;
                        temp_190.SetValue(temp_208, (object)temp_210, (IDictionary<SettingsKey, object>)temp_211);
                        codeFormatter.Format((ITreeNode)TokenNodeExtesions.GetPrevToken(tokenNode), (ITreeNode)tokenNode, codeFormatter.GetProfile(CodeFormatProfile.INDENT), (IProgressIndicator)NullProgressIndicator.Instance, local_1);
                        offset = FileExtensions.GetDocumentRange(file, new TreeTextRange(tokenNode.GetTreeStartOffset())).TextRange.StartOffset + offsetInToken;
                    }));
                if (text.Length > 0)
                {
                    int startOffset = this.LexerToTextControl(textControl, offset);
                    textControl.Document.DeleteText(new TextRange(startOffset, startOffset + text.Length));
                }
                offset = this.AdjustLineIndent(textControl, num1, this.LexerToTextControl(textControl, offset));
                ITextControlCaretEx.MoveTo(textControl.Caret, offset, CaretVisualPlacement.DontScrollIfVisible);
            }
        }

        protected virtual int AdjustLineIndent(ITextControl textControl, int originalOffset, int currentOffset)
        {
            return currentOffset;
        }

        protected bool HandleBackspacePressed(IActionContext context)
        {
            ITextControl textControl = context.TextControl;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart backspace"))
                return this.GetTypingAssistOption<TypingAssistSettings, bool>(textControl, TypingAssistOptions.SmartParenthEnabledExpression) && this.DoHandleBackspacePressed(textControl) || this.DoHandleBackspaceOnStringsConcat(textControl);
        }

        private bool DoHandleBackspaceOnStringsConcat(ITextControl textControl)
        {
            if (ITextControlSelectionEx.RandomRange(textControl.Selection).Length > 0)
                return false;
            int charPos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret) - 1);
            if (charPos > 0)
                return this.DoHandleDeletingPlus(textControl, charPos);
            else
                return false;
        }

        private bool DoHandleDeletingPlus(ITextControl textControl, int charPos)
        {
            CachingLexer cachingLexer = this.GetCachingLexer(textControl);
            if (!cachingLexer.FindTokenAt(charPos) || cachingLexer.TokenType != NTriplesTokenType.PLUS)
                return false;
            int currentPosition = cachingLexer.CurrentPosition;
            cachingLexer.Advance(-1);
            if (!this.SkipToRegularStringLiteral(cachingLexer, textControl, -1))
                return false;
            int offset1 = cachingLexer.TokenEnd - 1;
            cachingLexer.CurrentPosition = currentPosition;
            cachingLexer.Advance();
            if (!this.SkipToRegularStringLiteral(cachingLexer, textControl, 1))
                return false;
            int offset2 = cachingLexer.TokenStart + 1;
            textControl.Document.DeleteText(new TextRange(this.LexerToTextControl(textControl, offset1), this.LexerToTextControl(textControl, offset2)));
            return true;
        }

        private bool SkipToRegularStringLiteral(CachingLexer lexer, ITextControl textControl, int deltha)
        {
            while (lexer.TokenType != null && lexer.TokenType.IsWhitespace)
                lexer.Advance(deltha);
            return lexer.TokenType == NTriplesTokenType.STRING_LITERAL && (int)textControl.Document.Buffer[this.LexerToTextControl(textControl, lexer.TokenStart)] != 64;
        }

        private bool DoHandleBackspacePressed(ITextControl textControl)
        {
            if (ITextControlSelectionEx.RandomRange(textControl.Selection).Length > 0)
                return false;
            int index = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            if (index <= 0)
                return false;
            char ch = textControl.Document.Buffer[ITextControlCaretEx.Offset(textControl.Caret) - 1];
            switch (ch)
            {
            case '"':
            case '\'':
            case '(':
            case '[':
            case '{':
                CachingLexer cachingLexer = this.GetCachingLexer(textControl);
                if (!cachingLexer.FindTokenAt(index))
                    return false;
                if (cachingLexer.TokenType == NTriplesTokenType.STRING_LITERAL || cachingLexer.TokenType == NTriplesTokenType.CHARACTER_LITERAL)
                {
                    if (index != cachingLexer.TokenEnd - 1 || (int)ch != (int)textControl.Document.Buffer[ITextControlCaretEx.Offset(textControl.Caret)])
                        return false;
                    int num = this.LexerToTextControl(textControl, index);
                    ITextControlCaretEx.MoveTo(textControl.Caret, num - 1, CaretVisualPlacement.DontScrollIfVisible);
                    textControl.Document.DeleteText(new TextRange(num - 1, num + 1));
                    return true;
                }
                else
                {
                    if (cachingLexer.TokenStart != index)
                        return false;
                    TokenNodeType tokenType1 = cachingLexer.TokenType;
                    cachingLexer.Advance(-1);
                    TokenNodeType tokenType2 = cachingLexer.TokenType;
                    if (tokenType2 == NTriplesTokenType.LBRACE && tokenType1 == NTriplesTokenType.RBRACE || tokenType2 == NTriplesTokenType.LBRACKET && tokenType1 == NTriplesTokenType.RBRACKET || tokenType2 == NTriplesTokenType.LPARENTH && tokenType1 == NTriplesTokenType.RPARENTH)
                    {
                        var csharpBracketMatcher = new NTriplesBracketMatcher();
                        TokenNodeType tokenType3 = tokenType2;
                        int currentPosition = cachingLexer.CurrentPosition;
                        do
                        {
                            if (tokenType3 == tokenType2 && csharpBracketMatcher.IsStackEmpty())
                                currentPosition = cachingLexer.CurrentPosition;
                            else if (!csharpBracketMatcher.ProceedStack(tokenType3))
                                break;
                            cachingLexer.Advance(-1);
                        }
                        while ((tokenType3 = cachingLexer.TokenType) != null);
                        cachingLexer.CurrentPosition = currentPosition;
                        int num = this.LexerToTextControl(textControl, index);
                        int startOffset;
                        ITextControlCaretEx.MoveTo(textControl.Caret, startOffset = num - 1, CaretVisualPlacement.DontScrollIfVisible);
                        textControl.Document.DeleteText(new TextRange(startOffset, startOffset + (csharpBracketMatcher.FindMatchingBracket(cachingLexer) ? 2 : 1)));
                        return true;
                    }
                    else
                    {
                        IBuffer buffer = cachingLexer.Buffer;
                        if (tokenType1 != NTriplesTokenType.BAD_CHARACTER || (int)buffer[index] != 39 || (tokenType2 != NTriplesTokenType.BAD_CHARACTER || (int)buffer[index - 1] != 39))
                            return false;
                        int offset = index - 1;
                        int num = this.LexerToTextControl(textControl, offset);
                        ITextControlCaretEx.MoveTo(textControl.Caret, num, CaretVisualPlacement.DontScrollIfVisible);
                        textControl.Document.DeleteText(new TextRange(num, num + 2));
                        return true;
                    }
                }
            default:
                return false;
            }
        }

        protected bool HandleDelPressed(IActionContext context)
        {
            ITextControl textControl = context.TextControl;
            return this.DoHandleDelPressed(textControl) || this.DoHandleDelOnStringsConcat(textControl);
        }

        private bool DoHandleDelOnStringsConcat(ITextControl textControl)
        {
            if (ITextControlSelectionEx.RandomRange(textControl.Selection).Length > 0)
                return false;
            int charPos = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
            if (charPos <= 0)
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart delete"))
                return this.DoHandleDeletingPlus(textControl, charPos);
        }

        private bool DoHandleDelPressed(ITextControl textControl)
        {
            if (!TypingAssistUtils.HandleDeleteNewLine((TypingAssistBase)this, textControl, new Func<CachingLexer, bool>(this.IsNewLineToken)))
                return false;
            using (ICommandProcessorEx.UsingCommand(this.CommandProcessor, "Smart delete"))
            {
                IDocument document = textControl.Document;
                CachingLexer cachingLexer = this.GetCachingLexer(textControl);
                int num = this.TextControlToLexer(textControl, ITextControlCaretEx.Offset(textControl.Caret));
                if (num < 0 || !cachingLexer.FindTokenAt(num) || (cachingLexer.TokenType != NTriplesTokenType.END_OF_LINE_COMMENT || cachingLexer.TokenStart >= num))
                    return true;
                int startOffset = this.LexerToTextControl(textControl, cachingLexer.TokenStart);
                int endOffset = this.LexerToTextControl(textControl, cachingLexer.TokenEnd);
                if (startOffset >= 0 && endOffset >= 0)
                {
                    string text = document.GetText(new TextRange(startOffset, endOffset));
                    if (text.StartsWith("///") && text.Substring(num - cachingLexer.TokenStart).StartsWith("///"))
                        document.DeleteText(new TextRange(this.LexerToTextControl(textControl, num), this.LexerToTextControl(textControl, num + 3)));
                }
                return true;
            }
        }

        protected virtual bool IsNewLineToken(CachingLexer lexer)
        {
            return lexer.TokenType == NTriplesTokenType.NEW_LINE;
        }

        public static string GetCodeBehindIndent(CSharpTypingAssistBase typingAssist, ITextControl textControl, int lexerOffset, string indent)
        {
            int num1 = typingAssist.LexerToTextControl(textControl, lexerOffset);
            CachingLexer cachingLexer = typingAssist.GetCachingLexer(textControl);
            if (cachingLexer == null || !cachingLexer.FindTokenAt(lexerOffset - 1))
                return indent;
            while (cachingLexer.TokenType == NTriplesTokenType.WHITE_SPACE)
                cachingLexer.Advance(-1);
            if (cachingLexer.TokenType == null || cachingLexer.TokenType is INTriplesTokenNodeType)
                return indent;
            int tokenEnd = cachingLexer.TokenEnd;
            int num2 = indent.Length - (num1 - tokenEnd);
            indent = new string(' ', num2) + indent.Substring(num2);
            return indent;
        }

        public abstract bool QuickCheckAvailability(ITextControl textControl, IPsiSourceFile projectFile);
    }*/

    [SolutionComponent]
    public class PsiTypingAssist : TypingAssistLanguageBase<NTriplesLanguage, ICodeFormatter /*concrete class*/>, ITypingHandler
    {
        public PsiTypingAssist(
            Lifetime lifetime,
            ISolution solution,
            ISettingsStore settingsStore,
            CachingLexerService cachingLexerService,
            ICommandProcessor commandProcessor,
            ITypingAssistManager typingAssistManager,
            IPsiServices psiServices)
            : base(solution, settingsStore, cachingLexerService, commandProcessor, psiServices)
        {
            typingAssistManager.AddTypingHandler(
                lifetime, '{', this, this.HandleLeftBraceTyped, this.IsTypingSmartLBraceHandlerAvailable);
            typingAssistManager.AddTypingHandler(
                lifetime, '(', this, this.HandleLeftBracketOrParenthTyped, this.IsTypingSmartParenthesisHandlerAvailable);
            typingAssistManager.AddTypingHandler(
                lifetime, '[', this, this.HandleLeftBracketOrParenthTyped, this.IsTypingSmartParenthesisHandlerAvailable);
            typingAssistManager.AddTypingHandler(
                lifetime, ')', this, this.HandleRightBracketTyped, this.IsTypingSmartParenthesisHandlerAvailable);
            typingAssistManager.AddTypingHandler(
                lifetime, '"', this, this.HandleQuoteTyped, this.IsTypingSmartParenthesisHandlerAvailable);

            //typingAssistManager.AddActionHandler(lifetime, TextControlActions.ENTER_ACTION_ID, this, this.HandleEnterPressed, this.IsActionHandlerAvailabile);
            //typingAssistManager.AddTypingHandler(lifetime, ';', this, this.HandleSemicolonTyped, this.IsTypingSmartParenthesisHandlerAvailable);

            /*
            typingAssistManager.AddActionHandler(lifetime, TextControlActions.BACKSPACE_ACTION_ID, this, this.HandleEnterPressed, this.IsActionHandlerAvailabile);*/
        }

        public bool QuickCheckAvailability(ITextControl textControl, IPsiSourceFile projectFile)
        {
            return projectFile.LanguageType.Is<NTriplesProjectFileType>();
        }

        protected override bool IsSupported(ITextControl textControl)
        {
            IPsiSourceFile projectFile = textControl.Document.GetPsiSourceFile(this.Solution);
            if (projectFile == null || !projectFile.LanguageType.Is<NTriplesProjectFileType>() || !projectFile.IsValid())
            {
                return false;
            }

            return projectFile.Properties.ShouldBuildPsi;
        }

        private static bool NeedAutoinsertCloseBracket(CachingLexer lexer)
        {
            using (LexerStateCookie.Create(lexer))
            {
                TokenNodeType typedToken = lexer.TokenType;

                // find the leftmost non-closed bracket (including typed) of typed class so that there are no opened brackets of other type
                var bracketMatcher = new NTriplesBracketMatcher();
                TokenNodeType tokenType = typedToken;

                int leftParenthPos = lexer.CurrentPosition;
                do
                {
                    if (tokenType == typedToken && bracketMatcher.IsStackEmpty())
                    {
                        leftParenthPos = lexer.CurrentPosition;
                    }
                    else if (!bracketMatcher.ProceedStack(tokenType))
                    {
                        break;
                    }
                    lexer.Advance(-1);
                } while ((tokenType = lexer.TokenType) != null);

                // Try to find the matched pair bracket
                lexer.CurrentPosition = leftParenthPos;

                return !bracketMatcher.FindMatchingBracket(lexer);
            }
        }

        private bool AutoinsertRBrace(ITextControl textControl, CachingLexer lexer)
        {
            int charPos = lexer.TokenEnd;
            int lBracePos = charPos - 1;
            if (lexer.TokenType != NTriplesTokenType.L_BRACE)
            {
                return false;
            }

            if (!NeedAutoinsertCloseBracket(lexer))
            {
                return false;
            }

            // insert R_BRACE next to the L_BRACE
            IDocument document = textControl.Document;
            int position = lBracePos;
            if (position < 0)
            {
                return false;
            }

            document.InsertText(position + 1, "}");

            // Commit PSI
            IFile file = this.CommitPsi(textControl);
            if (file == null)
            {
                return false;
            }

            TreeTextRange treeLBraceRange = file.Translate(new DocumentRange(document, new TextRange(lBracePos + 1)));
            if (!treeLBraceRange.IsValid())
            {
                return false;
            }

            var rBraceToken = file.FindTokenAt(treeLBraceRange.StartOffset) as ITokenNode;
            if (rBraceToken == null || rBraceToken.GetTokenType() != NTriplesTokenType.R_BRACE)
            {
                return false;
            }
            TreeOffset positionForRBrace = rBraceToken.GetTreeTextRange().EndOffset;


            // move RBRACE to another position, if necessary
            DocumentRange documentRangeForRBrace = file.GetDocumentRange(positionForRBrace);
            if (documentRangeForRBrace.IsValid() && documentRangeForRBrace.TextRange.StartOffset != lBracePos + 1)
            {
                int pos = documentRangeForRBrace.TextRange.StartOffset;
                if (pos >= 0)
                {
                    document.InsertText(pos, "}");
                    document.DeleteText(new TextRange(lBracePos + 1, lBracePos + 2));
                }
            }

            return true;
        }

        private void DoFormatStatementOnSemicolon(ITextControl textControl)
        {
            IFile file = this.CommitPsi(textControl);
            if (file == null)
            {
                return;
            }
            int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset());
            if (charPos < 0)
            {
                return;
            }

            var tokenNode = file.FindTokenAt(textControl.Document, charPos - 1) as ITokenNode;
            if (tokenNode == null || tokenNode.GetTokenType() != NTriplesTokenType.SEMICOLON)
            {
                return;
            }

            var node = tokenNode.Parent;

            // do format if semicolon finished the statement
            if (node == null || tokenNode.NextSibling != null)
            {
                return;
            }

            // Select the correct start node for formatting
            ITreeNode startNode = node.FindFormattingRangeToLeft() ?? node.FirstChild;

            //NTriplesCodeFormatter codeFormatter = this.GetCodeFormatter(tokenNode);
            using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Format code"))
            {
                using (WriteLockCookie.Create())
                {
                    //codeFormatter.Format(startNode, tokenNode, CodeFormatProfile.DEFAULT);
                }
            }

            DocumentRange newPosition = tokenNode.GetDocumentRange();
            if (newPosition.IsValid())
            {
                textControl.Caret.MoveTo(newPosition.TextRange.EndOffset, CaretVisualPlacement.DontScrollIfVisible);
            }
        }

        private bool DoHandleEnterAfterLBracePressed(ITextControl textControl)
        {
            int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset());
            if (charPos <= 0)
            {
                return false;
            }

            // Check that token before caret is L_BRACE
            CachingLexer lexer = this.GetCachingLexer(textControl);
            if (!lexer.FindTokenAt(charPos - 1))
            {
                return false;
            }

            if (lexer.TokenType == NTriplesTokenType.WHITE_SPACE)
            {
                charPos = lexer.TokenStart;
                lexer.Advance(-1);
            }
            if (lexer.TokenType != NTriplesTokenType.L_BRACE)
            {
                return false;
            }

            int lBracePos = lexer.TokenStart;

            // If necessary, do R_BRACE autoinsert 
            bool braceInserted = false;
            if (this.GetTypingAssistOption(textControl, TypingAssistOptions.BraceInsertTypeExpression) ==
                SmartBraceInsertType.ON_ENTER)
            {
                braceInserted = this.AutoinsertRBrace(textControl, lexer);

                // Resync with modified text
                lexer = this.GetCachingLexer(textControl);
                lexer.FindTokenAt(lBracePos);
                Logger.Assert(
                    lexer.TokenType == NTriplesTokenType.L_BRACE,
                    "The condition (lexer.tokenTypeName == NTriplesTokenType.L_BRACE) is false.");
            }

            // Find the matched R_BRACE and check they are on the same line
            int rBracePos;
            if (!new NTriplesBracketMatcher().FindMatchingBracket(lexer, out rBracePos))
            {
                return false;
            }

            int textControlLBracePos = lBracePos;
            int textControlRBracePos = rBracePos;
            if (textControlLBracePos < 0 || textControlRBracePos < 0 ||
                (!braceInserted &&
                 textControl.Document.GetCoordsByOffset(textControlLBracePos).Line !=
                 textControl.Document.GetCoordsByOffset(textControlRBracePos).Line))
            {
                return false;
            }

            // Commit PSI for current document
            IFile file = this.CommitPsi(textControl);
            if (file == null)
            {
                return false;
            }

            // Find nodes at the tree for braces
            TreeOffset lBraceTreePos = file.Translate(new DocumentRange(textControl.Document, lBracePos)).StartOffset;
            TreeOffset rBraceTreePos = file.Translate(new DocumentRange(textControl.Document, rBracePos)).StartOffset;

            var lBraceNode = file.FindTokenAt(lBraceTreePos) as ITokenNode;
            if (lBraceNode == null || lBraceNode.GetTokenType() != NTriplesTokenType.L_BRACE)
            {
                return false;
            }

            var rBraceNode = file.FindTokenAt(rBraceTreePos) as ITokenNode;
            if (rBraceNode == null || rBraceNode.GetTokenType() != NTriplesTokenType.R_BRACE)
            {
                return false;
            }

            TreeTextRange reparseTreeOffset = file.Translate(new DocumentRange(textControl.Document, charPos));
            const string dummyText = "a";

            return this.ReformatForSmartEnter(dummyText, textControl, file, reparseTreeOffset, lBraceTreePos, rBraceTreePos);
        }

        private void DoSmartIndentOnEnter(ITextControl textControl)
        {
            var originalOffset = textControl.Caret.Offset();

            var offset = this.TextControlToLexer(textControl, originalOffset);
            var mixedLexer = this.GetCachingLexer(textControl);

            // if there is something on that line, then use existing text
            if (offset <= 0 || !mixedLexer.FindTokenAt(offset - 1))
            {
                return;
            }

            if (mixedLexer.TokenType == NTriplesTokenType.STRING_LITERAL)
            {
                return;
            }

            if (offset <= 0 || !mixedLexer.FindTokenAt(offset))
            {
                return;
            }

            while (mixedLexer.TokenType == NTriplesTokenType.WHITE_SPACE)
            {
                mixedLexer.Advance();
            }

            offset = mixedLexer.TokenType == null
                         ? offset
                         : mixedLexer.TokenStart;
            var extraText = (mixedLexer.TokenType == NTriplesTokenType.NEW_LINE || mixedLexer.TokenType == null)
                                ? "foo "
                                : String.Empty;

            var projectItem = textControl.Document.GetPsiSourceFile(this.Solution);
            if (projectItem == null || !projectItem.IsValid())
            {
                return;
            }

            var services = Solution.GetPsiServices();
            using (services.Transactions.DocumentTransactionManager.CreateTransactionCookie(DefaultAction.Commit, "Typing assist"))
            {
                // If the new line is empty, the do default indentation
                int lexerOffset = offset;
                if (extraText.Length > 0)
                {
                    textControl.Document.InsertText(lexerOffset, extraText);
                }

                services.Files.CommitAllDocuments();
                var file = projectItem.GetPsiFile<NTriplesLanguage>(new DocumentRange(textControl.Document, offset));

                if (file == null)
                {
                    return;
                }

                var rangeInJsTree = file.Translate(new DocumentOffset(textControl.Document, offset));

                if (!rangeInJsTree.IsValid())
                {
                    if (extraText.Length > 0)
                    {
                        textControl.Document.DeleteText(new TextRange(lexerOffset, lexerOffset + extraText.Length));
                    }
                    return;
                }

                var tokenNode = file.FindTokenAt(rangeInJsTree) as ITokenNode;
                if (tokenNode == null)
                {
                    if (extraText.Length > 0)
                    {
                        textControl.Document.DeleteText(new TextRange(lexerOffset, lexerOffset + extraText.Length));
                    }
                    return;
                }

                //TODO
                //NTriplesCodeFormatter codeFormatter = this.GetCodeFormatter(file);
                int offsetInToken = rangeInJsTree.Offset - tokenNode.GetTreeStartOffset().Offset;

                using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Typing assist"))
                {
                    Lifetimes.Using(
                        lifetime =>
                        {
                            var boundSettingsStore =
                                this.SettingsStore.CreateNestedTransaction(lifetime, "PsiTypingAssist")
                                    .BindToContextTransient(textControl.ToContextRange());
                            var prevToken = tokenNode.GetPrevToken();
                            if (prevToken == null)
                            {
                                return;
                            }

                                //TODO changes was made here
                                /*if (tokenNode.Parent is IParenExpression || prevToken.Parent is IParenExpression)
              {
                var node = tokenNode.Parent;
                if (prevToken.Parent is IParenExpression)
                {
                  node = prevToken.Parent;
                }
                codeFormatter.Format(node.FirstChild, node.LastChild,
                  CodeFormatProfile.DEFAULT, NullProgressIndicator.Instance, boundSettingsStore);
              }*/
                            else
                            {
                                //TODO codeFormatter.Format(prevToken, tokenNode, CodeFormatProfile.INDENT, NullProgressIndicator.Instance, boundSettingsStore);
                            }
                        });
                    offset = file.GetDocumentRange(tokenNode.GetTreeStartOffset()).TextRange.StartOffset +
                             offsetInToken;
                }

                if (extraText.Length > 0)
                {
                    lexerOffset = offset;
                    textControl.Document.DeleteText(new TextRange(lexerOffset, lexerOffset + extraText.Length));
                }
            }

            textControl.Caret.MoveTo(offset, CaretVisualPlacement.DontScrollIfVisible);
        }

        private bool HandleEnterPressed(IActionContext context)
        {
            ITextControl textControl = context.TextControl;
            if (this.GetTypingAssistOption(textControl, TypingAssistOptions.BraceInsertTypeExpression) ==
                SmartBraceInsertType.DISABLED)
            {
                return false;
            }

            using (this.CommandProcessor.UsingCommand("Smart Enter"))
            {
                using (WriteLockCookie.Create())
                {
                    if (this.DoHandleEnterAfterLBracePressed(textControl))
                    {
                        return true;
                    }

                    context.CallNext();
                    this.DoSmartIndentOnEnter(textControl);
                    return true;
                }
            }
        }

        private bool HandleLeftBraceTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            using (this.CommandProcessor.UsingCommand("Smart LBRACE"))
            {
                typingContext.CallNext();

                // check if typed char is a token
                int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset() - 1);
                CachingLexer lexer = this.GetCachingLexer(textControl);
                if (charPos < 0 || !lexer.FindTokenAt(charPos) || lexer.TokenStart != charPos)
                {
                    return true;
                }

                if (NeedAutoinsertCloseBracket(lexer))
                {
                    if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                    {
                        return true;
                    }

                    this.AutoinsertRBrace(textControl, lexer);
                    int position = charPos + 1;
                    if (position >= 0)
                    {
                        textControl.Caret.MoveTo(position, CaretVisualPlacement.DontScrollIfVisible);
                    }
                }
            }
            return true;
        }

        private bool HandleLeftBracketOrParenthTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            using (this.CommandProcessor.UsingCommand("Smart " + typingContext.Char))
            {
                typingContext.CallNext();
                using (WriteLockCookie.Create())
                {
                    // check if typed char is a token
                    CachingLexer lexer = this.GetCachingLexer(textControl);
                    int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset() - 1);
                    if (charPos < 0 || !lexer.FindTokenAt(charPos) || lexer.TokenStart != charPos)
                    {
                        return true;
                    }
                    if (lexer.TokenType != NTriplesTokenType.L_BRACKET && lexer.TokenType != NTriplesTokenType.L_PARENTHESES)
                    {
                        return true;
                    }

                    // check that next token is good one
                    TokenNodeType nextTokenType = lexer.LookaheadToken(1);
                    if (nextTokenType != null && nextTokenType != NTriplesTokenType.WHITE_SPACE &&
                        nextTokenType != NTriplesTokenType.NEW_LINE &&
                        nextTokenType != NTriplesTokenType.END_OF_LINE_COMMENT && nextTokenType != NTriplesTokenType.SEMICOLON &&
                        nextTokenType != NTriplesTokenType.R_BRACKET && nextTokenType != NTriplesTokenType.R_BRACE &&
                        nextTokenType != NTriplesTokenType.R_PARENTHESES)
                    {
                        return true;
                    }

                    if (NeedAutoinsertCloseBracket(lexer))
                    {
                        if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
                        {
                            return true;
                        }

                        char c = typingContext.Char;
                        int insertPos = charPos;
                        if (insertPos >= 0)
                        {
                            textControl.Document.InsertText(
                                insertPos + 1,
                                c == '('
                                    ? ")"
                                    : c == '['
                                          ? "]"
                                          : "}");
                            textControl.Caret.MoveTo(insertPos + 1, CaretVisualPlacement.DontScrollIfVisible);
                        }
                    }
                }
            }
            return true;
        }

        private bool HandleQuoteTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
            {
                return false;
            }

            using (this.CommandProcessor.UsingCommand("Smart quote"))
            {
                TextControlUtil.DeleteSelection(textControl);
                textControl.FillVirtualSpaceUntilCaret();

                CachingLexer lexer = this.GetCachingLexer(textControl);
                IBuffer buffer = lexer.Buffer;
                int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset());
                TokenNodeType correspondingTokenType = NTriplesTokenType.BAD_CHARACTER; // was STRING_LITERAL;

                if (charPos < 0 || !lexer.FindTokenAt(charPos))
                {
                    return false;
                }

                TokenNodeType tokenType = lexer.TokenType;

                // check if we should skip the typed char
                if (charPos < buffer.Length && buffer[charPos] == typingContext.Char &&
                    tokenType == correspondingTokenType && lexer.TokenStart != charPos && buffer[lexer.TokenStart] != '@')
                {
                    int position = charPos;
                    if (position >= 0)
                    {
                        textControl.Caret.MoveTo(position + 1, CaretVisualPlacement.DontScrollIfVisible);
                    }
                    return true;
                }

                // check that next token is a good one
                if (tokenType != null && !this.IsStopperTokenForStringLiteral(tokenType))
                {
                    return false;
                }


                // find next not whitespace token
                while (lexer.TokenType == NTriplesTokenType.WHITE_SPACE)
                {
                    lexer.Advance();
                }

                bool doInsertPairQuote = (lexer.TokenType == correspondingTokenType) &&
                                         ((lexer.TokenEnd > lexer.TokenStart + 1) &&
                                          (lexer.Buffer[lexer.TokenStart] == typingContext.Char) &&
                                          (lexer.Buffer[lexer.TokenEnd - 1] == typingContext.Char));

                // do inserting of the requested char and updating of the lexer
                typingContext.CallNext();
                lexer = this.GetCachingLexer(textControl);
                //        charPos = TextControlToLexer(textControl, textControl.CaretModel.Offset - 1);

                if (!doInsertPairQuote)
                {
                    // check if the typed char is the beginning of the corresponding token
                    if (!lexer.FindTokenAt(charPos))
                    {
                        return true;
                    }

                    bool isStringWithAt = lexer.TokenType == NTriplesTokenType.STRING_LITERAL && lexer.TokenStart == charPos - 1 &&
                                          lexer.Buffer[lexer.TokenStart] == '@';
                    if ((lexer.TokenStart != charPos) && !isStringWithAt)
                    {
                        return true;
                    }

                    // check if there is unclosed token of the corresponding type up to the end of the source line
                    int newPos = charPos;
                    if (newPos < 0)
                    {
                        return true;
                    }

                    DocumentCoords documentCoords = textControl.Document.GetCoordsByOffset(newPos);
                    int offset = textControl.Document.GetLineEndOffsetNoLineBreak(documentCoords.Line) - 1;

                    int lexerOffset = this.TextControlToLexer(textControl, offset);
                    if (lexerOffset >= 0)
                    {
                        lexer.FindTokenAt(lexerOffset);
                    }
                    if (lexerOffset < 0 || lexer.TokenType == null)
                    {
                        charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset() - 1);
                        if (charPos >= 0)
                        {
                            lexer.FindTokenAt(charPos);
                        }
                        else
                        {
                            return true;
                        }
                    }

                    doInsertPairQuote = (lexer.TokenType == correspondingTokenType) &&
                                        ((lexer.TokenEnd == lexer.TokenStart + 1) ||
                                         (lexer.Buffer[lexer.TokenEnd - 1] != typingContext.Char) ||
                                         (isStringWithAt && (lexer.TokenStart == charPos - 1) && (lexer.TokenEnd != charPos + 1)));
                }

                // insert paired quote
                if (doInsertPairQuote)
                {
                    charPos++;
                    int documentPos = charPos;
                    if (documentPos >= 0)
                    {
                        textControl.Document.InsertText(
                            documentPos,
                            typingContext.Char == '\''
                                ? "'"
                                : "\"");
                        textControl.Caret.MoveTo(documentPos, CaretVisualPlacement.DontScrollIfVisible);
                    }
                }
            }

            return true;
        }

        private bool HandleRightBracketTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
            {
                return false;
            }

            using (this.CommandProcessor.UsingCommand("Smart bracket"))
            {
                TextControlUtil.DeleteSelection(textControl);

                int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset());
                CachingLexer lexer = this.GetCachingLexer(textControl);
                if (charPos < 0 || !lexer.FindTokenAt(charPos) || lexer.TokenStart != charPos)
                {
                    return false;
                }

                if (this.NeedSkipCloseBracket(lexer, typingContext.Char))
                {
                    int position = charPos + 1;
                    if (position >= 0)
                    {
                        textControl.Caret.MoveTo(position, CaretVisualPlacement.DontScrollIfVisible);
                    }
                }
                else
                {
                    typingContext.CallNext();
                }
            }

            return true;
        }

        private bool HandleSemicolonTyped(ITypingContext typingContext)
        {
            ITextControl textControl = typingContext.TextControl;
            if (typingContext.EnsureWritable() != EnsureWritableResult.SUCCESS)
            {
                return false;
            }

            using (this.CommandProcessor.UsingCommand("Smart ;"))
            {
                TextControlUtil.DeleteSelection(textControl);

                textControl.FillVirtualSpaceUntilCaret();
                int charPos = this.TextControlToLexer(textControl, textControl.Caret.Offset());
                CachingLexer lexer = this.GetCachingLexer(textControl);
                if (charPos < 0 || !lexer.FindTokenAt(charPos) || lexer.TokenStart != charPos ||
                    lexer.TokenType != NTriplesTokenType.SEMICOLON)
                {
                    typingContext.CallNext();
                }
                else
                {
                    int position = charPos + 1;
                    if (position < 0)
                    {
                        return true;
                    }
                    textControl.Caret.MoveTo(position, CaretVisualPlacement.DontScrollIfVisible);
                }

                // format statement
                if (this.GetTypingAssistOption(textControl, TypingAssistOptions.FormatStatementOnSemicolonExpression))
                {
                    this.DoFormatStatementOnSemicolon(textControl);
                }
                return true;
            }
        }

        private bool IsStopperTokenForStringLiteral(TokenNodeType tokenType)
        {
            return tokenType == NTriplesTokenType.WHITE_SPACE || tokenType == NTriplesTokenType.NEW_LINE ||
                   tokenType == NTriplesTokenType.END_OF_LINE_COMMENT ||
                   tokenType == NTriplesTokenType.SEMICOLON || tokenType == NTriplesTokenType.COMMA ||
                   tokenType == NTriplesTokenType.R_BRACKET || tokenType == NTriplesTokenType.R_BRACE ||
                   tokenType == NTriplesTokenType.R_PARENTHESES || tokenType == NTriplesTokenType.STRING_LITERAL;
        }

        private bool NeedSkipCloseBracket(CachingLexer lexer, char charTyped)
        {
            // check if the next token matches the typed char
            TokenNodeType nextToken = lexer.TokenType;
            if ((charTyped == ')' && nextToken != NTriplesTokenType.R_PARENTHESES) ||
                (charTyped == ']' && nextToken != NTriplesTokenType.R_BRACKET) ||
                (charTyped == '}' && nextToken != NTriplesTokenType.R_BRACE))
            {
                return false;
            }

            // find the leftmost non-closed bracket (excluding typed) of typed class so that there are no opened brackets of other type
            var bracketMatcher = new NTriplesBracketMatcher();
            TokenNodeType searchTokenType = charTyped == ')'
                                                ? NTriplesTokenType.L_PARENTHESES
                                                : charTyped == ']'
                                                      ? NTriplesTokenType.L_BRACKET
                                                      : NTriplesTokenType.L_BRACE;
            int? leftParenthPos = null;
            TokenNodeType tokenType;
            for (lexer.Advance(-1); (tokenType = lexer.TokenType) != null; lexer.Advance(-1))
            {
                if (tokenType == searchTokenType && bracketMatcher.IsStackEmpty())
                {
                    leftParenthPos = lexer.CurrentPosition;
                }
                else if (!bracketMatcher.ProceedStack(tokenType))
                {
                    break;
                }
            }

            // proceed with search result
            if (leftParenthPos == null)
            {
                return false;
            }
            lexer.CurrentPosition = leftParenthPos.Value;
            return bracketMatcher.FindMatchingBracket(lexer);
        }

        private bool ReformatForSmartEnter(
            string dummyText,
            ITextControl textControl,
            IFile file,
            TreeTextRange reparseTreeOffset,
            TreeOffset lBraceTreePos,
            TreeOffset rBraceTreePos,
            bool insertEnterAfter = false)
        {
            // insert dummy text and reformat
            TreeOffset newCaretPos;
            var codeFormatter = this.GetCodeFormatter(file);

            using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Typing assist"))
            {
                string newLine = Environment.NewLine;
                string textToInsert = newLine + dummyText;
                if (insertEnterAfter)
                {
                    textToInsert = textToInsert + newLine;
                }
                file = file.ReParse(reparseTreeOffset, textToInsert);
                if (file == null)
                {
                    return false;
                }

                ITreeNode lBraceNode = file.FindTokenAt(lBraceTreePos);
                if (lBraceNode == null)
                {
                    return false;
                }

                var dummyNode = file.FindTokenAt(reparseTreeOffset.StartOffset + newLine.Length) as ITokenNode;

                var languageService = file.Language.LanguageService();
                if (languageService == null)
                {
                    return false;
                }

                while (dummyNode != null && languageService.IsFilteredNode(dummyNode))
                {
                    dummyNode = dummyNode.GetNextToken();
                }

                if (dummyNode == null)
                {
                    return false;
                }

                var rBraceNode = file.FindTokenAt(
                    rBraceTreePos + newLine.Length + dummyText.Length + (insertEnterAfter
                                                                             ? newLine.Length
                                                                             : 0));

                var boundSettingsStore = this.SettingsStore.BindToContextTransient(textControl.ToContextRange());

                /*codeFormatter.Format(lBraceNode, CodeFormatProfile.DEFAULT, null, boundSettingsStore);
        codeFormatter.Format(
          rBraceNode.FindFormattingRangeToLeft(),
          rBraceNode,
          CodeFormatProfile.DEFAULT,
          null,
          boundSettingsStore);
        codeFormatter.Format(lBraceNode.Parent, CodeFormatProfile.INDENT, null);*/

                newCaretPos = dummyNode.GetTreeStartOffset();
                file = file.ReParse(new TreeTextRange(newCaretPos, newCaretPos + dummyText.Length), "");
                Assertion.Assert(file != null, "fileFullName != null");
            }

            // dposition cursor
            DocumentRange newCaretPosition = file.GetDocumentRange(newCaretPos);
            if (newCaretPosition.IsValid())
            {
                textControl.Caret.MoveTo(newCaretPosition.TextRange.StartOffset, CaretVisualPlacement.DontScrollIfVisible);
            }

            return true;
        }
    }
}
