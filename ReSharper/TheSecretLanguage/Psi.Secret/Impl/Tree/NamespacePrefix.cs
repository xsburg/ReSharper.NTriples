using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class NamespacePrefix
    {
        private SecretNamespacePrefixReference myNamespacePrefixReference;

        public SecretNamespacePrefixReference RuleNameReference
        {
            get
            {
                lock (this)
                {
                    return this.myNamespacePrefixReference ?? (this.myNamespacePrefixReference = new SecretNamespacePrefixReference(this));
                }
            }
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.RuleNameReference);
        }

        public void SetName(string shortName)
        {
            this.RuleNameReference.SetName(shortName);
        }
    }
}