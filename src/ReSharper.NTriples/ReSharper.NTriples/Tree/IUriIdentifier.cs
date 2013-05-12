// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IUriIdentifier.cs
// </summary>
// ***********************************************************************

using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Tree
{
    /*public partial interface IUriIdentifier : IDeclaration//, IChameleonNode
    {
        void SetReferenceName(string shortName);
        string GetFullName(INTriplesFile file);
    }*/

    public partial interface IUriIdentifier
    {
        IUriIdentifierDeclaredElement DescendantDeclaredElement { get; }
        IdentifierKind GetKind();
        string GetLocalName();
        string GetNamespace();
        string GetUri();
    }
}
