// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IDocCommentNode.cs
// </summary>
// ***********************************************************************

namespace ReSharper.NTriples.Tree
{
    public interface IDocCommentNode : ISecretCommentNode
    {
        IDocCommentNode ReplaceBy(IDocCommentNode docCommentNode);
    }
}
