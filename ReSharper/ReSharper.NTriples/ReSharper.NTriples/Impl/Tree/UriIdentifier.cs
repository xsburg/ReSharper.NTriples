// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   UriIdentifier.cs
// </summary>
// ***********************************************************************

using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class UriIdentifier
    {
        public IUriIdentifierDeclaredElement DescendantDeclaredElement
        {
            get
            {
                return this.Prefix != null
                           ? (IUriIdentifierDeclaredElement)this.LocalName
                           : (IUriIdentifierDeclaredElement)this.UriStringElement;
            }
        }

        public IdentifierKind GetKind()
        {
            var identifier = this.Parent as Identifier;
            if (identifier == null)
            {
                return IdentifierKind.Other;
            }

            return identifier.GetKind();
        }

        public string GetLocalName()
        {
            return this.DescendantDeclaredElement == null
                       ? null
                       : this.DescendantDeclaredElement.GetLocalName();
        }

        public string GetNamespace()
        {
            return this.DescendantDeclaredElement == null
                       ? null
                       : this.DescendantDeclaredElement.GetNamespace();
        }

        public string GetUri()
        {
            return this.DescendantDeclaredElement == null
                       ? null
                       : this.DescendantDeclaredElement.GetUri();
        }
    }
}
