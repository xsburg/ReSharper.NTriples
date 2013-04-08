namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class SecretFile
    {
        public override PsiLanguageType Language
        {
            get { return SecretLanguage.Instance; }
        }
    }
}