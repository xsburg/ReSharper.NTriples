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
using JetBrains.ReSharper.Psi.Secret.Parsing;
namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree {
  internal partial class Statement : SecretCompositeElement, JetBrains.ReSharper.Psi.Secret.Tree.IStatement {
    public const short SUBJECT= ChildRole.LAST + 1;
    public const short FACTS= ChildRole.LAST + 2;
    internal Statement() : base() {
    }
    public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType {
      get { return JetBrains.ReSharper.Psi.Secret.Impl.Tree.ElementType.STATEMENT; }
    }
    public override void Accept(JetBrains.ReSharper.Psi.Secret.Tree.TreeNodeVisitor visitor) {
      visitor.VisitStatement(this);
    }
    public override void Accept<TContext>(JetBrains.ReSharper.Psi.Secret.Tree.TreeNodeVisitor<TContext> visitor, TContext context) {
      visitor.VisitStatement(this, context);
    }
    public override TReturn Accept<TContext, TReturn>(JetBrains.ReSharper.Psi.Secret.Tree.TreeNodeVisitor<TContext, TReturn> visitor, TContext context) {
      return visitor.VisitStatement(this, context);
    }
    private static readonly JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short> CHILD_ROLES = new JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeTypeDictionary<short>(
      new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>[]
      {
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(JetBrains.ReSharper.Psi.Secret.Impl.Tree.ElementType.SUBJECT, SUBJECT),
        new System.Collections.Generic.KeyValuePair<JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType, short>(JetBrains.ReSharper.Psi.Secret.Impl.Tree.ElementType.FACTS, FACTS),
      }
    );
    public override short GetChildRole (JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.TreeElement child) {
      return CHILD_ROLES[child.NodeType];
    }
    public override string ToString() {
      return "IStatement";
    }
  }
}
