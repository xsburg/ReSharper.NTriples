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
  internal partial class Formula : NTriplesCompositeElement, ReSharper.NTriples.Tree.IFormula {
    public const short META= ChildRole.LAST + 1;
    public const short NAME_KEY_IDENTIFIER= ChildRole.LAST + 2;
    public const short SENTENCES= ChildRole.LAST + 3;
    internal Formula() : base() {
    }
    public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType {
      get { return ReSharper.NTriples.Impl.Tree.ElementType.FORMULA; }
    }
    public override void Accept(ReSharper.NTriples.Tree.TreeNodeVisitor visitor) {
      visitor.VisitFormula(this);
    }
    public override void Accept<TContext>(ReSharper.NTriples.Tree.TreeNodeVisitor<TContext> visitor, TContext context) {
      visitor.VisitFormula(this, context);
    }
    public override TReturn Accept<TContext, TReturn>(ReSharper.NTriples.Tree.TreeNodeVisitor<TContext, TReturn> visitor, TContext context) {
      return visitor.VisitFormula(this, context);
    }
    private static readonly JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short> CHILD_ROLES = new JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short>(
      new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>[]
      {
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.SENTENCES, SENTENCES),
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.META, META),
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(ReSharper.NTriples.Impl.Tree.ElementType.IDENTIFIER, NAME_KEY_IDENTIFIER),
      }
    );
    public override short GetChildRole (JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.TreeElement child) {
      return CHILD_ROLES[child.NodeType];
    }
    public override string ToString() {
      return "IFormula";
    }
  }
}
