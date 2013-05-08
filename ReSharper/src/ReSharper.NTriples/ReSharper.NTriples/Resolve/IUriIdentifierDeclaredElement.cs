// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IUriIdentifierDeclaredElement.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using ReSharper.NTriples.Cache;

namespace ReSharper.NTriples.Resolve
{
    public interface IUriIdentifierDeclaredElement : IDeclaredElement
    {
        IdentifierKind GetKind();
        string GetLocalName();
        string GetNamespace();
        string GetUri();
    }
}
