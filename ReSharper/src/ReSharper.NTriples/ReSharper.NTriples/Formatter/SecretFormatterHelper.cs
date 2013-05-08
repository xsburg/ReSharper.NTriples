// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretFormatterHelper.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Parsing;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    public static class SecretFormatterHelper
    {
        public static IWhitespaceNode CreateNewLine(string text)
        {
            return new NewLine(text);
        }

        public static IWhitespaceNode CreateSpace(string spaceText)
        {
            return new Whitespace(spaceText);
        }

        public static bool HasLineFeedsTo(this ITreeNode fromNode, ITreeNode toNode)
        {
            return fromNode.GetLineFeedsTo(toNode).Any();
        }

        public static void MakeIndent(this ITreeNode indentNode, string indent)
        {
            var lastSpace = indentNode.PrevSibling as IWhitespaceNode;
            if (lastSpace != null && lastSpace.GetTokenType() != SecretTokenType.NEW_LINE)
            {
                ITreeNode firstSpace =
                    lastSpace.LeftWhitespaces().TakeWhile(ws => ws.GetTokenType() != SecretTokenType.NEW_LINE).LastOrDefault() ??
                    lastSpace;
                Debug.Assert(firstSpace != null, "firstSpace != null");
                if (firstSpace != lastSpace)
                {
                    while ((firstSpace != null) && (firstSpace.GetTokenType() != SecretTokenType.NEW_LINE) &&
                           (firstSpace.GetNextToken() != lastSpace))
                    {
                        firstSpace = firstSpace.GetNextToken();
                    }
                    firstSpace = firstSpace.GetNextToken();
                }
                if (firstSpace != null)
                {
                    if ((firstSpace != lastSpace || lastSpace.GetText() != indent) && (firstSpace.Parent == lastSpace.Parent))
                    {
                        if (indent.IsEmpty())
                        {
                            LowLevelModificationUtil.DeleteChildRange(firstSpace, lastSpace);
                        }
                        else
                        {
                            LowLevelModificationUtil.ReplaceChildRange(firstSpace, lastSpace, CreateSpace(indent));
                        }
                    }
                }
            }
            else if (!indent.IsEmpty())
            {
                LowLevelModificationUtil.AddChildBefore(indentNode, CreateSpace(indent));
            }
        }

        public static void ReplaceSpaces(ITreeNode leftNode, ITreeNode rightNode, IEnumerable<string> wsTexts)
        {
            if (wsTexts == null)
            {
                return;
            }
            FormatterImplHelper.ReplaceSpaces(leftNode, rightNode, wsTexts.CreateWhitespaces().Cast<ITreeNode>().ToArray());
        }

        [NotNull]
        private static IWhitespaceNode[] CreateWhitespaces([NotNull] this IEnumerable<string> wsTexts)
        {
            if (wsTexts == null)
            {
                throw new ArgumentNullException("wsTexts");
            }

            return wsTexts.Where(text => !text.IsEmpty()).Select(
                text =>
                {
                    if (text.IsNewLine())
                    {
                        return CreateNewLine("\r\n");
                    }
                    // consistency check (remove in release?)
                    if (!SecretLexer.IsWhitespace(text))
                    {
                        throw new ApplicationException("Inconsistent space structure");
                    }
                    return CreateSpace(text);
                }).ToArray();
        }

        private static IEnumerable<IWhitespaceNode> GetLineFeedsTo(this ITreeNode fromNode, ITreeNode toNode)
        {
            return
                fromNode.GetWhitespacesTo(toNode)
                        .Where(wsNode => (wsNode.GetTokenType() == SecretTokenType.NEW_LINE) && (wsNode is IWhitespaceNode))
                        .Cast<IWhitespaceNode>();
        }
    }
}
