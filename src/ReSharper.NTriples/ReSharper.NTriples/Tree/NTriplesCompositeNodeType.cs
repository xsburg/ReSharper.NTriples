// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCompositeNodeType.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace ReSharper.NTriples.Tree
{
    public abstract class NTriplesCompositeNodeType : CompositeNodeType
    {
        protected NTriplesCompositeNodeType(string s, int index)
            : base(s, index)
        {
        }
    }
}
