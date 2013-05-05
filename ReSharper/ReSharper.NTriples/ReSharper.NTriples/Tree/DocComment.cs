// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   DocComment.cs
// </summary>
// ***********************************************************************

using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.NTriples.Tree
{
    internal class DocComment : Comment, IDocCommentNode
    {
        public DocComment(TokenNodeType nodeType, string text) :
            base(nodeType, text)
        {
        }

        public override IChameleonNode ReSync(CachingLexer cachingLexer, TreeTextRange changedRange, int insertedTextLen)
        {
            return base.ReSync(cachingLexer, changedRange, insertedTextLen) as DocComment;
        }

        public IDocCommentNode ReplaceBy(IDocCommentNode docCommentNode)
        {
            using (WriteLockCookie.Create(this.IsPhysical()))
            {
                return ModificationUtil.ReplaceChild(this, docCommentNode);
            }
        }
    }
}
