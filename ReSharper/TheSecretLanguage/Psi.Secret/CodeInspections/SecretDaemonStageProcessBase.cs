using System;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
    public abstract class SecretDaemonStageProcessBase : 
        TreeNodeVisitor<IHighlightingConsumer>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
    {
        private readonly IDaemonProcess myDaemonProcess;
        private readonly ISecretFile myFile;

        private readonly IContextBoundSettingsStore mySettingsStore;

        protected SecretDaemonStageProcessBase(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
        {
            this.myDaemonProcess = process;
            this.mySettingsStore = settingsStore;
            this.myFile = SecretDaemonStageBase.GetSecretFile(this.myDaemonProcess.SourceFile);
        }

        protected ISecretFile File
        {
            get { return this.myFile; }
        }

        public IDaemonProcess DaemonProcess
        {
            get { return this.myDaemonProcess; }
        }

        public abstract void Execute(Action<DaemonStageResult> commiter);

        public virtual bool InteriorShouldBeProcessed(ITreeNode element, IHighlightingConsumer context)
        {
            return true;
        }

        public bool IsProcessingFinished(IHighlightingConsumer context)
        {
            if (this.myDaemonProcess.InterruptFlag)
            {
                throw new ProcessCancelledException();
            }
            return false;
        }

        public virtual void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
        {
        }

        public virtual void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
        {
            var secretElement = element as ISecretTreeNode;
            if (secretElement != null)
            {
                var tokenNode = secretElement as ITokenNode;
                if (tokenNode == null || !tokenNode.GetTokenType().IsWhitespace)
                {
                    secretElement.Accept(this, consumer);
                }
            }
            else
            {
                this.VisitNode(element, consumer);
            }
        }

        protected void HighlightInFile(Action<ISecretFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
        {
            var consumer = new DefaultHighlightingConsumer(this, this.mySettingsStore);
            fileHighlighter(this.File, consumer);
            commiter(new DaemonStageResult(consumer.Highlightings));
        }
    }
}