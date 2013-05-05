// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   ReformatCode.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCleanup;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    [CodeCleanupModule]
    public class ReformatCode : ICodeCleanupModule
    {
        private static readonly Descriptor OurDescriptor = new Descriptor();

        public ICollection<CodeCleanupOptionDescriptor> Descriptors
        {
            get
            {
                return new CodeCleanupOptionDescriptor[] { OurDescriptor };
            }
        }

        public bool IsAvailableOnSelection
        {
            get
            {
                return true;
            }
        }

        public PsiLanguageType LanguageType
        {
            get
            {
                return SecretLanguage.Instance;
            }
        }

        public bool IsAvailable(IPsiSourceFile sourceFile)
        {
            return sourceFile.IsLanguageSupported<SecretLanguage>();
        }

        public void Process(
            IPsiSourceFile sourceFile,
            IRangeMarker rangeMarkerMarker,
            CodeCleanupProfile profile,
            IProgressIndicator progressIndicator)
        {
            ISolution solution = sourceFile.GetSolution();
            if (!profile.GetSetting(OurDescriptor))
            {
                return;
            }

            ISecretFile[] files = sourceFile.GetPsiFiles<SecretLanguage>().Cast<ISecretFile>().ToArray();
            using (progressIndicator.SafeTotal("Reformat Psi", files.Length))
            {
                foreach (ISecretFile file in files)
                {
                    using (IProgressIndicator pi = progressIndicator.CreateSubProgress(1))
                    {
                        using (WriteLockCookie.Create())
                        {
                            var languageService = file.Language.LanguageService();
                            Assertion.Assert(languageService != null, "languageService != null");
                            var formatter = languageService.CodeFormatter;
                            Assertion.Assert(formatter != null, "formatter != null");

                            PsiManager.GetInstance(sourceFile.GetSolution()).DoTransaction(
                                delegate
                                {
                                    if (rangeMarkerMarker != null && rangeMarkerMarker.IsValid)
                                    {
                                        formatter.Format(
                                            solution,
                                            rangeMarkerMarker.DocumentRange,
                                            CodeFormatProfile.DEFAULT,
                                            true,
                                            false,
                                            pi);
                                    }
                                    else
                                    {
                                        formatter.FormatFile(
                                            file,
                                            CodeFormatProfile.DEFAULT,
                                            pi);
                                    }
                                },
                                "Code cleanup");
                        }
                    }
                }
            }
        }

        public void SetDefaultSetting(CodeCleanupProfile profile, CodeCleanup.DefaultProfileType profileType)
        {
            switch (profileType)
            {
            case CodeCleanup.DefaultProfileType.FULL:
            case CodeCleanup.DefaultProfileType.REFORMAT:
                profile.SetSetting(OurDescriptor, true);
                break;
            default:
                throw new ArgumentOutOfRangeException("profileType");
            }
        }

        [DisplayName("Reformat code")]
        [DefaultValue(false)]
        [Category("Psi")]
        private class Descriptor : CodeCleanupBoolOptionDescriptor
        {
            public Descriptor()
                : base("PsiReformatCode")
            {
            }
        }
    }
}
