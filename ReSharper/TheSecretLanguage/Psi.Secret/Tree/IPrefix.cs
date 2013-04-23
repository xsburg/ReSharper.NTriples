using JetBrains.ReSharper.Psi.Secret.Resolve;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public partial interface IPrefix
    {
        SecretPrefixReference PrefixReference { get; }
        void SetName(string shortName);
    }
}