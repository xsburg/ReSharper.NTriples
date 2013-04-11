using System;
using JetBrains.ReSharper.Daemon;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
  public class SmartResolverProcess : IDaemonStageProcess
  {
    private readonly IDaemonProcess myDaemonProcess;

    public SmartResolverProcess(IDaemonProcess daemonProcess)
    {
      this.myDaemonProcess = daemonProcess;
    }

    #region IDaemonStageProcess Members

    public IDaemonProcess DaemonProcess
    {
      get { return this.myDaemonProcess; }
    }

    public void Execute(Action<DaemonStageResult> commiter)
    {
    }

    #endregion
  }
}
