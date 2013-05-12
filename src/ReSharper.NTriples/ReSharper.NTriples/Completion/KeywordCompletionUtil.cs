// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   KeywordCompletionUtil.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;
using IIdentifier = ReSharper.NTriples.Tree.IIdentifier;

namespace ReSharper.NTriples.Completion
{
    internal static class KeywordCompletionUtil
    {
        private const string OfPredicateKeyword1 = "@of";
        private const string OfPredicateKeyword2 = "of";

        private static readonly string[] DirectiveKeywords = new[]
            {
                "@prefix",
                "@std_prefix",
                "@extension",
                "@using",
                "@axis-default",
                "@forAll",
                "@forSome"
            };

        private static readonly string[] MetaKeywords = new[]
            {
                "@in",
                "@for",
                "@out",
                "@axis",
                "@meta",
                "out",
                "in",
                "axis",
                "meta",
            };

        private static readonly string[] ObjectLiteralKeywords = new[]
            {
                "true",
                "false",
                "null"
            };

        private static readonly string[] PredicateKeywords = new[]
            {
                "a",
                "@has",
                "@is"
            };

        public static IEnumerable<string> GetAplicableKeywords(
            INTriplesFile file, TreeTextRange referenceRange, NTriplesCodeCompletionContext context)
        {
            var list = new List<string>();
            var node = file.FindNodeAt(referenceRange);
            if (node == null)
            {
                return list;
            }

            var isTopLevel = IsTopLevel(node);
            var kind = GetKind(node);
            if (isTopLevel && (kind == IdentifierKind.Subject || kind == IdentifierKind.Other))
            {
                list.AddRange(DirectiveKeywords);
                list.AddRange(MetaKeywords);
            }

            if (kind != IdentifierKind.Subject)
            {
                list.AddRange(PredicateKeywords);
                list.Add(OfPredicateKeyword1);
                list.Add(OfPredicateKeyword2);
            }

            list.AddRange(ObjectLiteralKeywords);

            return list;
        }

        private static IdentifierKind GetKind(ITreeNode node)
        {
            var kind = IdentifierKind.Other;

            IIdentifier identifier;
            if (node.Parent is IAnonymousIdentifier)
            {
                kind = IdentifierKind.Predicate;
            }
            else if ((identifier = node.GetParent<IIdentifier>(3) as IIdentifier) != null)
            {
                kind = identifier.GetKind();
            }
            else
            {
                if (node.Parent is IObjects || HasPrevSibling<IPredicate>(node)) // that is wrong!
                {
                    kind = IdentifierKind.Object;
                }
                else if (node.Parent is IFacts || node.Parent is IFact || node.Parent is IIsOfExpression ||
                         HasPrevSibling<ISubject>(node))
                {
                    kind = IdentifierKind.Predicate;
                }
            }

            return kind;
        }

        private static bool HasPrevSibling<T>(ITreeNode node) where T : ITreeNode
        {
            var prevSibling = node.PrevSibling;
            while (prevSibling != null)
            {
                if (prevSibling is T)
                {
                    return true;
                }

                var tokenType = prevSibling.GetTokenType();
                if (tokenType == null || !tokenType.IsWhitespace)
                {
                    return false;
                }

                prevSibling = prevSibling.PrevSibling;
            }

            return false;
        }

        private static bool IsTopLevel(ITreeNode node)
        {
            var ai = node.GetContainingNode<IAnonymousIdentifier>();
            var f = node.GetContainingNode<IFormula>();
            return ai == null && f == null;
        }
    }
}
