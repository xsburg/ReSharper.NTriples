// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretIndentingStage.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    public class SecretIndentingStage
    {
        private readonly bool myInTypingAssist;
        private static SecretIndentVisitor ourIndentVisitor;

        private SecretIndentingStage(bool inTypingAssist = false)
        {
            this.myInTypingAssist = inTypingAssist;
        }

        public static void DoIndent(
            CodeFormattingContext context, GlobalFormatSettings formatSettings, IProgressIndicator progress, bool inTypingAssist)
        {
            var indentCache = new SecretIndentCache(
                context.CodeFormatter, null, AlignmentTabFillStyle.OPTIMAL_FILL, formatSettings);

            ourIndentVisitor = CreateIndentVisitor(indentCache, inTypingAssist);
            var stage = new SecretIndentingStage(inTypingAssist);
            List<FormattingRange> nodePairs =
                context.SequentialEnumNodes().Where(p => context.CanModifyInsideNodeRange(p.First, p.Last)).ToList();
            //List<FormattingRange> nodePairs = GetNodePairs(context).ToList();
            IEnumerable<FormatResult<string>> indents = nodePairs.
                Select(range => new FormatResult<string>(range, stage.CalcIndent(new FormattingStageContext(range)))).
                Where(res => res.ResultValue != null);

            FormatterImplHelper.ForeachResult(
                indents,
                progress,
                res => res.Range.Last.MakeIndent(res.ResultValue));
        }

        [NotNull]
        private static SecretIndentVisitor CreateIndentVisitor([NotNull] SecretIndentCache indentCache, bool inTypingAssist)
        {
            return new SecretIndentVisitor(indentCache, inTypingAssist);
        }

        private string CalcIndent(FormattingStageContext context)
        {
            CompositeElement parent = context.Parent;

            ITreeNode rChild = context.RightChild;
            if ((!context.LeftChild.HasLineFeedsTo(rChild)) && (!this.myInTypingAssist))
            {
                return null;
            }

            var psiTreeNode = context.Parent as INTriplesTreeNode;

            return psiTreeNode != null
                       ? psiTreeNode.Accept(ourIndentVisitor, context)
                       : ourIndentVisitor.VisitNode(parent, context);
        }
    }
}
