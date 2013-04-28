using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    /*public partial interface IUriIdentifier : IDeclaration//, IChameleonNode
    {
        void SetReferenceName(string shortName);
        string GetFullName(ISecretFile file);
    }*/
    public partial interface IUriIdentifier
    {
        string GetUri(ISecretFile file);
        UriIdentifierKind GetKind();
        IDeclaredElement DescendantDeclaredElement { get; }
    }
}