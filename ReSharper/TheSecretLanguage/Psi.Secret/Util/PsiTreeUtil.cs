// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiTreeUtil.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Util
{
    internal static class PsiTreeUtil
    {
        public static bool EqualsElements(ITreeNode treeNode1, ITreeNode treeNode2)
        {
            string s1 = GetTextWhithoutWhitespaces(treeNode1);
            string s2 = GetTextWhithoutWhitespaces(treeNode2);
            return s1.Equals(s2);
        }

        public static ICollection<T> GetAllChildren<T>(ITreeNode parent)
        {
            IList<T> list = new List<T>();
            GetAllChildren(parent, list);
            return list;
        }

        public static ITreeNode GetFirstChild<T>(ITreeNode element) where T : ITreeNode
        {
            if (element == null)
            {
                return null;
            }
            ITreeNode child = element.FirstChild;
            while (child != null)
            {
                if (child is T)
                {
                    return child;
                }

                ITreeNode result = GetFirstChild<T>(child);

                if (result != null)
                {
                    return result;
                }
                child = child.NextSibling;
            }

            return null;
        }

        /// <summary>Looks for a parent of specified type and returns null if the node is not found.</summary>
        /// <param name="element">The element to operate on.</param>
        /// <param name="maxDistance">The max distance to find. Use 0 to look only this item, 1 to look the direct parent only etc.</param>
        public static ITreeNode GetParent<T>(this ITreeNode element, int maxDistance)
        {
            int distance = 0;
            while (element != null && distance <= maxDistance)
            {
                if (element is T)
                {
                    return element;
                }

                element = element.Parent;
                distance++;
            }

            return null;
        }

        public static T GetParent<T>(this ITreeNode element) where T : class
        {
            while (element != null)
            {
                if (element is T)
                {
                    return element as T;
                }

                element = element.Parent;
            }

            return null;
        }

        public static bool HasParent<T>(ITreeNode element) where T : ITreeNode
        {
            while (element != null)
            {
                if (element is T)
                {
                    return true;
                }
                element = element.Parent;
            }
            return false;
        }

        public static void ReplaceChild(ITreeNode parent, ITreeNode nameNode, string name)
        {
            if (name.IsEmpty())
            {
                throw new ArgumentException("name shouldn't be empty", "name");
            }

            using (WriteLockCookie.Create(parent.IsPhysical()))
            {
                var srcIsUri = Uri.IsWellFormedUriString(name, UriKind.Absolute);
                var dstIsUri = parent is IUriString;
                if (srcIsUri && !dstIsUri)
                {
                    var uri = new Uri(name);
                    if (uri.Fragment.Length < 2)
                    {
                        throw new FormatException("The new name provided in rename operation is invalid.");
                    }

                    name = uri.Fragment.Substring(1);
                }
                else if (!srcIsUri && dstIsUri)
                {
                    if (parent.Parent == null)
                    {
                        throw new InvalidOperationException("The element being replaced doesn't have a parent.");
                    }

                    var uriIdentifier = (IUriIdentifier)parent.Parent;
                    var ns = uriIdentifier.GetNamespace();
                    name = ns + name;
                }

                ITreeNode newNode;
                if (parent is IPrefixName)
                {
                    newNode = SecretElementFactory.GetInstance(parent.GetPsiModule()).CreatePrefixNameExpression(name).FirstChild;
                }
                else if (parent is IPrefix)
                {
                    newNode = SecretElementFactory.GetInstance(parent.GetPsiModule()).CreatePrefixExpression(name).FirstChild;
                }
                else if (parent is ILocalName)
                {
                    newNode = SecretElementFactory.GetInstance(parent.GetPsiModule()).CreateLocalNameExpression(name).FirstChild;
                }
                else if (parent is IUriString)
                {
                    newNode = SecretElementFactory.GetInstance(parent.GetPsiModule()).CreateUriStringExpression(name).FirstChild;
                }
                else
                {
                    throw new NotSupportedException();
                }

                LowLevelModificationUtil.ReplaceChildRange(nameNode, nameNode, newNode);
            }
        }

        private static void GetAllChildren<T>(ITreeNode parent, ICollection<T> collection)
        {
            ITreeNode child = parent.FirstChild;
            while (child != null)
            {
                if (child is T)
                {
                    collection.Add((T)child);
                }
                GetAllChildren(child, collection);
                child = child.NextSibling;
            }
        }

        private static string GetTextWhithoutWhitespaces(ITreeNode treeNode)
        {
            string s = "";
            if (treeNode.FirstChild == null)
            {
                if (!(treeNode is Whitespace))
                {
                    s = s + treeNode.GetText();
                }
                return s;
            }
            ITreeNode child = treeNode.FirstChild;
            while (child != null)
            {
                if (!(child is Whitespace))
                {
                    s = s + GetTextWhithoutWhitespaces(child);
                }
                child = child.NextSibling;
            }
            return s;
        }
    }
}
