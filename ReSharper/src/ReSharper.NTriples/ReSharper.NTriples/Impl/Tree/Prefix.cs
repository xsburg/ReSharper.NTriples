// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   Prefix.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class Prefix
    {
        private NTriplesPrefixReference myPrefixReference;

        public NTriplesPrefixReference PrefixReference
        {
            get
            {
                lock (this)
                {
                    return this.myPrefixReference ?? (this.myPrefixReference = new NTriplesPrefixReference(this));
                }
            }
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.PrefixReference);
        }

        public ResolveResultWithInfo Resolve()
        {
            return this.PrefixReference.Resolve();
        }

        public void SetName(string shortName)
        {
            this.PrefixReference.SetName(shortName);
        }
    }
}
