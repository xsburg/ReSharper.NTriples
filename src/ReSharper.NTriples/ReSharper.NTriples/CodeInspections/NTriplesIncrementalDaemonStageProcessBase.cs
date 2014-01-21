// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesIncrementalDaemonStageProcessBase.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.Application.Settings;
using JetBrains.Application.Threading;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace ReSharper.NTriples.CodeInspections
{
    /// <summary>
    ///     Base class for daemon stages which can incrementally re-highlight changed function only
    /// </summary>
    public abstract class NTriplesIncrementalDaemonStageProcessBase : NTriplesDaemonStageProcessBase
    {
        private readonly IContextBoundSettingsStore mySettingsStore;

        protected NTriplesIncrementalDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
            : base(process, settingsStore)
        {
            this.mySettingsStore = settingsStore;
        }


        public override void Execute(Action<DaemonStageResult> commiter)
        {
            Action globalHighlighter = () =>
            {
                var consumer = new DefaultHighlightingConsumer(this, this.mySettingsStore);
                this.File.ProcessThisAndDescendants(new GlobalProcessor(this, consumer));
                commiter(
                    new DaemonStageResult(consumer.Highlightings)
                        {
                            Layer = 1
                        });
            };

            using (var fibers = this.DaemonProcess.CreateFibers())
            {
                fibers.EnqueueJob(globalHighlighter);
            }

            commiter(new DaemonStageResult(EmptyArray<HighlightingInfo>.Instance));
        }

        private class GlobalProcessor : ProcessorBase
        {
            public GlobalProcessor(NTriplesDaemonStageProcessBase process, IHighlightingConsumer consumer)
                : base(process, consumer)
            {
            }
        }

        private class ProcessorBase : IRecursiveElementProcessor
        {
            private readonly IHighlightingConsumer myConsumer;
            private readonly NTriplesDaemonStageProcessBase myProcess;

            protected ProcessorBase(NTriplesDaemonStageProcessBase process, IHighlightingConsumer consumer)
            {
                this.myProcess = process;
                this.myConsumer = consumer;
            }

            public bool ProcessingIsFinished
            {
                get
                {
                    return this.myProcess.IsProcessingFinished(this.myConsumer);
                }
            }

            public virtual bool InteriorShouldBeProcessed(ITreeNode element)
            {
                return this.myProcess.InteriorShouldBeProcessed(element, this.myConsumer);
            }

            public virtual void ProcessAfterInterior(ITreeNode element)
            {
                this.myProcess.ProcessAfterInterior(element, this.myConsumer);
            }

            public virtual void ProcessBeforeInterior(ITreeNode element)
            {
                this.myProcess.ProcessBeforeInterior(element, this.myConsumer);
            }
        }
    }
}
