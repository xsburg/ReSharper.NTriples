// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ISecretTreeNode.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.NTriples.Tree
{
    public interface ISecretTreeNode : ITreeNode
    {
        void Accept(TreeNodeVisitor visitor);
        void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context);
        TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context);
    }
}
