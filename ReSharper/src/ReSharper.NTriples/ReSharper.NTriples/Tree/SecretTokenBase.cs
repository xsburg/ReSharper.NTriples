// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretTokenBase.cs
// </summary>
// ***********************************************************************

using System.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tree
{
    public abstract class SecretTokenBase : LeafElementBase, ISecretTreeNode, ITokenNode
    {
        public override PsiLanguageType Language
        {
            get
            {
                return SecretLanguage.Instance;
            }
        }

        public void Accept(TreeNodeVisitor visitor)
        {
            visitor.VisitNode(this);
        }

        public void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitNode(this, context);
        }

        public TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitNode(this, context);
        }

        public override StringBuilder GetText(StringBuilder to)
        {
            to.Append(this.GetText());
            return to;
        }

        public override IBuffer GetTextAsBuffer()
        {
            return new StringBuffer(this.GetText());
        }

        public TokenNodeType GetTokenType()
        {
            return (TokenNodeType)this.NodeType;
        }

        public override string ToString()
        {
            return base.ToString() + "(type:" + this.NodeType + ", text:" + this.GetText() + ")";
        }
    }
}
