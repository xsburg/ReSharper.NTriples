// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ISecretIdentifier.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.NTriples.Tree
{
    public interface ISecretIdentifier : JetBrains.ReSharper.Psi.Tree.IIdentifier, ITokenNode, ISecretTreeNode
    {
    }
}
