using System;
using JetBrains.Application;
using JetBrains.Application.CommandProcessing;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Options;
using JetBrains.ReSharper.Feature.Services.TypingAssist;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.TextControl.Actions;
using JetBrains.TextControl.DataConstants;
using JetBrains.TextControl.Util;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.TypingAssist
{
  [SolutionComponent]
  public class PsiTypingAssist : TypingAssistLanguageBase<SecretLanguage, SecretCodeFormatter>, ITypingHandler
  {
    public PsiTypingAssist(Lifetime lifetime, ISolution solution, ISettingsStore settingsStore, CachingLexerService cachingLexerService, ICommandProcessor commandProcessor,
      ITypingAssistManager typingAssistManager, IPsiServices psiServices)
      : base(solution, settingsStore, cachingLexerService, commandProcessor, psiServices)
    {
      typingAssistManager.AddTypingHandler(lifetime, '{', this, this.HandleLeftBraceTyped, this.IsTypingSmartLBraceHandlerAvailable);
      typingAssistManager.AddTypingHandler(lifetime, '(', this, this.HandleLeftBracketOrParenthTyped, this.IsTypingSmartParenthesisHandlerAvailable);
      typingAssistManager.AddTypingHandler(lifetime, ')', this, this.HandleRightBracketTyped, this.IsTypingSmartParenthesisHandlerAvailable);
      typingAssistManager.AddTypingHandler(lifetime, '[', this, this.HandleLeftBracketOrParenthTyped, this.IsTypingSmartParenthesisHandlerAvailable);
      typingAssistManager.AddTypingHandler(lifetime, ';', this, this.HandleSemicolonTyped, this.IsTypingSmartParenthesisHandlerAvailable);
      typingAssistManager.AddTypingHandler(lifetime, '"', this, this.HandleQuoteTyped, this.IsTypingSmartParenthesisHandlerAvailable);
      typingAssistManager.AddActionHandler(lifetime, TextControlActions.ENTER_ACTION_ID, this, this.HandleEnterPressed, this.IsActionHandlerAvailabile);
    }

    #region ITypingHandler Members

    public bool QuickCheckAvailability(ITextControl textControl, IPsiSourceFile projectFile)
    {
      return projectFile.LanguageType.Is<SecretProjectFileType>();
    }

    #endregion

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

    private bool NeedSkipCloseBracket(CachingLexer lexer, char charTyped)
    {
      // check if the next token matches the typed char
      TokenNodeType nextToken = lexer.TokenType;
      if ((charTyped == ')' && nextToken != SecretTokenType.R_PARENTHESES) ||
        (charTyped == ']' && nextToken != SecretTokenType.R_BRACKET) ||
          (charTyped == '}' && nextToken != SecretTokenType.R_BRACE))
      {
        return false;
      }

      // find the leftmost non-closed bracket (excluding typed) of typed class so that there are no opened brackets of other type
      var bracketMatcher = new PsiBracketMatcher();
      TokenNodeType searchTokenType = charTyped == ')'
        ? SecretTokenType.L_PARENTHESES
        : charTyped == ']' ? SecretTokenType.L_BRACKET : SecretTokenType.L_BRACE;
      int? leftParenthPos = null;
      TokenNodeType tokenType;
      for (lexer.Advance(-1) ; (tokenType = lexer.TokenType) != null ; lexer.Advance(-1))
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

    private static bool NeedAutoinsertCloseBracket(CachingLexer lexer)
    {
      using (LexerStateCookie.Create(lexer))
      {
        TokenNodeType typedToken = lexer.TokenType;

        // find the leftmost non-closed bracket (including typed) of typed class so that there are no opened brackets of other type
        var bracketMatcher = new PsiBracketMatcher();
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

    private bool HandleEnterPressed(IActionContext context)
    {
      ITextControl textControl = context.TextControl;
      if (this.GetTypingAssistOption(textControl, TypingAssistOptions.BraceInsertTypeExpression) == SmartBraceInsertType.DISABLED)
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

      if (mixedLexer.TokenType == SecretTokenType.C_STYLE_COMMENT || mixedLexer.TokenType == SecretTokenType.STRING_LITERAL)
      {
        return;
      }

      if (offset <= 0 || !mixedLexer.FindTokenAt(offset))
      {
        return;
      }

      while (mixedLexer.TokenType == SecretTokenType.WHITE_SPACE)
      {
        mixedLexer.Advance();
      }

      offset = mixedLexer.TokenType == null ? offset : mixedLexer.TokenStart;
      var extraText = (mixedLexer.TokenType == SecretTokenType.NEW_LINE || mixedLexer.TokenType == null) ? "foo " : String.Empty;

      var projectItem = textControl.Document.GetPsiSourceFile(this.Solution);
      if (projectItem == null || !projectItem.IsValid())
      {
        return;
      }

      using (PsiManager.GetInstance(this.Solution).DocumentTransactionManager.CreateTransactionCookie(DefaultAction.Commit, "Typing assist"))
      {
        // If the new line is empty, the do default indentation
        int lexerOffset = offset;
        if (extraText.Length > 0)
        {
          textControl.Document.InsertText(lexerOffset, extraText);
        }

        this.PsiServices.PsiManager.CommitAllDocuments();
        var file = projectItem.GetPsiFile<PsiLanguage>(new DocumentRange(textControl.Document, offset));

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

        SecretCodeFormatter codeFormatter = this.GetCodeFormatter(file);
        int offsetInToken = rangeInJsTree.Offset - tokenNode.GetTreeStartOffset().Offset;

        using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Typing assist"))
        {
          Lifetimes.Using(
            lifetime =>
            {
              var boundSettingsStore = this.SettingsStore.CreateNestedTransaction(lifetime, "PsiTypingAssist").BindToContextTransient(textControl.ToContextRange());
              var prevToken = tokenNode.GetPrevToken();
              if (prevToken == null)
              {
                return;
              }

              if (tokenNode.Parent is IParenExpression || prevToken.Parent is IParenExpression)
              {
                var node = tokenNode.Parent;
                if (prevToken.Parent is IParenExpression)
                {
                  node = prevToken.Parent;
                }
                codeFormatter.Format(node.FirstChild, node.LastChild,
                  CodeFormatProfile.DEFAULT, NullProgressIndicator.Instance, boundSettingsStore);
              }
              else
              {
                codeFormatter.Format(prevToken, tokenNode,
                  CodeFormatProfile.INDENT, NullProgressIndicator.Instance, boundSettingsStore);
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

      if (lexer.TokenType == SecretTokenType.WHITE_SPACE)
      {
        charPos = lexer.TokenStart;
        lexer.Advance(-1);
      }
      if (lexer.TokenType != SecretTokenType.L_BRACE)
      {
        return false;
      }

      int lBracePos = lexer.TokenStart;

      // If necessary, do R_BRACE autoinsert 
      bool braceInserted = false;
      if (this.GetTypingAssistOption(textControl, TypingAssistOptions.BraceInsertTypeExpression) == SmartBraceInsertType.ON_ENTER)
      {
        braceInserted = this.AutoinsertRBrace(textControl, lexer);

        // Resync with modified text
        lexer = this.GetCachingLexer(textControl);
        lexer.FindTokenAt(lBracePos);
        Logger.Assert(lexer.TokenType == SecretTokenType.L_BRACE, "The condition (lexer.tokenTypeName == CSharpTokenType.L_BRACE) is false.");
      }

      // Find the matched R_BRACE and check they are on the same line
      int rBracePos;
      if (!new PsiBracketMatcher().FindMatchingBracket(lexer, out rBracePos))
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
      if (lBraceNode == null || lBraceNode.GetTokenType() != SecretTokenType.L_BRACE)
      {
        return false;
      }

      var rBraceNode = file.FindTokenAt(rBraceTreePos) as ITokenNode;
      if (rBraceNode == null || rBraceNode.GetTokenType() != SecretTokenType.R_BRACE)
      {
        return false;
      }

      TreeTextRange reparseTreeOffset = file.Translate(new DocumentRange(textControl.Document, charPos));
      const string dummyText = "a";

      return this.ReformatForSmartEnter(dummyText, textControl, file, reparseTreeOffset, lBraceTreePos, rBraceTreePos);
    }

    private bool ReformatForSmartEnter(string dummyText, ITextControl textControl, IFile file, TreeTextRange reparseTreeOffset, TreeOffset lBraceTreePos, TreeOffset rBraceTreePos, bool insertEnterAfter = false)
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
          dummyNode = dummyNode.GetNextToken();

        if (dummyNode == null)
          return false;

        var rBraceNode = file.FindTokenAt(rBraceTreePos + newLine.Length + dummyText.Length + (insertEnterAfter ? newLine.Length : 0));

        var boundSettingsStore = this.SettingsStore.BindToContextTransient(textControl.ToContextRange());

        codeFormatter.Format(lBraceNode, CodeFormatProfile.DEFAULT, null, boundSettingsStore);
        codeFormatter.Format(
          rBraceNode.FindFormattingRangeToLeft(),
          rBraceNode,
          CodeFormatProfile.DEFAULT,
          null,
          boundSettingsStore);
        codeFormatter.Format(lBraceNode.Parent, CodeFormatProfile.INDENT, null);

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
        TokenNodeType correspondingTokenType = SecretTokenType.STRING_LITERAL;

        if (charPos < 0 || !lexer.FindTokenAt(charPos))
        {
          return false;
        }

        TokenNodeType tokenType = lexer.TokenType;


        // check if we should skip the typed char
        if (charPos < buffer.Length && buffer[charPos] == typingContext.Char && tokenType == correspondingTokenType &&
          lexer.TokenStart != charPos && buffer[lexer.TokenStart] != '@')
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
        while (lexer.TokenType == SecretTokenType.WHITE_SPACE)
        {
          lexer.Advance();
        }

        bool doInsertPairQuote = (lexer.TokenType == correspondingTokenType) &&
          ((lexer.TokenEnd > lexer.TokenStart + 1) && (lexer.Buffer[lexer.TokenStart] == typingContext.Char) && (lexer.Buffer[lexer.TokenEnd - 1] == typingContext.Char));

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

          bool isStringWithAt = lexer.TokenType == SecretTokenType.STRING_LITERAL && lexer.TokenStart == charPos - 1 &&
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
            ((lexer.TokenEnd == lexer.TokenStart + 1) || (lexer.Buffer[lexer.TokenEnd - 1] != typingContext.Char) ||
              (isStringWithAt && (lexer.TokenStart == charPos - 1) && (lexer.TokenEnd != charPos + 1)));
        }

        // insert paired quote
        if (doInsertPairQuote)
        {
          charPos++;
          int documentPos = charPos;
          if (documentPos >= 0)
          {
            textControl.Document.InsertText(documentPos, typingContext.Char == '\'' ? "'" : "\"");
            textControl.Caret.MoveTo(documentPos, CaretVisualPlacement.DontScrollIfVisible);
          }
        }
      }

      return true;
    }

    private bool IsStopperTokenForStringLiteral(TokenNodeType tokenType)
    {
      return tokenType == SecretTokenType.WHITE_SPACE || tokenType == SecretTokenType.NEW_LINE ||
        tokenType == SecretTokenType.END_OF_LINE_COMMENT ||
          tokenType == SecretTokenType.SEMICOLON || tokenType == SecretTokenType.COMMA ||
            tokenType == SecretTokenType.R_BRACKET || tokenType == SecretTokenType.R_BRACE ||
              tokenType == SecretTokenType.R_PARENTHESES || tokenType == SecretTokenType.STRING_LITERAL;
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
          lexer.TokenType != SecretTokenType.SEMICOLON)
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
      if (tokenNode == null || tokenNode.GetTokenType() != SecretTokenType.SEMICOLON)
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
      ITreeNode startNode = node.FindFormattingRangeToLeft();
      if (startNode == null)
      {
        startNode = node.FirstChild;
      }

      SecretCodeFormatter codeFormatter = this.GetCodeFormatter(tokenNode);
      using (PsiTransactionCookie.CreateAutoCommitCookieWithCachesUpdate(this.PsiServices, "Format code"))
      {
        using (WriteLockCookie.Create())
        {
          codeFormatter.Format(startNode, tokenNode, CodeFormatProfile.DEFAULT);
        }
      }

      DocumentRange newPosition = tokenNode.GetDocumentRange();
      if (newPosition.IsValid())
      {
        textControl.Caret.MoveTo(newPosition.TextRange.EndOffset, CaretVisualPlacement.DontScrollIfVisible);
      }
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
          if (lexer.TokenType != SecretTokenType.L_BRACKET && lexer.TokenType != SecretTokenType.L_PARENTHESES)
          {
            return true;
          }

          // check that next token is good one
          TokenNodeType nextTokenType = lexer.LookaheadToken(1);
          if (nextTokenType != null && nextTokenType != SecretTokenType.WHITE_SPACE &&
            nextTokenType != SecretTokenType.NEW_LINE && 
              nextTokenType != SecretTokenType.END_OF_LINE_COMMENT && nextTokenType != SecretTokenType.SEMICOLON &&
                nextTokenType != SecretTokenType.R_BRACKET && nextTokenType != SecretTokenType.R_BRACE &&
                  nextTokenType != SecretTokenType.R_PARENTHESES)
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
              textControl.Document.InsertText(insertPos + 1, c == '(' ? ")" : c == '[' ? "]" : "}");
              textControl.Caret.MoveTo(insertPos + 1, CaretVisualPlacement.DontScrollIfVisible);
            }
          }
        }
      }
      return true;
    }

    private bool AutoinsertRBrace(ITextControl textControl, CachingLexer lexer)
    {
      int charPos = lexer.TokenEnd;
      int lBracePos = charPos - 1;
      if (lexer.TokenType != SecretTokenType.L_BRACE)
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
      if (rBraceToken == null || rBraceToken.GetTokenType() != SecretTokenType.R_BRACE)
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

    protected override bool IsSupported(ITextControl textControl)
    {
      IPsiSourceFile projectFile = textControl.Document.GetPsiSourceFile(this.Solution);
      if (projectFile == null || !projectFile.LanguageType.Is<SecretProjectFileType>() || !projectFile.IsValid())
      {
        return false;
      }

      return projectFile.Properties.ShouldBuildPsi;
    }
  }
}
