using System;
using JetBrains.Application.Settings;
using JetBrains.Application.Threading;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
    /// <summary>
    ///   Base class for daemon stages which can incrementally re-highlight changed function only
    /// </summary>
    public abstract class SecretIncrementalDaemonStageProcessBase : SecretDaemonStageProcessBase
    {
        private readonly IContextBoundSettingsStore mySettingsStore;

        protected SecretIncrementalDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
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
                commiter(new DaemonStageResult(consumer.Highlightings) { Layer = 1 });
            };

            using (IMultiCoreFibers fibers = this.DaemonProcess.CreateFibers())
            {
                // highlgiht global space
                //if (DaemonProcess.FullRehighlightingRequired)
                fibers.EnqueueJob(globalHighlighter);
            }

            // remove all old highlightings
            //if (DaemonProcess.FullRehighlightingRequired)
            commiter(new DaemonStageResult(EmptyArray<HighlightingInfo>.Instance));
        }

        #region Nested type: GlobalProcessor

        private class GlobalProcessor : ProcessorBase
        {
            public GlobalProcessor(SecretDaemonStageProcessBase process, IHighlightingConsumer consumer)
                : base(process, consumer)
            {
            }
        }

        #endregion

        #region Nested type: ProcessorBase

        private class ProcessorBase : IRecursiveElementProcessor
        {
            private readonly IHighlightingConsumer myConsumer;
            private readonly SecretDaemonStageProcessBase myProcess;

            protected ProcessorBase(SecretDaemonStageProcessBase process, IHighlightingConsumer consumer)
            {
                this.myProcess = process;
                this.myConsumer = consumer;
            }

            #region IRecursiveElementProcessor Members

            public bool ProcessingIsFinished
            {
                get { return this.myProcess.IsProcessingFinished(this.myConsumer); }
            }

            public virtual void ProcessBeforeInterior(ITreeNode element)
            {
                this.myProcess.ProcessBeforeInterior(element, this.myConsumer);
            }

            public virtual void ProcessAfterInterior(ITreeNode element)
            {
                this.myProcess.ProcessAfterInterior(element, this.myConsumer);
            }

            public virtual bool InteriorShouldBeProcessed(ITreeNode element)
            {
                return this.myProcess.InteriorShouldBeProcessed(element, this.myConsumer);
            }

            #endregion
        }

        #endregion
    }
}