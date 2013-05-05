using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.CodeInspections
{
    internal class SuggestionHighlighterProcess : SecretIncrementalDaemonStageProcessBase
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
            const string message = "Statement can be simplified";
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
                            this.AddSuggestionHighlighting(consumer, message, startSentence, sentence);
                            startSentence = null;
                        }
                    }

                    lastSentence = sentence;
                    lastText = text;
                }
            }

            if (startSentence != null)
            {
                this.AddSuggestionHighlighting(consumer, message, startSentence, lastSentence);
            }
        }

        private void AddSuggestionHighlighting(IHighlightingConsumer consumer, string message, ITreeNode startElement, ITreeNode endElement)
        {
            var highlighting = new SuggestionHighlighting(startElement, endElement, message);
            IFile file = startElement.GetContainingFile();
            if (file != null)
            {
                consumer.AddHighlighting(highlighting, file);
            }
        }
    }
}