namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    internal interface IUriIdentifierDeclaredElement
    {
        string GetNamespace();
        string GetLocalName();
    }
}