using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.PsiPlugin.CodeInspections.Psi;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
  public interface IPsiInspectionsProcessFactory
  {
    IDaemonStageProcess CreateInspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings);
  }

  [DaemonStage(StagesBefore = new[] { typeof (SmartResolverStage), typeof (PsiFileIndexStage) })]
  public class InspectionsStage : SecretDaemonStageBase
  {
    private readonly ProjectFileTypeServices myServices;

    public InspectionsStage(ProjectFileTypeServices services)
    {
      this.myServices = services;
    }

    public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
    {
      return this.IsSupported(sourceFile) ? ErrorStripeRequest.STRIPE_AND_ERRORS : ErrorStripeRequest.NONE;
    }

    public override IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
    {
      if (!this.IsSupported(process.SourceFile))
        return EmptyList<IDaemonStageProcess>.InstanceList;

      var factory = this.myServices.TryGetService<IPsiInspectionsProcessFactory>(process.SourceFile.LanguageType);
      if (factory == null)
        return EmptyList<IDaemonStageProcess>.InstanceList;

      return new List<IDaemonStageProcess> { factory.CreateInspectionsProcess(process, settings) };
    }
  }

  [ProjectFileType(typeof (KnownProjectFileType))]
  public sealed class PsiInspectionsProcessFactory : IPsiInspectionsProcessFactory
  {
    #region IPsiInspectionsProcessFactory Members

    public IDaemonStageProcess CreateInspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings)
    {
      return new InspectionsProcess(process, settings);
    }

    #endregion
  }
}
