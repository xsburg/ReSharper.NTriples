// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ContextErrorHighlighterProcess.cs
// </summary>
// ***********************************************************************

using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Secret.CodeInspections.Highlightings;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
    internal class ContextErrorHighlighterProcess : SecretIncrementalDaemonStageProcessBase
    {
        public ContextErrorHighlighterProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, settingsStore)
        {
        }

        /*public override void VisitRuleDeclaredName(IRuleDeclaredName ruleDeclaredName, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = ruleDeclaredName.GetDocumentRange();
      AddHighLighting(colorConstantRange, ruleDeclaredName, consumer, new PsiRuleHighlighting(ruleDeclaredName));
      base.VisitRuleDeclaredName(ruleDeclaredName, consumer);
    }

    public override void VisitRuleName(IRuleName ruleNameParam, IHighlightingConsumer consumer)
    {
      var ruleName = ruleNameParam as RuleName;
      if (ruleName != null)
      {
        DocumentRange colorConstantRange = ruleName.GetDocumentRange();

        ResolveResultWithInfo resolve = ruleName.RuleNameReference.Resolve();

        bool isRuleResolved = resolve.Result.DeclaredElement != null || (resolve.Result.Candidates.Count > 0);
        if (isRuleResolved)
        {
          AddHighLighting(colorConstantRange, ruleName, consumer, new PsiRuleHighlighting(ruleName));
        }
        else
        {
          AddHighLighting(colorConstantRange, ruleName, consumer, new PsiUnresolvedRuleReferenceHighlighting(ruleName));
        }

        base.VisitRuleName(ruleName, consumer);
      }
    }

    public override void VisitVariableName(IVariableName variableNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = variableNameParam.GetDocumentRange();
      var variableName = variableNameParam as VariableName;
      if (variableName != null)
      {
        ResolveResultWithInfo resolve = variableName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          AddHighLighting(colorConstantRange, variableNameParam, consumer, new PsiVariableHighlighting(variableNameParam));
        }
        else
        {
          AddHighLighting(colorConstantRange, variableNameParam, consumer, new PsiUnresolvedVariableReferenceHighlighting(variableName));
        }
      }
    }

    public override void VisitPathName(IPathName pathNameParam, IHighlightingConsumer consumer)
    {
      DocumentRange colorConstantRange = pathNameParam.GetDocumentRange();
      var pathName = pathNameParam as PathName;
      if (pathName != null)
      {
        ResolveResultWithInfo resolve = pathName.Resolve();
        if ((resolve != null) && ((resolve.Result.DeclaredElement != null) || (resolve.Result.Candidates.Count > 0)))
        {
          AddHighLighting(colorConstantRange, pathNameParam, consumer, new PsiRuleHighlighting(pathNameParam));
        }
        else
        {
          AddHighLighting(colorConstantRange, pathNameParam, consumer, new PsiUnresolvedPathReferenceHighlighting(pathName));
        }
      }
    }*/

        public override void VisitPrefix(IPrefix prefixParam, IHighlightingConsumer consumer)
        {
            DocumentRange range = prefixParam.GetDocumentRange();
            var prefix = prefixParam as Prefix;
            if (prefix != null)
            {
                ResolveResultWithInfo resolve = prefix.Resolve();
                if (resolve == null ||
                    resolve.Result.DeclaredElement is UnresolvedNamespacePrefixDeclaredElement ||
                    ((resolve.Result.DeclaredElement == null) && (resolve.Result.Candidates.Count == 0)))
                {
                    this.AddHighLighting(
                        range,
                        prefixParam,
                        consumer,
                        new SecretUnresolvedReferenceHighlighting<SecretPrefixReference>(
                            prefix, prefix.PrefixReference, string.Format("Unresolved prefix '{0}'", prefix.GetText())));
                }
            }
        }

        private void AddHighLighting(
            DocumentRange range, ITreeNode element, IHighlightingConsumer consumer, IHighlighting highlighting)
        {
            var info = new HighlightingInfo(range, highlighting, new Severity?());
            IFile file = element.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(info.Highlighting, file);
            }
        }
    }
}
