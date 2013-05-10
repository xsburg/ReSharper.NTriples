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
using IStatement = ReSharper.NTriples.Tree.IStatement;

namespace ReSharper.NTriples.Impl
{
    public static class NTriplesIdentifierFilter
    {
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
            var statement = ((ITreeNode)uriIdentifier).GetContainingNode<IStatement>();
            if (statement == null)
            {
                return false;
            }

            foreach (var fact in statement.FactsEnumerable)
            {
                var predicate = fact.Predicate;
                if (predicate != null && predicate.FirstChild != null &&
                    predicate.FirstChild.GetTokenType() == NTriplesTokenType.A_KEYWORD)
                {
                    return true;
                }
            }

            return false;
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
