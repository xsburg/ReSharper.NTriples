// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesSearcherFactory.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Impl.Tree;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Feature.Services.FindUsages
{
    [PsiSharedComponent]
    internal class NTriplesSearcherFactory : IDomainSpecificSearcherFactory
    {
        private readonly SearchDomainFactory mySearchDomainFactory;

        public NTriplesSearcherFactory(SearchDomainFactory searchDomainFactory)
        {
            this.mySearchDomainFactory = searchDomainFactory;
        }

        public IDomainSpecificSearcher CreateAnonymousTypeSearcher(
            IList<AnonymousTypeDescriptor> typeDescription, bool caseSensitive)
        {
            return null;
        }

        public IDomainSpecificSearcher CreateConstantExpressionSearcher(ConstantValue constantValue, bool onlyLiteralExpression)
        {
            return new ConstantExpressionDomainSpecificSearcher<NTriplesLanguage>(constantValue, onlyLiteralExpression);
        }

        public IDomainSpecificSearcher CreateConstructorSpecialReferenceSearcher(ICollection<IConstructor> constructors)
        {
            return null;
        }

        public IDomainSpecificSearcher CreateLateBoundReferenceSearcher(ICollection<IDeclaredElement> elements)
        {
            return new NTriplesReferenceSearcher(this, elements, true);
        }

        public IDomainSpecificSearcher CreateMethodsReferencedByDelegateSearcher(IDelegate @delegate)
        {
            return null;
        }

        public IDomainSpecificSearcher CreateReferenceSearcher(ICollection<IDeclaredElement> elements, bool findCandidates)
        {
            return new NTriplesReferenceSearcher(this, elements, false);
        }

        public IDomainSpecificSearcher CreateTextOccurenceSearcher(ICollection<IDeclaredElement> elements)
        {
            return null;
        }

        public IDomainSpecificSearcher CreateTextOccurenceSearcher(string subject)
        {
            return null;
        }

        public IEnumerable<string> GetAllPossibleWordsInFile(IDeclaredElement element)
        {
            var names = new JetHashSet<string>
                {
                    element.ShortName
                };

            return names;
        }

        public ISearchDomain GetDeclaredElementSearchDomain(IDeclaredElement declaredElement)
        {
            var uriIdentifier = declaredElement as IUriIdentifierDeclaredElement;
            if (uriIdentifier != null)
            {
                var cache = declaredElement.GetSolution().GetComponent<NTriplesCache>();
                var files = cache.GetFilesContainingUri(uriIdentifier.GetNamespace(), uriIdentifier.GetLocalName());
                return this.mySearchDomainFactory.CreateSearchDomain(files);
            }

            if (declaredElement is PrefixDeclaration)
            {
                var files = declaredElement.GetSourceFiles();
                if (files.Count > 0)
                {
                    return this.mySearchDomainFactory.CreateSearchDomain(files[0]);
                }
            }

            return this.mySearchDomainFactory.CreateSearchDomain(declaredElement.GetSolution(), false);
        }

        public JetTuple<ICollection<IDeclaredElement>, Predicate<IFindResultReference>, bool> GetDerivedFindRequest(
            IFindResultReference result)
        {
            return null;
        }

        public JetTuple<ICollection<IDeclaredElement>, bool> GetNavigateToTargets(IDeclaredElement element)
        {
            return null;
        }

        public IEnumerable<Pair<IDeclaredElement, Predicate<FindResult>>> GetRelatedDeclaredElements(IDeclaredElement element)
        {
            var uriIdentifier = element as IUriIdentifierDeclaredElement;
            if (uriIdentifier != null)
            {
                var declarations = element.GetDeclarations();
                var actualDecalrations = NTriplesIdentifierFilter.GetTypeDeclarations(declarations).ToArray();
                if (!actualDecalrations.Any())
                {
                    var subjects =
                        declarations.OfType<IUriIdentifierDeclaredElement>().Where(d => d.GetKind() == IdentifierKind.Subject).ToArray();
                    if (subjects.Any())
                    {
                        actualDecalrations = subjects;
                    }
                    else
                    {
                        actualDecalrations = new[] { (IUriIdentifierDeclaredElement)declarations.First() };
                    }
                }

                Func<IDeclaration, Pair<IDeclaredElement, Predicate<FindResult>>> selector =
                    e => new Pair<IDeclaredElement, Predicate<FindResult>>(e.DeclaredElement, JetPredicate<FindResult>.True);
                if (actualDecalrations.Any())
                {
                    return actualDecalrations.Cast<IDeclaration>().Select(selector);
                }

                if (declarations.Any())
                {
                    return declarations.Select(selector);
                }
            }

            return new[] { new Pair<IDeclaredElement, Predicate<FindResult>>(element, JetPredicate<FindResult>.True) };
        }

        public bool IsCompatibleWithLanguage(PsiLanguageType languageType)
        {
            return languageType.Is<NTriplesLanguage>();
        }

        public ICollection<FindResult> TransformNavigationTargets(ICollection<FindResult> targets)
        {
            return targets;
        }
    }
}
