// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   PrefixUri.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class PrefixUri
    {
        private SecretPrefixUriReference myPrefixReference;

        public SecretPrefixUriReference PrefixUriReference
        {
            get
            {
                lock (this)
                {
                    return this.myPrefixReference ?? (this.myPrefixReference = new SecretPrefixUriReference(this));
                }
            }
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.PrefixUriReference);
        }

        public ResolveResultWithInfo Resolve()
        {
            return this.PrefixUriReference.Resolve();
        }

        public void SetName(string shortName)
        {
            this.PrefixUriReference.SetName(shortName);
        }
    }
}
