// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesIdentifierFilter.cs
// </summary>
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;
using IStatement = ReSharper.NTriples.Tree.IStatement;
using IIdentifier = ReSharper.NTriples.Tree.IIdentifier;

namespace ReSharper.NTriples.Impl
{
    public static class NTriplesIdentifierFilter
    {
        private const string Class = Prefix + "Class";
        private const string Property = Prefix + "Property";
        private const string UserProperty = Prefix + "UserProperty";
        private const string Prefix = "http://comindware.com/logics#";

        public static IdentifierInfo GetIdentifierInfo(IIdentifier identifier)
        {
            var kind = IdentifierKind.Other;
            var parent2 = identifier.Parent;
            while (parent2 != null && !(parent2 is ISentence) && !(parent2 is IAnonymousIdentifier))
            {
                if (parent2 is ISubject)
                {
                    kind = IdentifierKind.Subject;
                    break;
                }

                if (parent2 is IPredicate)
                {
                    kind = IdentifierKind.Predicate;
                    break;
                }

                if (parent2 is IObjects)
                {
                    kind = IdentifierKind.Object;
                    break;
                }

                parent2 = parent2.Parent;
            }

            if (parent2 != null && kind == IdentifierKind.Subject)
            {
                var statement = parent2.Parent as IStatement;
                if (statement != null)
                {
                    return AnalizeStatement(statement);
                }
            }

            return new IdentifierInfo(kind);
        }

        private static IdentifierInfo AnalizeStatement(IStatement statement)
        {
            var typeDeclarations = new List<string>();
            foreach (var fact in statement.FactsEnumerable)
            {
                var predicate = fact.Predicate;
                if (predicate != null && predicate.FirstChild != null &&
                    predicate.FirstChild.GetTokenType() == NTriplesTokenType.A_KEYWORD)
                {
                    foreach (var expression in fact.ObjectsEnumerable)
                    {
                        using (var e = expression.IdentifiersEnumerable.GetEnumerator())
                        {
                            IIdentifier identifier;
                            IUriIdentifier uriIdentifier;
                            string uri;
                            if (e.MoveNext() &&
                                (identifier = e.Current) != null &&
                                !e.MoveNext() &&
                                (uriIdentifier = identifier.FirstChild as IUriIdentifier) != null &&
                                !string.IsNullOrEmpty(uri = uriIdentifier.GetUri()))
                            {
                                // The expression has the only child identifier that is the URI identifier that is not empty
                                typeDeclarations.Add(uri);
                            }
                        }
                    }
                }
            }

            bool isClass = false;
            bool isProperty = false;
            bool isUserProperty = false;
            foreach (var declaration in typeDeclarations)
            {
                if (declaration == Class)
                {
                    isClass = true;
                } else if (declaration == Property)
                {
                    isProperty = true;
                }
                else if (declaration == UserProperty)
                {
                    isUserProperty = true;
                }
            }

            return new IdentifierInfo(IdentifierKind.Subject, isClass, isProperty, isUserProperty, typeDeclarations.ToArray());
        }

        public static IdentifierKind GetIdentifierKind(IIdentifier identifier)
        {
            var kind = IdentifierKind.Other;
            var parent2 = identifier.Parent;
            while (parent2 != null && !(parent2 is ISentence) && !(parent2 is IAnonymousIdentifier))
            {
                if (parent2 is ISubject)
                {
                    kind = IdentifierKind.Subject;
                    break;
                }

                if (parent2 is IPredicate)
                {
                    kind = IdentifierKind.Predicate;
                    break;
                }

                if (parent2 is IObjects)
                {
                    kind = IdentifierKind.Object;
                    break;
                }

                parent2 = parent2.Parent;
            }

            return kind;
        }

        public static IEnumerable<IUriIdentifierDeclaredElement> GetImportantSubjects(IEnumerable<IDeclaration> elements)
        {
            return GetImportantSubjects((IEnumerable)elements);
        }

        public static IEnumerable<IUriIdentifierDeclaredElement> GetImportantSubjects(IEnumerable<IDeclaredElement> elements)
        {
            return GetImportantSubjects((IEnumerable)elements);
        }

        public static bool IsImportantSubject(IUriIdentifierDeclaredElement uriIdentifier)
        {
            return uriIdentifier.GetInfo().IsTypeDeclaration;
        }

        private static IEnumerable<IUriIdentifierDeclaredElement> GetImportantSubjects(IEnumerable elements)
        {
            var subjects =
                elements.OfType<IUriIdentifierDeclaredElement>().Where(e => e.GetKind() == IdentifierKind.Subject).ToArray();

            var importantSubjects = subjects.Where(IsImportantSubject).ToArray();

            return importantSubjects.Any()
                       ? importantSubjects
                       : subjects;
        }
    }
}
