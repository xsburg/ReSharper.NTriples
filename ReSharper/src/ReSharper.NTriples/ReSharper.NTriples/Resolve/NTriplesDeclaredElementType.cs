// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesDeclaredElementType.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Xaml.Resources;
using JetBrains.UI.Icons;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Services;

namespace ReSharper.NTriples.Resolve
{
    public class NTriplesDeclaredElementType : DeclaredElementType
    {
        // Full list of PSI symbols is located here: http://www.jetbrains.com/resharper/webhelp/Reference__Symbol_Icons.html
        public static readonly NTriplesDeclaredElementType Prefix = new NTriplesDeclaredElementType(
            "Prefix", PsiXamlThemedIcons.XamlNamespaceAlias.Id);

        public static readonly NTriplesDeclaredElementType PrefixUri = new NTriplesDeclaredElementType(
            "PrefixUri", PsiSymbolsThemedIcons.Namespace.Id);

        public static readonly NTriplesDeclaredElementType UriIdentifier = new NTriplesDeclaredElementType(
            "UriIdentifier", PsiXamlThemedIcons.XamlPredefinedObjectElement.Id);

        private static readonly PsiLanguageType Language = NTriplesLanguage.Instance;

        private readonly IDeclaredElementPresenter myElementPresenter;
        private readonly IconId myIconId;

        private NTriplesDeclaredElementType(string name, IconId iconId)
            : base(name)
        {
            this.myElementPresenter = new NTriplesDeclaredElementPresenter();
            this.myIconId = iconId;
        }

        public override string PresentableName
        {
            get
            {
                return "N-Triples";
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
