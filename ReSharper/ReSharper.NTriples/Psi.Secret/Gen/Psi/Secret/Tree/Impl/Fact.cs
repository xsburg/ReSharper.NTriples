//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by IntelliJ parserGen
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 0168, 0219, 0108, 0414
// ReSharper disable RedundantNameQualifier
using System;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using ReSharper.NTriples.Parsing;
namespace ReSharper.NTriples.Impl.Tree {
  internal partial class Fact : SecretCompositeElement, ReSharper.NTriples.Tree.IFact {
    public const short PREDICATE= ChildRole.LAST + 1;
    public const short OBJECTS= ChildRole.LAST + 2;
    internal Fact() : base() {
    }
    public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType {
      get { return ReSharper.NTriples.Impl.Tree.ElementType.FACT; }
    }
    public override void Accept(ReSharper.NTriples.Tree.TreeNodeVisitor visitor) {
      visitor.VisitFact(this);
    }
    public override void Accept<TContext>(ReSharper.NTriples.Tree.TreeNodeVisitor<TContext> visitor, TContext context) {
      visitor.VisitFact(this, context);
    }
    public override TReturn Accept<TContext, TReturn>(ReSharper.NTriples.Tree.TreeNodeVisitor<TContext, TReturn> visitor, TContext context) {
      return visitor.VisitFact(this, context);
    }
    private static readonly JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short> CHILD_ROLES = new JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short>(
      new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>[]
      {
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.OBJECTS, OBJECTS),
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.PREDICATE, PREDICATE),
      }
    );
    public override short GetChildRole (JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.TreeElement child) {
      return CHILD_ROLES[child.NodeType];
    }
    public virtual ReSharper.NTriples.Tree.IPredicate Predicate {
      get { return (ReSharper.NTriples.Tree.IPredicate) FindChildByRole(PREDICATE); }
    }
    public virtual JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IExpression>  Objects {
      get
      {
        CompositeElement current = this;  
    
        var result = JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IExpression>.Empty;
        CompositeElement current0 = (CompositeElement)current.FindChildByRole (ReSharper.NTriples.Impl.Tree.Fact.OBJECTS);
        if (current0 != null) {
          result = ((CompositeElement)current0).FindListOfChildrenByRole<ReSharper.NTriples.Tree.IExpression> (ReSharper.NTriples.Impl.Tree.Objects.IDENTIFIERS);
        }
        return result;
      }
    }
    public virtual JetBrains.ReSharper.Psi.Tree.TreeNodeEnumerable<ReSharper.NTriples.Tree.IExpression> ObjectsEnumerable {
      get
      {
        CompositeElement current = this;
    
        return new JetBrains.ReSharper.Psi.Tree.TreeNodeEnumerable<ReSharper.NTriples.Tree.IExpression>(current, ReSharper.NTriples.Impl.Tree.Fact.OBJECTS, ReSharper.NTriples.Impl.Tree.Objects.IDENTIFIERS);
      }
    }
    public virtual JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IIdentifier>  PredicateIdentifiers {
      get
      {
        CompositeElement current = this;  
    
        var result = JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IIdentifier>.Empty;
        CompositeElement current0 = (CompositeElement)current.FindChildByRole (ReSharper.NTriples.Impl.Tree.Fact.PREDICATE);
        if (current0 != null) {
          CompositeElement current1 = (CompositeElement)current0.FindChildByRole (ReSharper.NTriples.Impl.Tree.Predicate.EXPRESSION);
          if (current1 != null) {
            result = ((CompositeElement)current1).FindListOfChildrenByRole<ReSharper.NTriples.Tree.IIdentifier> (ReSharper.NTriples.Impl.Tree.Expression.IDENTIFIER);
          }
        }
        return result;
      }
    }
    public virtual JetBrains.ReSharper.Psi.Tree.TreeNodeEnumerable<ReSharper.NTriples.Tree.IIdentifier> PredicateIdentifiersEnumerable {
      get
      {
        CompositeElement current = this;
    
        return new JetBrains.ReSharper.Psi.Tree.TreeNodeEnumerable<ReSharper.NTriples.Tree.IIdentifier>(current, ReSharper.NTriples.Impl.Tree.Fact.PREDICATE, ReSharper.NTriples.Impl.Tree.Predicate.EXPRESSION, ReSharper.NTriples.Impl.Tree.Expression.IDENTIFIER);
      }
    }
    public virtual ReSharper.NTriples.Tree.IPredicate SetPredicate (ReSharper.NTriples.Tree.IPredicate param)
    {
      using (JetBrains.Application.WriteLockCookie.Create (this.IsPhysical()))
      {
        TreeElement current = null, next = GetNextFilteredChild (current), result = null;
        next = GetNextFilteredChild (current);
        if (next == null) {
          if (param == null) return null;
          result = current = (TreeElement)JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.ModificationUtil.AddChildAfter (this, current, (JetBrains.ReSharper.Psi.Tree.ITreeNode)param);
        } else {
          if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.PREDICATE) {
            if (param != null) {
              result = current = (TreeElement)JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.ModificationUtil.ReplaceChild(next, (JetBrains.ReSharper.Psi.Tree.ITreeNode)param);
            } else {
              current = GetNextFilteredChild (next);
              JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.ModificationUtil.DeleteChild (next);
            }
          } else {
            if (param == null) return null;
            result = (TreeElement)JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.ModificationUtil.AddChildBefore(next, (JetBrains.ReSharper.Psi.Tree.ITreeNode)param);
            current = next;
          }
        }
        return (ReSharper.NTriples.Tree.IPredicate)result;
      }
    }
    public override string ToString() {
      return "IFact";
    }
  }
}
