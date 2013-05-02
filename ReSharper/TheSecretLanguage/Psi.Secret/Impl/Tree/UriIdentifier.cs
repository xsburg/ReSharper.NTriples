// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   UriIdentifier.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
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

        public UriIdentifierKind GetKind()
        {
            var kind = UriIdentifierKind.Other;
            var parent2 = this.Parent;
            while (parent2 != null && !(parent2 is ISentence) && !(parent2 is IAnonymousIdentifier))
            {
                if (parent2 is ISubject)
                {
                    kind = UriIdentifierKind.Subject;
                    break;
                }

                if (parent2 is IPredicate)
                {
                    kind = UriIdentifierKind.Predicate;
                    break;
                }

                if (parent2 is IObjects)
                {
                    kind = UriIdentifierKind.Object;
                    break;
                }

                parent2 = parent2.Parent;
            }

            return kind;
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
