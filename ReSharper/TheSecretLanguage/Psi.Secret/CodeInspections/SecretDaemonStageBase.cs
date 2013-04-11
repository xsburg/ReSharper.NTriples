using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.Secret.Tree;

namespace JetBrains.ReSharper.Psi.Secret.CodeInspections
{
    public abstract class SecretDaemonStageBase : IDaemonStage
    {
        public abstract IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind);

        public virtual ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        {
            if (!this.IsSupported(sourceFile))
            {
                return ErrorStripeRequest.NONE;
            }

            var properties = sourceFile.Properties;
            if (!properties.ProvidesCodeModel || properties.IsNonUserFile)
            {
                return ErrorStripeRequest.NONE;
            }

            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        [CanBeNull]
        public static ISecretFile GetSecretFile(IPsiSourceFile sourceFile)
        {
            PsiManager manager = PsiManager.GetInstance(sourceFile.GetSolution());
            manager.AssertAllDocumentAreCommited();
            return manager.GetPsiFile<SecretLanguage>(new DocumentRange(sourceFile.Document, 0)) as ISecretFile;
        }

        protected bool IsSupported(IPsiSourceFile sourceFile)
        {
            if (sourceFile == null || !sourceFile.IsValid())
            {
                return false;
            }

            ISecretFile file = GetSecretFile(sourceFile);
            return file != null && file.Language.Is<SecretLanguage>();
        }
    }
}