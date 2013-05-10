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
  internal partial class Statement : NTriplesCompositeElement, ReSharper.NTriples.Tree.IStatement {
    public const short SUBJECT= ChildRole.LAST + 1;
    public const short FACTS= ChildRole.LAST + 2;
    internal Statement() : base() {
    }
    public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType {
      get { return ReSharper.NTriples.Impl.Tree.ElementType.STATEMENT; }
    }
    public override void Accept(ReSharper.NTriples.Tree.TreeNodeVisitor visitor) {
      visitor.VisitStatement(this);
    }
    public override void Accept<TContext>(ReSharper.NTriples.Tree.TreeNodeVisitor<TContext> visitor, TContext context) {
      visitor.VisitStatement(this, context);
    }
    public override TReturn Accept<TContext, TReturn>(ReSharper.NTriples.Tree.TreeNodeVisitor<TContext, TReturn> visitor, TContext context) {
      return visitor.VisitStatement(this, context);
    }
    private static readonly JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short> CHILD_ROLES = new JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short>(
      new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>[]
      {
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.FACTS, FACTS),
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.SUBJECT, SUBJECT),
      }
    );
    public override short GetChildRole (JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.TreeElement child) {
      return CHILD_ROLES[child.NodeType];
    }
    public virtual ReSharper.NTriples.Tree.IFacts FactsElement {
      get { return (ReSharper.NTriples.Tree.IFacts) FindChildByRole(FACTS); }
    }
    public virtual ReSharper.NTriples.Tree.ISubject Subject {
      get { return (ReSharper.NTriples.Tree.ISubject) FindChildByRole(SUBJECT); }
    }
    public virtual JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IFact>  Facts {
      get
      {
        CompositeElement current = this;  
    
        var result = JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IFact>.Empty;
        CompositeElement current0 = (CompositeElement)current.FindChildByRole (ReSharper.NTriples.Impl.Tree.Statement.FACTS);
        if (current0 != null) {
          result = ((CompositeElement)current0).FindListOfChildrenByRole<ReSharper.NTriples.Tree.IFact> (ReSharper.NTriples.Impl.Tree.Facts.FACTS);
        }
        return result;
      }
    }
    public virtual JetBrains.ReSharper.Psi.Tree.TreeNodeEnumerable<ReSharper.NTriples.Tree.IFact> FactsEnumerable {
      get
      {
        CompositeElement current = this;
    
        return new JetBrains.ReSharper.Psi.Tree.TreeNodeEnumerable<ReSharper.NTriples.Tree.IFact>(current, ReSharper.NTriples.Impl.Tree.Statement.FACTS, ReSharper.NTriples.Impl.Tree.Facts.FACTS);
      }
    }
    public virtual ReSharper.NTriples.Tree.IFacts SetFactsElement (ReSharper.NTriples.Tree.IFacts param)
    {
      using (JetBrains.Application.WriteLockCookie.Create (this.IsPhysical()))
      {
        TreeElement current = null, next = GetNextFilteredChild (current), result = null;
        next = GetNextFilteredChild (current);
        if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.SUBJECT) {
          next = GetNextFilteredChild (current);
          if (next == null) {
            return (ReSharper.NTriples.Tree.IFacts)result;
          } else {
            if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.SUBJECT) {
              current = next;
            } else {
              return (ReSharper.NTriples.Tree.IFacts)result;
            }
          }
          next = GetNextFilteredChild (current);
          if (next == null) {
            if (param == null) return null;
            result = current = (TreeElement)JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.ModificationUtil.AddChildAfter (this, current, (JetBrains.ReSharper.Psi.Tree.ITreeNode)param);
          } else {
            if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.FACTS) {
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
        }
        else if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.KEYWORD_STATEMENT) {
        }
        else return null;
        return (ReSharper.NTriples.Tree.IFacts)result;
      }
    }
    public virtual ReSharper.NTriples.Tree.ISubject SetSubject (ReSharper.NTriples.Tree.ISubject param)
    {
      using (JetBrains.Application.WriteLockCookie.Create (this.IsPhysical()))
      {
        TreeElement current = null, next = GetNextFilteredChild (current), result = null;
        next = GetNextFilteredChild (current);
        if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.SUBJECT) {
          next = GetNextFilteredChild (current);
          if (next == null) {
            if (param == null) return null;
            result = current = (TreeElement)JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.ModificationUtil.AddChildAfter (this, current, (JetBrains.ReSharper.Psi.Tree.ITreeNode)param);
          } else {
            if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.SUBJECT) {
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
        }
        else if (next.NodeType == ReSharper.NTriples.Impl.Tree.ElementType.KEYWORD_STATEMENT) {
        }
        else return null;
        return (ReSharper.NTriples.Tree.ISubject)result;
      }
    }
    public override string ToString() {
      return "IStatement";
    }
  }
}
