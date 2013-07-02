// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   FactsSimplificationQuickFix.cs
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
    public class FactsSimplificationQuickFix : ContextActionBase, IQuickFix
    {
        private readonly HintRangeHighlighting<IFact> highlighter;

        public FactsSimplificationQuickFix(HintRangeHighlighting<IFact> highlighter)
        {
            this.highlighter = highlighter;
        }

        public override string Text
        {
            get
            {
                return "Simplify facts";
            }
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return this.highlighter.IsValid();
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var startFact = this.highlighter.StartElement;
            var factElements = this.CollectFacts();
            var facts = factElements.GroupBy(GetPredicateText)
                                    .ToDictionary(
                                        g => g.Key, g => g.SelectMany(x => x.ObjectsEnumerable).Select(o => o.GetText()).ToArray());
            var statement = startFact.GetContainingNode<IStatement>();
            if (statement == null)
            {
                return null;
            }

            var subjectText = statement.Subject.GetText();
            var newSentence = NTriplesElementFactory.GetInstance(startFact).CreateSentence(subjectText, facts);

            ModificationUtil.ReplaceChild(statement, newSentence);
            return null;
        }

        private static string GetPredicateText(IFact f)
        {
            return f.Predicate != null
                       ? f.Predicate.GetText()
                       : "";
        }

        private List<IFact> CollectFacts()
        {
            var facts = new List<IFact>();
            var fact = this.highlighter.StartElement;
            while (fact != this.highlighter.EndElement && fact != null)
            {
                facts.Add(fact);

                var nextSibling = fact.NextSibling;
                while (!(nextSibling is IFact) && nextSibling != null)
                {
                    nextSibling = nextSibling.NextSibling;
                }

                fact = nextSibling as IFact;
            }

            facts.Add(fact);
            return facts;
        }
    }
}
