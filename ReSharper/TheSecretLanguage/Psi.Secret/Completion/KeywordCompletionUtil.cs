// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   KeywordCompletionUtil.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using IIdentifier = JetBrains.ReSharper.Psi.Secret.Tree.IIdentifier;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    internal static class KeywordCompletionUtil
    {
        public static IEnumerable<string> GetAplicableKeywords(ISecretFile file, TreeTextRange referenceRange)
        {
            var list = new List<string>();
            var token = file.FindNodeAt(referenceRange) as ITokenNode;
            if (token == null)
            {
                return list;
            }

            // identifier/literal_keywords
            var identifier = token.GetParent<IIdentifier>(2);
            if (identifier != null)
            {
                list.Add("true");
                list.Add("false");
                list.Add("null");
            }

            // predicate
            list.Add("a");
            // predicate/hasExpression
            list.Add("@has");
            // predicate/isOfExpression
            list.Add("@is");
            list.Add("@of");

            // meta
            list.Add("in");
            list.Add("@for");
            list.Add("out");
            list.Add("axis");
            list.Add("meta");

            // directives (starts directive, global scope)
            list.Add("@prefix");
            list.Add("@std_prefix");
            list.Add("@extension");
            list.Add("@using");
            list.Add("@axis-default");
            list.Add("@forAll");
            list.Add("@forSome");

            return list;
        }
    }
}
