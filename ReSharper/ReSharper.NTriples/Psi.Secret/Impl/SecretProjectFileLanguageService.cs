// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiProjectFileLanguageService.cs
// </summary>
// ***********************************************************************

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.UI.Icons;
using ReSharper.NTriples.Resources;
using ReSharper.NTriples.Resources.N3PluginSymbolIcons;

namespace ReSharper.NTriples.Impl
{
    [ProjectFileType(typeof(SecretProjectFileType))]
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
                return N3PluginSymbolThemedIcons.N3Icon.Id;
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
