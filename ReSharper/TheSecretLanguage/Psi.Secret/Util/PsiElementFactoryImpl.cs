using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Util
{
  public class PsiElementFactoryImpl : PsiElementFactory
  {
    private readonly SecretLanguageService myLanguageService;
    private readonly IPsiModule myModule;

    public PsiElementFactoryImpl([NotNull] IPsiModule module)
      : this(module, module.GetSolution())
    {
    }

    private PsiElementFactoryImpl([NotNull] IPsiModule module, [NotNull] ISolution solution)
    {
      this.myModule = module;
      this.Solution = solution;
      this.Solution.GetPsiServices();
      this.myLanguageService = (SecretLanguageService)SecretLanguage.Instance.LanguageService();
    }

    private SecretParser CreateParser(string text)
    {
        return (SecretParser)this.myLanguageService.CreateParser(this.myLanguageService.GetPrimaryLexerFactory().CreateLexer(new StringBuffer(text)), null, null);
    }

    /*public override IRuleName CreateIdentifierExpression(string name)
    {
      var expression = (IRuleName)this.CreateExpression("$0", name);
      return expression;
    }

    public override IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters = false)
    {
      string braceParameters = "";
      if(hasBraceParameters)
      {
        braceParameters = " {ROLE, getter} ";
      }
      var node = this.CreateParser(name + braceParameters + "\n" + ":" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name + braceParameters + "\n" + ":" + "\n" + ";"));
      }
      SandBox.CreateSandBoxFor(node, this.myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name));
    }

    public override IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters, IList<Pair<string, string>> variableParameters)
    {
      if(variableParameters.Count == 0)
      {
        return this.CreateRuleDeclaration(name, hasBraceParameters);
      }

      string braceParameters = "";
      if (hasBraceParameters)
      {
        braceParameters = " {ROLE, getter} ";
      }

      string variableParametersString = " [";
      foreach (var variableParameter in variableParameters)
      {
        variableParametersString = variableParametersString + variableParameter.Second + " " + variableParameter.First + ",";
      }
      variableParametersString = variableParametersString.Substring(0, variableParametersString.Length - 1) + "]";

      var node = this.CreateParser(name + braceParameters + variableParametersString + "\n" + ":" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name + braceParameters + variableParametersString + "\n" + ":" + "\n" + ";"));
      }
      SandBox.CreateSandBoxFor(node, this.myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name));
    }

    private ITreeNode CreateExpression(string format, string name)
    {
      var node = this.CreateParser(name + "\n" + ":" + name + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
      }
      SandBox.CreateSandBoxFor(node, this.myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        IRuleBody ruleBody = ruleDeclaration.Body;
        ITreeNode child = ruleBody.FirstChild;
        while (child != null && ! (child is IPsiExpression))
        {
          child = child.NextSibling;
        }
        while (child != null && !(child is IRuleName))
        {
          child = child.FirstChild;
        }
        if (child != null)
        {
          return child;
        }
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'" + name, format));
    }*/
  }
}
