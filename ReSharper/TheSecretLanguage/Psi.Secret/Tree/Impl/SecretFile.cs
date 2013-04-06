namespace JetBrains.ReSharper.Psi.Secret.Tree.Impl
{
    internal partial class SecretFile
    {
        public override PsiLanguageType Language
        {
            get { return SecretLanguage.Instance; }
        }
    }
}