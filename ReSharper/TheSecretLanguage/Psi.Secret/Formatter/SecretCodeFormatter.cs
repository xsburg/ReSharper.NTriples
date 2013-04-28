// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiCodeFormatter.cs
// </summary>
// ***********************************************************************

using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    [Language(typeof(SecretLanguage))]
    public class SecretCodeFormatter : CodeFormatterBase
    {
        private readonly SecretLanguage myLanguage;

        public SecretCodeFormatter(
            Lifetime lifetime, SecretLanguage language, ISettingsStore settingsStore, ISettingsOptimization settingsOptimization)
            : base(settingsStore)
        {
            this.myLanguage = language;
        }

        protected override PsiLanguageType LanguageType
        {
            get
            {
                return this.myLanguage;
            }
        }

        public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
        {
            return new ITreeNode[] { SecretFormatterHelper.CreateSpace(indent) };
        }

        public override ITreeRange Format(
            ITreeNode firstElement,
            ITreeNode lastElement,
            CodeFormatProfile profile,
            IProgressIndicator pi,
            IContextBoundSettingsStore overrideSettingsStore = null)
        {
            ITreeNode firstNode;
            ITreeNode lastNode;

            GetFirstAndLastNode(firstElement, lastElement, out firstNode, out lastNode);

            var contextBoundSettingsStore = this.GetProperContextBoundSettingsStore(overrideSettingsStore, firstNode);

            var globalSettings = GlobalFormatSettingsHelper
                .GetService(firstNode.GetSolution())
                .GetSettingsForLanguage(this.myLanguage, firstNode.GetProjectFileType(), contextBoundSettingsStore);

            using (pi.SafeTotal(4))
            {
                var context = new PsiCodeFormattingContext(this, firstNode, lastNode);
                if (profile != CodeFormatProfile.INDENT)
                {
                    using (IProgressIndicator subPi = pi.CreateSubProgress(2))
                    {
                        using (subPi.SafeTotal(2))
                        {
                            SecretFormattingStage.DoFormat(context, subPi.CreateSubProgress(1));
                            SecretIndentingStage.DoIndent(context, globalSettings, subPi.CreateSubProgress(1), false);
                        }
                    }
                }
                else
                {
                    using (IProgressIndicator subPi = pi.CreateSubProgress(4))
                    {
                        SecretIndentingStage.DoIndent(context, globalSettings, subPi, true);
                    }
                }
            }
            return new TreeRange(firstElement, lastElement);
        }

        public override void FormatDeletedNodes(ITreeNode parent, ITreeNode prevNode, ITreeNode nextNode)
        {
            this.Format(
                prevNode,
                nextNode,
                CodeFormatProfile.GENERATOR,
                null);
        }

        public override void FormatInsertedNodes(ITreeNode nodeFirst, ITreeNode nodeLast, bool formatSurround)
        {
            this.Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
        }

        public override ITreeRange FormatInsertedRange(ITreeNode nodeFirst, ITreeNode nodeLast, ITreeRange origin)
        {
            this.Format(nodeFirst, nodeLast, CodeFormatProfile.GENERATOR, null);
            return new TreeRange(nodeFirst, nodeLast);
        }

        public override void FormatReplacedNode(ITreeNode oldNode, ITreeNode newNode)
        {
            this.FormatInsertedNodes(newNode, newNode, false);
        }

        public override ITokenNode GetMinimalSeparator(ITokenNode leftToken, ITokenNode rightToken)
        {
            return SecretFormatterHelper.CreateSpace(" ");
        }

        public override bool IsWhitespaceToken(ITokenNode token)
        {
            return token.IsWhitespaceToken();
        }

        protected override bool IsFormatNextSpaces(CodeFormatProfile profile)
        {
            return profile != CodeFormatProfile.INDENT;
        }

        private static void GetFirstAndLastNode(
            ITreeNode firstElement, ITreeNode lastElement, out ITreeNode firstNode, out ITreeNode lastNode)
        {
            firstNode = firstElement;
            lastNode = lastElement;
            if (firstElement != lastElement)
            {
                if (firstElement == null)
                {
                    firstNode = lastElement;
                }
                ITreeNode commonParent = firstNode.FindCommonParent(lastNode);

                firstNode = GetFirstNode(firstNode, commonParent);
                lastNode = GetLastNode(lastNode, commonParent);
            }
            else
            {
                if (firstElement.FirstChild != null)
                {
                    firstNode = firstElement.FirstChild;
                    lastNode = firstElement.LastChild;
                }
            }
        }

        private static ITreeNode GetFirstNode(ITreeNode firstChild, ITreeNode commonParent)
        {
            while ((firstChild.Parent != null) && (firstChild.Parent != commonParent))
            {
                firstChild = firstChild.Parent;
            }

            ITreeNode firstNode = firstChild;
            while (firstNode.FirstChild != null)
            {
                firstNode = firstNode.FirstChild;
            }
            return firstNode;
        }

        private static ITreeNode GetLastNode(ITreeNode lastChild, ITreeNode commonParent)
        {
            while ((lastChild.Parent != null) && (lastChild.Parent != commonParent))
            {
                lastChild = lastChild.Parent;
            }

            ITreeNode lastNode = lastChild;
            while (lastNode.LastChild != null)
            {
                lastNode = lastNode.LastChild;
            }
            return lastNode;
        }
    }

    public class PsiCodeFormattingContext : CodeFormattingContext
    {
        public PsiCodeFormattingContext(SecretCodeFormatter secretCodeFormatter, ITreeNode firstNode, ITreeNode lastNode)
            : base(secretCodeFormatter, firstNode, lastNode)
        {
        }

        protected override bool CanModifyNode(ITreeNode element, NodeModificationType nodeModificationType)
        {
            return true;
        }
    }
}
