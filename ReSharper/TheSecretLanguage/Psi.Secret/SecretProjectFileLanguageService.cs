// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiProjectFileLanguageService.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Resources;
using JetBrains.Text;
using JetBrains.UI.Icons;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret
{
    //TODO:[ProjectFileType(typeof(SecretProjectFileType))]
    public class SecretProjectFileLanguageService : ProjectFileLanguageService
    {
        public SecretProjectFileLanguageService(SecretProjectFileType projectFileType)
            : base(projectFileType)
        {
        }

        public override IconId Icon
        {
            get
            {
                return N3PluginSymbolThemedIcons.AccordionDisable.Id;
            }
        }

        protected override PsiLanguageType PsiLanguageType
        {
            get
            {
                return SecretLanguage.Instance;
            }
        }

        public override ILexerFactory GetMixedLexerFactory(ISolution solution, IBuffer buffer, IPsiSourceFile sourceFile = null)
        {
            {
                return new SecretLexerFactory();
            }
        }
    }
}
