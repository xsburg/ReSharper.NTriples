// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   StatementsSimplificationQuickFix.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Intentions.QuickFix
{
    [QuickFix]
    public class StatementsSimplificationQuickFix : ContextActionBase, IQuickFix
    {
        private readonly HintRangeHighlighting<ISentence> highlighter;

        public StatementsSimplificationQuickFix(HintRangeHighlighting<ISentence> highlighter)
        {
            this.highlighter = highlighter;
        }

        public override string Text
        {
            get
            {
                return "Simplify statements";
            }
        }

        /*public void CreateBulbItems(BulbMenu menu, Severity severity)
        {
            menu.ArrangeQuickFix(this, severity);
        }*/

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return this.highlighter.IsValid();
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var startSentence = this.highlighter.StartElement;
            var sentences = this.CollectSentences();
            var facts =
                sentences.SelectMany(s => s.Statement.FactsEnumerable)
                         .GroupBy(GetPredicateText)
                         .ToDictionary(g => g.Key, g => g.SelectMany(x => x.ObjectsEnumerable).Select(o => o.GetText()).ToArray());
            var subjectText = startSentence.Statement.Subject.GetText();
            var newSentence = NTriplesElementFactory.GetInstance(startSentence).CreateSentence(subjectText, facts);

            ModificationUtil.DeleteChildRange(this.highlighter.StartElement, this.highlighter.EndElement.PrevSibling);
            ModificationUtil.ReplaceChild(sentences.Last(), newSentence);

            return null;
        }

        private static string GetPredicateText(IFact f)
        {
            return f.Predicate != null
                       ? f.Predicate.GetText()
                       : "";
        }

        private List<ISentence> CollectSentences()
        {
            var sentences = new List<ISentence>();
            var sentence = this.highlighter.StartElement;
            while (sentence != this.highlighter.EndElement && sentence != null)
            {
                sentences.Add(sentence);

                var nextSibling = sentence.NextSibling;
                while (!(nextSibling is ISentence) && nextSibling != null)
                {
                    nextSibling = nextSibling.NextSibling;
                }

                sentence = nextSibling as ISentence;
            }

            sentences.Add(sentence);
            return sentences;
        }
    }
}
