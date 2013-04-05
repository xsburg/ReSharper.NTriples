using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public abstract class SecretCompositeNodeType : CompositeNodeType
    {
        protected SecretCompositeNodeType(string s)
            : base(s)
        {
        }
    }
}