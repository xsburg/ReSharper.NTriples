// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesFileElement.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Impl.Tree
{
    public abstract class NTriplesFileElement : FileElementBase
    {
        public abstract void Accept(TreeNodeVisitor visitor);
        public abstract void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context);
        public abstract TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context);
    }
}
