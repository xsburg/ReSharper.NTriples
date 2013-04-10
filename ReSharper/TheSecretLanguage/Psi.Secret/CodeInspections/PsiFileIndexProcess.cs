using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
  public class PsiFileIndexProcess : SecretDaemonStageProcessBase
  {
    public PsiFileIndexProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore)
      : base(process, settingsStore)
    {
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      this.HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
    }
  }
}
