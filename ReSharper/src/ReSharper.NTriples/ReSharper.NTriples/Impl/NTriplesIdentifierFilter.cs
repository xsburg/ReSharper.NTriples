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
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;
using IExpression = ReSharper.NTriples.Tree.IExpression;
using IStatement = ReSharper.NTriples.Tree.IStatement;
using IIdentifier = ReSharper.NTriples.Tree.IIdentifier;
using JetBrains.ProjectModel;

namespace ReSharper.NTriples.Impl
{
    public static class NTriplesIdentifierFilter
    {
        private const string Class = Prefix + "Class";
        private const string Property = Prefix + "Property";
        private const string UserProperty = Prefix + "UserProperty";
        private const string Prefix = "http://comindware.com/logics#";
        private const string TypePropertyDeclaration = Prefix + "property";

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
            var typePropertyDeclarations = new List<string>();
            foreach (var fact in statement.FactsEnumerable)
            {
                var predicate = fact.Predicate;
                if (predicate != null && predicate.FirstChild != null &&
                    predicate.FirstChild.GetTokenType() == NTriplesTokenType.A_KEYWORD)
                {
                    typeDeclarations.AddRange(
                        fact.ObjectsEnumerable.Select(expression => expression.ToUri()).Where(uri => uri != null));
                }
                else if (GetExpressionUri(fact.PredicateIdentifiersEnumerable) == TypePropertyDeclaration)
                {
                    typePropertyDeclarations.AddRange(fact.ObjectsEnumerable.SelectNotNull(e => e.ToUri()));
                }
            }

            bool isClass = typeDeclarations.Any(declaration => declaration == Class);
            if (isClass || typePropertyDeclarations.Count > 0)
            {
                return IdentifierInfo.CreateClassDeclaration(isClass, typePropertyDeclarations.ToArray());
            }
            
            if (typeDeclarations.Any())
            {
                return IdentifierInfo.CreateClassInstantiation(typeDeclarations.ToArray());
            }

            return new IdentifierInfo(IdentifierKind.Subject);
        }

        public static string ToUri(this ISubject subject)
        {
            var expression = subject.FirstChild as IExpression;
            if (expression == null)
            {
                return null;
            }

            return GetExpressionUri(expression.IdentifiersEnumerable);
        }

        public static string ToUri(this IPredicate predicate)
        {
            var expression = predicate.FirstChild as IExpression;
            if (expression == null)
            {
                return null;
            }

            return GetExpressionUri(expression.IdentifiersEnumerable);
        }

        public static string ToUri(this IExpression expression)
        {
            if (expression == null)
            {
                return null;
            }

            return GetExpressionUri(expression.IdentifiersEnumerable);
        }

        private static string GetExpressionUri(TreeNodeEnumerable<IIdentifier> expressionIdentifiers)
        {
            using (var e = expressionIdentifiers.GetEnumerator())
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
                    return uri;
                }
            }

            return null;
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

        public static IEnumerable<IUriIdentifierDeclaredElement> GetTypeDeclarations(IEnumerable<IDeclaration> elements)
        {
            return GetTypeDeclarations(elements.OfType<IUriIdentifierDeclaredElement>().ToArray());
        }

        public static IEnumerable<IUriIdentifierDeclaredElement> GetTypeDeclarations(IEnumerable<IDeclaredElement> elements)
        {
            return GetTypeDeclarations(elements.OfType<IUriIdentifierDeclaredElement>().ToArray());
        }

        public static bool IsImportantSubject(IUriIdentifierDeclaredElement uriIdentifier)
        {
            return uriIdentifier.GetInfo().IsClassDeclaration;
        }

        private static IEnumerable<IUriIdentifierDeclaredElement> GetTypeDeclarations(IList<IUriIdentifierDeclaredElement> declaredUriElements)
        {
            if (!declaredUriElements.Any())
            {
                return EmptyList<IUriIdentifierDeclaredElement>.InstanceList;
            }

            var uriElement = declaredUriElements[0];
            var uri = uriElement.GetUri();
            var cache = ((ITreeNode)uriElement).GetSolution().GetComponent<NTriplesCache>();
            var typeDeclarations = cache.GetTypeDeclarations(uri).SelectNotNull(GetDeclaredElement).ToArray();
            typeDeclarations = typeDeclarations.Intersect(declaredUriElements).ToArray();

            return typeDeclarations;
        }

        public static bool HasTypeDeclaration(IDeclaration element)
        {
            var uriDeclaredElement = element as IUriIdentifierDeclaredElement;
            if (uriDeclaredElement == null)
            {
                return false;
            }

            var uri = uriDeclaredElement.GetUri();
            if (string.IsNullOrEmpty(uri))
            {
                return false;
            }

            var cache = element.GetSolution().GetComponent<NTriplesCache>();
            return cache.HasTypeDeclarations(uri);
        }

        private static IUriIdentifierDeclaredElement GetDeclaredElement(NTriplesUriIdentifierSymbol symbol)
        {
            var file = symbol.SourceFile.GetPsiFile<NTriplesLanguage>(new DocumentRange(symbol.SourceFile.Document, 0));
            if (file == null)
            {
                return null;
            }

            var treeNode = file.FindNodeAt(new TreeTextRange(new TreeOffset(symbol.Offset), 1));
            if (treeNode == null)
            {
                return null;
            }

            var uriIdentifier = treeNode.GetContainingNode<IUriIdentifier>();
            if (uriIdentifier == null)
            {
                return null;
            }

            return uriIdentifier.DescendantDeclaredElement;
        }

        public static IEnumerable<IExpression> GetPropertyExpressions(this IStatement statement)
        {
            return statement.FactsEnumerable.Where(fact => fact.Predicate.ToUri() == TypePropertyDeclaration)
                .SelectMany(propertyFact => propertyFact.ObjectsEnumerable);
        }

        public static IEnumerable<string> GetPropertyUris(this IStatement statement)
        {
            return statement.FactsEnumerable.Where(fact => fact.Predicate.ToUri() == TypePropertyDeclaration)
                .SelectMany(propertyFact => propertyFact.ObjectsEnumerable).SelectNotNull(ToUri);
        }
    }
}
