using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
  internal class InspectionsProcess : SecretDaemonStageProcessBase
  {
    private readonly IDictionary<string, List<IDeclaration>> myDeclarations;

    public InspectionsProcess(IDaemonProcess process, IContextBoundSettingsStore settings)
      : base(process, settings)
    {
      process.SourceFile.PrimaryPsiLanguage.Is<SecretLanguage>();
      process.GetStageProcess<PsiFileIndexProcess>();

      this.myDeclarations = new Dictionary<string, List<IDeclaration>>();
      //this.VisitFile(process.SourceFile.GetPsiFile<SecretLanguage>(new DocumentRange(process.SourceFile.Document, 0)) as ISecretFile);
    }

    public override void Execute(Action<DaemonStageResult> commiter)
    {
      this.HighlightInFile((file, consumer) => file.ProcessDescendants(this, consumer), commiter);
    }
  }
}
