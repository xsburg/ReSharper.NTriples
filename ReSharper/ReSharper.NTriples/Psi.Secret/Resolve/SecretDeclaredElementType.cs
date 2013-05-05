// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretDeclaredElementType.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Features.Altering.Resources;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Secret.Resources;
using JetBrains.ReSharper.Psi.Secret.Services;
using JetBrains.ReSharper.Psi.Xaml.Resources;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public class SecretDeclaredElementType : DeclaredElementType
    {
        // Full list of PSI symbols is located here: http://www.jetbrains.com/resharper/webhelp/Reference__Symbol_Icons.html
        public static readonly SecretDeclaredElementType Prefix = new SecretDeclaredElementType(
            "Prefix", PsiXamlThemedIcons.XamlNamespaceAlias.Id);

        public static readonly SecretDeclaredElementType UriIdentifier = new SecretDeclaredElementType(
            "UriIdentifier", PsiXamlThemedIcons.XamlPredefinedObjectElement.Id);

        public static readonly SecretDeclaredElementType PrefixUri = new SecretDeclaredElementType(
            "PrefixUri", PsiSymbolsThemedIcons.Namespace.Id);

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
