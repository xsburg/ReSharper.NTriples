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
  public partial interface IUriIdentifier : ReSharper.NTriples.Tree.INTriplesTreeNode {
    ReSharper.NTriples.Tree.ILocalName LocalName { get; }
  
    ReSharper.NTriples.Tree.IPrefix Prefix { get; }
  
    ReSharper.NTriples.Tree.IUriString UriStringElement { get; }
  
    JetBrains.ReSharper.Psi.Tree.ITokenNode UriString { get; }
  
    ReSharper.NTriples.Tree.ILocalName SetLocalName (ReSharper.NTriples.Tree.ILocalName param);
  
    ReSharper.NTriples.Tree.IPrefix SetPrefix (ReSharper.NTriples.Tree.IPrefix param);
  
    ReSharper.NTriples.Tree.IUriString SetUriStringElement (ReSharper.NTriples.Tree.IUriString param);
  
  }
}
