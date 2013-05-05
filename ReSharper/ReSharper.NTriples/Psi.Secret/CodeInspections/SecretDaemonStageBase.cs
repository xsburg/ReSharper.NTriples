// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretDaemonStageBase.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.CodeInspections
{
    public abstract class SecretDaemonStageBase : IDaemonStage
    {
        [CanBeNull]
        public static ISecretFile GetSecretFile(IPsiSourceFile sourceFile)
        {
            PsiManager manager = PsiManager.GetInstance(sourceFile.GetSolution());
            manager.AssertAllDocumentAreCommited();
            return manager.GetPsiFile<SecretLanguage>(new DocumentRange(sourceFile.Document, 0)) as ISecretFile;
        }

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
