using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
  public class SecretFormattingVisitor : TreeNodeVisitor<FormattingStageContext, IEnumerable<string>>
  {
    private readonly bool myIsGenerated;

    public SecretFormattingVisitor(CodeFormattingContext context)
    {
      ITreeNode node = context.FirstNode;
      IPsiSourceFile projectFile = node.GetSourceFile();
      if (projectFile != null)
      {
        this.myIsGenerated = !Equals(projectFile.PrimaryPsiLanguage, node.Language);
      }
    }

    public override IEnumerable<string> VisitSecretFile(ISecretFile psiFile, FormattingStageContext context)
    {
      if (!this.myIsGenerated)
      {
        return base.VisitSecretFile(psiFile, context);
      }

      return base.VisitSecretFile(psiFile, context);
    }

    /*public override IEnumerable<string> VisitRuleDeclaration(IRuleDeclaration ruleDeclarationParam, FormattingStageContext context)
    {
      if (context.LeftChild is IModifier)
      {
        return new[] { " " };
      }
      if (context.RightChild is IRoleGetterParameter)
      {
        return new[] { " " };
      }
      if (context.RightChild is IRuleBracketTypedParameters)
      {
        return new[] { " " };
      }

      return new[] { "\r\n" };
    }

    public override IEnumerable<string> VisitRuleBody(IRuleBody ruleBodyParam, FormattingStageContext context)
    {
      return new[] { " " };
    }

    public override IEnumerable<string> VisitExtrasDefinition(IExtrasDefinition extrasDefinitionParam, FormattingStageContext context)
    {
      return new[] { "\r\n" };
    }

    public override IEnumerable<string> VisitSequence(ISequence sequenceParam, FormattingStageContext context)
    {
      return new[] { "\r\n" };
    }

    public override IEnumerable<string> VisitExtraDefinition(IExtraDefinition extraDefinitionParam, FormattingStageContext context)
    {
      return new[] { " " };
    }

    public override IEnumerable<string> VisitOptionsDefinition(IOptionsDefinition optionsDefinitionParam, FormattingStageContext context)
    {
      return new[] { "\r\n" };
    }

    public override IEnumerable<string> VisitPsiExpression(IPsiExpression psiExpressionParam, FormattingStageContext context)
    {
      if (context.RightChild is IChoiceTail)
      {
        return new[] { "\r\n" };
      }
      return new[] { " " };
    }

    public override IEnumerable<string> VisitParenExpression(IParenExpression parenExpressionParam, FormattingStageContext context)
    {
      if ((context.LeftChild is IPsiExpression) || (context.RightChild is IPsiExpression))
      {
        return new[] { "\r\n" };
      }
      return new[] { " " };
    }

    public override IEnumerable<string> VisitChoiceTail(IChoiceTail choiceTailParam, FormattingStageContext context)
    {
      if (context.LeftChild is ICommentNode)
      {
        return new[] { "\r\n" };
      }
      return new[] { " " };
    }*/
  }
}
