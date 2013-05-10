﻿// ***********************************************************************
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
            /*var ruleDeclaration = element as IRuleDeclaration;
            if (ruleDeclaration != null)
            {
                return ruleDeclaration.RuleName.GetText();
            }*/
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