// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesDeclaredElementPresenter.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Services
{
    internal class NTriplesDeclaredElementPresenter : IDeclaredElementPresenter
    {
        public string Format(
            DeclaredElementPresenterStyle style,
            IDeclaredElement element,
            ISubstitution substitution,
            out DeclaredElementPresenterMarking marking)
        {
            marking = new DeclaredElementPresenterMarking();
            var uriIdentifier = element as IUriIdentifierDeclaredElement;
            if (uriIdentifier != null)
            {
                return uriIdentifier.GetUri();
            }

            return element.ShortName;
        }

        public string Format(ParameterKind parameterKind)
        {
            throw new NotImplementedException();
        }

        public string Format(AccessRights accessRights)
        {
            throw new NotImplementedException();
        }
    }
}
