using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class Prefix
    {
        private SecretPrefixReference myPrefixReference;

        public SecretPrefixReference PrefixReference
        {
            get
            {
                lock (this)
                {
                    return this.myPrefixReference ?? (this.myPrefixReference = new SecretPrefixReference(this));
                }
            }
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.PrefixReference);
        }

        public void SetName(string shortName)
        {
            this.PrefixReference.SetName(shortName);
        }

        public ResolveResultWithInfo Resolve()
        {
            return PrefixReference.Resolve();
        }
    }
}