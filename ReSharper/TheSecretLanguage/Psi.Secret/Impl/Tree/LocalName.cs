using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class LocalName
    {
        private SecretLocalNameReference myLocalNameReference;

        public SecretLocalNameReference LocalNameReference
        {
            get
            {
                lock (this)
                {
                    return this.myLocalNameReference ?? (this.myLocalNameReference = new SecretLocalNameReference(this));
                }
            }
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.LocalNameReference);
        }

        public void SetName(string shortName)
        {
            this.LocalNameReference.SetName(shortName);
        }
    }
}