using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.CodeInspections
{
    internal class SuggestionHighlighterProcess : NTriplesIncrementalDaemonStageProcessBase
    {
        public SuggestionHighlighterProcess(IDaemonProcess daemonProcess, IContextBoundSettingsStore settingsStore)
            : base(daemonProcess, settingsStore)
        {
        }

        public override void VisitSentences(ISentences sentencesParam, IHighlightingConsumer consumer)
        {
            string lastText = null;
            ISentence lastSentence = null;
            ISentence startSentence = null;
            const string message = "Statements can be simplified";
            foreach (var sentence in sentencesParam.SentenceListEnumerable)
            {
                string text;
                if (sentence.Statement != null && sentence.Statement.Subject != null &&
                    !string.IsNullOrEmpty(text = sentence.Statement.Subject.GetText()))
                {
                    if (text.Equals(lastText, StringComparison.Ordinal))
                    {
                        if (startSentence == null)
                        {
                            startSentence = lastSentence;
                        }
                    }
                    else
                    {
                        if (startSentence != null)
                        {
                            this.AddSentenceSuggestionHighlighting(consumer, message, startSentence, lastSentence);
                            startSentence = null;
                        }
                    }

                    lastSentence = sentence;
                    lastText = text;
                }
            }

            if (startSentence != null)
            {
                this.AddSentenceSuggestionHighlighting(consumer, message, startSentence, lastSentence);
            }
        }

        public override void VisitStatement(Tree.IStatement statementParam, IHighlightingConsumer consumer)
        {
            var canBeSimplified = statementParam.FactsEnumerable.GroupBy(x => x.Predicate.GetText()).Any(x => x.Count() >= 2);
            if (canBeSimplified)
            {
                AddFactsSuggestionHighlighting(
                    consumer,
                    "Facts can be simplified",
                    statementParam.FactsEnumerable.First(),
                    statementParam.FactsEnumerable.Last());
            }
        }

        private void AddSentenceSuggestionHighlighting(IHighlightingConsumer consumer, string message, ISentence startElement, ISentence endElement)
        {
            var highlighting = new SuggestionRangeHighlighting<ISentence>(startElement, endElement, message);
            IFile file = startElement.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(highlighting, file);
            }
        }

        private void AddFactsSuggestionHighlighting(IHighlightingConsumer consumer, string message, IFact startElement, IFact endElement)
        {
            var highlighting = new SuggestionRangeHighlighting<IFact>(startElement, endElement, message);
            IFile file = startElement.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(highlighting, file);
            }
        }
    }
}