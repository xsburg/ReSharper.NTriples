// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ISecretCommentNode.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public interface ISecretCommentNode : ICommentNode
    {
        CommentType CommentType { get; }
    }
}
