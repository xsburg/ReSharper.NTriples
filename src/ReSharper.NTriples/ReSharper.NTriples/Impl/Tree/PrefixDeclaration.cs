// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   PrefixDeclaration.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class PrefixDeclaration : IDeclaredElement
    {
        public bool CaseSensistiveName
        {
            get
            {
                return true;
            }
        }

        public IDeclaredElement DeclaredElement
        {
            get
            {
                return this;
            }
        }

        public string DeclaredName
        {
            get
            {
                return this.GetDeclaredName();
            }
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
                    (INTriplesParser)
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
                    var psiFile = GetContainingNode<NTriplesFile>();
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
            get
            {
                return false;
            }
        }

        public PsiLanguageType PresentationLanguage
        {
            get
            {
                return NTriplesLanguage.Instance;
            }
        }

        public string ShortName
        {
            get
            {
                return this.PrefixName.GetText();
            }
        }

        public IList<IDeclaration> GetDeclarations()
        {
            var list = new List<IDeclaration>
                {
                    this
                };
            return list;
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            var list = new List<IDeclaration>
                {
                    this
                };
            return list;
        }

        public DeclaredElementType GetElementType()
        {
            return NTriplesDeclaredElementType.Prefix;
        }

        public TreeTextRange GetNameRange()
        {
            ITreeNode prefixName = this.PrefixName;
            int offset = prefixName.GetNavigationRange().TextRange.StartOffset;
            return new TreeTextRange(new TreeOffset(offset), this.GetDeclaredName().Length);
        }

        public XmlNode GetXMLDescriptionSummary(bool inherit)
        {
            return null;
        }

        public XmlNode GetXMLDoc(bool inherit)
        {
            return null;
        }

        public bool IsSynthetic()
        {
            return false;
        }

        public void SetName(string name)
        {
            PsiTreeUtil.ReplaceChild(this.PrefixName, this.PrefixName.FirstChild, name);
        }

        private string GetDeclaredName()
        {
            return this.PrefixName.GetText();
        }
    }
}
