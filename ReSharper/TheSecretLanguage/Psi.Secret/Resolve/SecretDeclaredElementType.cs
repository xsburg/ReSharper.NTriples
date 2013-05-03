// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretDeclaredElementType.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Secret.Resources;
using JetBrains.ReSharper.Psi.Secret.Services;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public class SecretDeclaredElementType : DeclaredElementType
    {
        public static readonly SecretDeclaredElementType Prefix = new SecretDeclaredElementType(
            "Prefix", N3PluginSymbolThemedIcons.AccordionDisable.Id);

        public static readonly SecretDeclaredElementType UriIdentifier = new SecretDeclaredElementType(
            "UriIdentifier", N3PluginSymbolThemedIcons.AccordionDisable.Id);

        private static readonly PsiLanguageType Language = SecretLanguage.Instance;

        private readonly IDeclaredElementPresenter myElementPresenter;
        private readonly IconId myIconId;

        private SecretDeclaredElementType(string name, IconId iconId)
            : base(name)
        {
            this.myElementPresenter = new SecretDeclaredElementPresenter();
            this.myIconId = iconId;
        }

        public override string PresentableName
        {
            get
            {
                return "Secret";
            }
        }

        protected override IDeclaredElementPresenter DefaultPresenter
        {
            get
            {
                return this.myElementPresenter;
            }
        }

        public override bool IsPresentable(PsiLanguageType language)
        {
            return Equals(Language, language);
        }

        protected override IconId GetImage()
        {
            return this.myIconId;
        }
    }
}
