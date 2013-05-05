using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperSecretLanguage
{
    [Language(typeof(N3Language))]
    public class N3CodeFormatter : CodeFormatterBase
    {
        private readonly N3Language myLanguage;

        public N3CodeFormatter(Lifetime lifetime, N3Language language, ISettingsStore settingsStore, ISettingsOptimization settingsOptimization)
            : base(settingsStore)
        {
            this.myLanguage = language;
        }

        protected override PsiLanguageType LanguageType
        {
            get { return this.myLanguage; }
        }

        public override bool IsWhitespaceToken(ITokenNode token)
        {
            return token.IsWhitespaceToken();
        }

        protected override bool IsFormatNextSpaces(CodeFormatProfile profile)
        {
            return profile != CodeFormatProfile.INDENT;
        }

        public override ITokenNode GetMinimalSeparator(ITokenNode leftToken, ITokenNode rightToken)
        {
            return leftToken;// PsiFormatterHelper.CreateSpace(" ");
        }

        public override ITreeNode[] CreateSpace(string indent, ITreeNode rightNonSpace, ITreeNode replacedSpace)
        {
            return new ITreeNode[] { replacedSpace/*PsiFormatterHelper.CreateSpace(indent)*/ };
        }

        public override ITreeRange Format(ITreeNode firstElement, ITreeNode lastElement, CodeFormatProfile profile, IProgressIndicator pi, IContextBoundSettingsStore overrideSettingsStore = null)
        {
            return null;
        }

        private static void GetFirstAndLastNode(ITreeNode firstElement, ITreeNode lastElement, out ITreeNode firstNode, out ITreeNode lastNode)
        {
            firstNode = firstElement;
            lastNode = lastElement;
        }

        private static ITreeNode GetLastNode(ITreeNode lastChild, ITreeNode commonParent)
        {
            return lastChild;
        }

        private static ITreeNode GetFirstNode(ITreeNode firstChild, ITreeNode commonParent)
        {
            return firstChild;
        }

        public override void FormatInsertedNodes(ITreeNode nodeFirst, ITreeNode nodeLast, bool formatSurround)
        {
        }

        public override ITreeRange FormatInsertedRange(ITreeNode nodeFirst, ITreeNode nodeLast, ITreeRange origin)
        {
            return origin;
        }

        public override void FormatReplacedNode(ITreeNode oldNode, ITreeNode newNode)
        {
        }

        public override void FormatDeletedNodes(ITreeNode parent, ITreeNode prevNode, ITreeNode nextNode)
        {
        }
    }
}