//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by IntelliJ parserGen
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 0168, 0219, 0108, 0414
// ReSharper disable RedundantNameQualifier
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
namespace ReSharper.NTriples.Tree {
  public partial interface IPrefixDeclaration : ReSharper.NTriples.Tree.ISecretTreeNode {
    JetBrains.ReSharper.Psi.Tree.ITokenNode Prefix { get; }
  
    ReSharper.NTriples.Tree.IPrefixName PrefixName { get; }
  
    ReSharper.NTriples.Tree.IPrefixUri PrefixUri { get; }
  
    JetBrains.ReSharper.Psi.Tree.ITokenNode UriString { get; }
  
    ReSharper.NTriples.Tree.IPrefixName SetPrefixName (ReSharper.NTriples.Tree.IPrefixName param);
  
    ReSharper.NTriples.Tree.IPrefixUri SetPrefixUri (ReSharper.NTriples.Tree.IPrefixUri param);
  
  }
}
