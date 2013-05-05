using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
  public class SecretIndentVisitor : TreeNodeVisitor<FormattingStageContext, string>
  {
    private const string StandartIndent = "  ";
    private readonly IDictionary<ITreeNode, string> myCache;
    private readonly bool myInTypingAssist;
    private readonly SecretIndentCache myIndentCache;

    public SecretIndentVisitor(SecretIndentCache indentCache, bool inTypingAssist)
    {
      this.myIndentCache = indentCache;
      this.myInTypingAssist = inTypingAssist;
      this.myCache = new Dictionary<ITreeNode, string>();
    }

      /*public override string VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, FormattingStageContext context)
      {
        string parentIndent = this.GetParentIndent(context.Parent);
        if (context.RightChild is IExtraDefinition)
        {
          return parentIndent + StandartIndent;
        }
        return this.myIndentCache.GetNodeIndent(extrasDefinitionParam);
      }

      public override string VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, FormattingStageContext context)
      {
        string parentIndent = this.GetParentIndent(context.Parent);
        if (context.RightChild is IOptionDefinition)
        {
          return parentIndent + StandartIndent;
        }
        return this.myIndentCache.GetNodeIndent(optionsDefinitionParam);
      }

      public override string VisitRuleBody(IRuleBody ruleBodyParam, FormattingStageContext context)
      {
        string parentIndent = this.GetParentIndent(context.Parent);
        return parentIndent + StandartIndent;
      }

      public override string VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, FormattingStageContext context)
      {
        string parentIndent = this.GetParentIndent(context.Parent);
        if (context.RightChild is IRuleBody)
        {
          return parentIndent + StandartIndent + StandartIndent;
        }
        var token = context.RightChild as ITokenNode;
        if ((token != null) && ((token.GetTokenType() == PsiTokenType.COLON) || (token.GetTokenType() == PsiTokenType.SEMICOLON)))
        {
          return parentIndent + StandartIndent;
        }
        return this.myIndentCache.GetNodeIndent(ruleDeclarationParam);
      }

      public override string VisitRuleDeclaredName(IRuleDeclaredName ruleDeclaredNameParam, FormattingStageContext context)
      {
        return this.myIndentCache.GetNodeIndent(ruleDeclaredNameParam);
      }

      public override string VisitNotChoiceExpression(INotChoiceExpression notChoiceExpressionParam, FormattingStageContext context)
      {
        return this.myIndentCache.GetNodeIndent(notChoiceExpressionParam);
      }

      public override string VisitPsiExpression(IPsiExpression psiExpressionParam, FormattingStageContext context)
      {
        string parentIndent = this.GetParentIndent(context.Parent);
        if (context.RightChild is IChoiceTail)
        {
          return parentIndent;
        }
        if (context.Parent is ParenExpression)
        {
          return parentIndent + StandartIndent;
        }
        return this.myIndentCache.GetNodeIndent(psiExpressionParam);
      }

      public override string VisitParenExpression(IParenExpression parenExpressionParam, FormattingStageContext context)
      {
        string parentIndent = this.GetParentIndent(context.Parent);
        if (context.RightChild is IPsiExpression)
        {
          return parentIndent + StandartIndent;
        }
        return parentIndent;
      }

      public override string VisitSequence(ISequence sequenceParam, FormattingStageContext context)
      {
        if (this.myInTypingAssist)
        {
          ITreeNode node = this.GetParent(context.Parent);
          string indent = this.GetIndentByOldParent(node);
          if (node.Parent is IParenExpression)
          {
            return this.VisitParenExpression(node.Parent as IParenExpression, new FormattingStageContext(new FormattingRange(node.PrevSibling, node)));
          }
          return indent;
        }
        return this.GetParentIndent(context.Parent);
      }

      public override string VisitChoiceTail(IChoiceTail choiceTailParam, FormattingStageContext context)
      {
        if (context.LeftChild is ICommentNode)
        {
          return GetParentIndent(choiceTailParam) + StandartIndent;
        }
        return base.VisitChoiceTail(choiceTailParam, context);
      }

      private string GetParentIndent(ITreeNode parent)
      {
        string result = "";
        ITreeNode node = this.GetParent(parent);
        if (node == null)
        {
          return "";
        }
        if (this.myCache.ContainsKey(node))
        {
          return this.myCache[node];
        }
        ITreeNode oldParent = node;
        node = node.PrevSibling;
        while (node != null)
        {
          if (!(node is IWhitespaceNode))
          {
            result = "";
            break;
          }
          if (node is NewLine)
          {
            break;
          }
          result += node.GetText();
          node = node.PrevSibling;
        }
        this.myCache.Add(oldParent, result);
        return result;
      }

      private string GetIndentByOldParent(ITreeNode parent)
      {
        string result = "";
        ITreeNode node = parent;
        if (node == null)
        {
          return "";
        }
        if (this.myCache.ContainsKey(node))
        {
          return this.myCache[node];
        }
        ITreeNode oldParent = node;
        node = node.PrevSibling;
        while (node != null)
        {
          if (!(node is IWhitespaceNode))
          {
            result = "";
            break;
          }
          if (node is NewLine)
          {
            break;
          }
          result += node.GetText();
          node = node.PrevSibling;
        }
        this.myCache.Add(oldParent, result);
        return result;
      }

      private ITreeNode GetParent(ITreeNode node)
      {
        //while((node != null) && ((node.PrevSibling == null) || (node.Parent is IParenExpression) || (node.Parent is IChoiceTail)))
        //while((node != null) && ((node.PrevSibling == null)))
        while ((node != null) && ((node.PrevSibling == null) || (node.Parent is IChoiceTail)))
        {
          node = node.Parent;
        }
        return node;
      }*/
  }
}
