// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   KeywordCompletionUtil.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using IIdentifier = JetBrains.ReSharper.Psi.Secret.Tree.IIdentifier;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    internal static class KeywordCompletionUtil
    {
        private const string OfPredicateKeyword = "@of";

        public static IEnumerable<string> GetAplicableKeywords(ISecretFile file, TreeTextRange referenceRange, SecretCodeCompletionContext context)
        {
            var list = new List<string>();
            var token = file.FindNodeAt(referenceRange);
            if (token == null)
            {
                return list;
            }

            var references = file.FindReferencesAt(referenceRange);

            if (references.Length == 0 && token.Parent is ISentences && token.Parent.Parent is ISecretFile)
            {
                list.AddRange(DirectiveKeywords);
                list.AddRange(MetaKeywords);
            }
            else if (references.OfType<SecretPrefixReference>().Any() || context.ReparsedContext.Reference is SecretPrefixReference)
            {
                IdentifierKind kind;
                IIdentifier identifier;
                if (token.Parent is IAnonymousIdentifier)
                {
                    kind = IdentifierKind.Predicate;
                }
                else if ((identifier = token.GetParent<IIdentifier>(2) as IIdentifier) != null)
                {
                    kind = identifier.GetKind();
                }
                else
                {
                    if (token.Parent is IObjects || HasPrevSibling<IPredicate>(token))
                    {
                        kind = IdentifierKind.Object;
                    }
                    else if (token.Parent is IFacts || token.Parent is IIsOfExpression || HasPrevSibling<ISubject>(token))
                    {
                        kind = IdentifierKind.Predicate;
                    }
                    else
                    {
                        return EmptyList<string>.InstanceList;
                    }
                }

                if (kind == IdentifierKind.Object)
                {
                    list.AddRange(ObjectLiteralKeywords);
                }
                else if (kind == IdentifierKind.Predicate)
                {
                    var isOfExpression = token.Parent as IIsOfExpression;
                    if (isOfExpression != null && isOfExpression.IsKeyword != null && isOfExpression.Expression != null)
                    {
                        // is-of identifier continuation
                        list.Add(OfPredicateKeyword);
                    }
                    else
                    {
                        list.AddRange(PredicateKeywords);
                    }
                }
            }

            return list;
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

                prevSibling = prevSibling.PrevSibling;
            }

            return false;
        }

        private static readonly string[] PredicateKeywords = new[]
            {
                "a",
                "@has",
                "@is"
            };

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

        private static readonly string[] ObjectLiteralKeywords = new[]
            {
                "true",
                "false",
                "null"
            };

        private static readonly string[] MetaKeywords = new[]
            {
                "in",
                "@for",
                "out",
                "axis",
                "meta"
            };
    }
}
