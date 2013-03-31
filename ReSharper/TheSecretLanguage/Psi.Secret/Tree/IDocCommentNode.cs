// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   IDocCommentNode.cs
// </summary>
// ***********************************************************************

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public interface IDocCommentNode : ISecretCommentNode
    {
        IDocCommentNode ReplaceBy(IDocCommentNode docCommentNode);
    }
}
