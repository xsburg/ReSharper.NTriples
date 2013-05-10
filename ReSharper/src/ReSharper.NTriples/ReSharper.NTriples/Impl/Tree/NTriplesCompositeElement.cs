// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesCompositeElement.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Impl.Tree
{
    public abstract class NTriplesCompositeElement : CompositeElement, INTriplesTreeNode
    {
        public override PsiLanguageType Language
        {
            get
            {
                return NTriplesLanguage.Instance;
            }
        }

        public virtual void Accept(TreeNodeVisitor visitor)
        {
            visitor.VisitNode(this);
        }

        public virtual void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitNode(this, context);
        }

        public virtual TReturn Accept<TContext, TReturn>(TreeNodeVisitor<TContext, TReturn> visitor, TContext context)
        {
            return visitor.VisitNode(this, context);
        }
    }
}
