using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Parsing;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class PrefixDeclaration : IDeclaredElement
    {
        #region IDeclaredElement Members

        public IList<IDeclaration> GetDeclarations()
        {
            var list = new List<IDeclaration> { this };
            return list;
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            var list = new List<IDeclaration> { this };
            return list;
        }

        public DeclaredElementType GetElementType()
        {
            return SecretDeclaredElementType.Prefix;
        }

        public XmlNode GetXMLDoc(bool inherit)
        {
            return null;
        }

        public XmlNode GetXMLDescriptionSummary(bool inherit)
        {
            return null;
        }

        public bool IsSynthetic()
        {
            return false;
        }

        public string ShortName
        {
            get { return PrefixName.GetText().TrimEnd(':'); }
        }

        public bool CaseSensistiveName
        {
            get { return true; }
        }

        public PsiLanguageType PresentationLanguage
        {
            get { return SecretLanguage.Instance; }
        }

        #endregion

        #region IRuleDeclaration Members

        public void SetName(string name)
        {
            PsiTreeUtil.ReplaceChild(PrefixName, PrefixName.FirstChild, name);
        }

        public TreeTextRange GetNameRange()
        {
            ITreeNode prefixName = PrefixName;
            int offset = prefixName.GetNavigationRange().TextRange.StartOffset;
            return new TreeTextRange(new TreeOffset(offset), GetDeclaredName().Length);
        }

        public IDeclaredElement DeclaredElement
        {
            get { return this; }
        }

        public string DeclaredName
        {
            get { return GetDeclaredName(); }
        }

        /*public IChameleonNode ReSync(CachingLexer cachingLexer, TreeTextRange changedRange, int insertedTextLen)
        {
            TreeOffset currStartOffset = GetTreeStartOffset();
            int currLength = GetTextLength();

            Logger.Assert(changedRange.StartOffset >= currStartOffset && changedRange.EndOffset <= (currStartOffset + currLength),
              "changedRange.StartOffset >= currStartOffset && changedRange.EndOffset <= (currStartOffset+currLength)");

            int newLength = currLength - changedRange.Length + insertedTextLen;

            LanguageService languageService = Language.LanguageService();
            if (languageService != null)
            {
                var parser =
                    (ISecretParser)
                    languageService.CreateParser(
                        new ProjectedLexer(cachingLexer, new TextRange(currStartOffset.Offset, currStartOffset.Offset + newLength)),
                        GetPsiModule(),
                        GetSourceFile());
                throw new NotImplementedException();
                /*TreeElement newElement = parser.ParseStatement();
                if (newElement.GetTextLength() == 0)
                {
                    return null;
                }
                if ((newElement.GetTextLength() == newLength) && (";".Equals(newElement.GetText().Substring(newElement.GetTextLength() - 1))))
                {
                    var psiFile = GetContainingNode<SecretFile>();
                    if (psiFile != null)
                    {
                        psiFile.ClearTables();
                    }
                    return newElement as IRuleDeclaration;
                }#1#
            }

            return null;
        }*/

        public bool IsOpened
        {
            get { return false; }
        }

        #endregion

        private string GetDeclaredName()
        {
            return PrefixName.GetText().TrimEnd(':');
        }
    }
}