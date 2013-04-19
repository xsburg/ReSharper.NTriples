using JetBrains.ReSharper.Psi.Secret.Resources;
using JetBrains.ReSharper.Psi.Secret.Services;
using JetBrains.UI.Icons;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public class SecretDeclaredElementType : DeclaredElementType
    {
        private static readonly PsiLanguageType Language = SecretLanguage.Instance;

        public static readonly SecretDeclaredElementType NamespacePrefix = new SecretDeclaredElementType("NamespacePrefix", N3PluginSymbolThemedIcons.AccordionDisable.Id);
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
            get { return "Secret"; }
        }

        protected override IDeclaredElementPresenter DefaultPresenter
        {
            get { return this.myElementPresenter; }
        }

        protected override IconId GetImage()
        {
            return this.myIconId;
        }

        public override bool IsPresentable(PsiLanguageType language)
        {
            return Equals(Language, language);
        }
    }
}