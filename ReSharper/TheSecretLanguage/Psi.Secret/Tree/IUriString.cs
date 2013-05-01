using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public partial interface IUriString : IDeclaration//, IChameleonNode
    {
        void SetReferenceName(string shortName);
        string GetUri();
    }
}