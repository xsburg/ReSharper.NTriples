using JetBrains.ReSharper.Psi.Secret.Resolve;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public partial interface INamespacePrefix
    {
        SecretNamespacePrefixReference RuleNameReference { get; }
        void SetName(string shortName);
    }
}