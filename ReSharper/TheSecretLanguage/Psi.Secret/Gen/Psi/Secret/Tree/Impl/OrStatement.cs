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
  internal partial class OrStatement : SecretCompositeElement, JetBrains.ReSharper.Psi.Secret.Tree.IOrStatement {
    internal OrStatement() : base() {
    }
    public override JetBrains.ReSharper.Psi.ExtensionsAPI.Tree.NodeType NodeType {
      get { return JetBrains.ReSharper.Psi.Secret.Impl.Tree.ElementType.OR_STATEMENT; }
    }
    public override void Accept(JetBrains.ReSharper.Psi.Secret.Tree.TreeNodeVisitor visitor) {
      visitor.VisitOrStatement(this);
    }
    public override void Accept<TContext>(JetBrains.ReSharper.Psi.Secret.Tree.TreeNodeVisitor<TContext> visitor, TContext context) {
      visitor.VisitOrStatement(this, context);
    }
    public override TReturn Accept<TContext, TReturn>(JetBrains.ReSharper.Psi.Secret.Tree.TreeNodeVisitor<TContext, TReturn> visitor, TContext context) {
      return visitor.VisitOrStatement(this, context);
    }
    public override string ToString() {
      return "IOrStatement";
    }
  }
}
