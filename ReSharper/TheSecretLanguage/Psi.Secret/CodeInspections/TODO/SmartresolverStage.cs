using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
  [DaemonStage(StagesBefore = new[] { typeof (GlobalFileStructureCollectorStage) }, StagesAfter = new[] { typeof (IdentifierHighlightingStage) })]
  public class SmartResolverStage : SecretDaemonStageBase
  {
    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return ErrorStripeRequest.NONE;
    }

    public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!this.IsSupported(process.SourceFile))
      {
        return EmptyList<IDaemonStageProcess>.InstanceList;
      }
      return new List<IDaemonStageProcess> { new SmartResolverProcess(process) };
    }
  }
}
