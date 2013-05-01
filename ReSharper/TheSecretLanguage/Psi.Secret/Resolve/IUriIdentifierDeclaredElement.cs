using JetBrains.ReSharper.Psi.Secret.Cache;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public interface IUriIdentifierDeclaredElement : IDeclaredElement
    {
        string GetNamespace();
        string GetLocalName();
        string GetUri();

        UriIdentifierKind GetKind();
    }
}