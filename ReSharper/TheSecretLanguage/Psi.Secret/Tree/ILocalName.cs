using JetBrains.ReSharper.Psi.Secret.Resolve;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public partial interface ILocalName
    {
        SecretLocalNameReference LocalNameReference { get; }
        void SetName(string shortName);
    }
}