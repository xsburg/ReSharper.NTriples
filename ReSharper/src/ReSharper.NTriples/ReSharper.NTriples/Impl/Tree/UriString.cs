// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   UriString.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class UriString : IDeclaredElement, IUriIdentifierDeclaredElement
    {
        private NTriplesUriStringReference myUriStringReference;

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
                }#2#
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
                return this.GetText();
            }
        }

        public NTriplesUriStringReference UriStringReference
        {
            get
            {
                lock (this)
                {
                    return this.myUriStringReference ?? (this.myUriStringReference = new NTriplesUriStringReference(this));
                }
            }
        }

        public IList<IDeclaration> GetDeclarations()
        {
            return UriIdentifierDeclaredElement.GetDeclarations(this);
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return UriIdentifierDeclaredElement.GetDeclarationsIn(sourceFile, this);
        }

        public DeclaredElementType GetElementType()
        {
            return NTriplesDeclaredElementType.UriIdentifier;
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.UriStringReference);
        }

        public IdentifierKind GetKind()
        {
            var uriIdentifier = this.Parent as UriIdentifier;
            if (uriIdentifier == null)
            {
                return IdentifierKind.Other;
            }

            return uriIdentifier.GetKind();
        }

        public string GetLocalName()
        {
            var fullName = this.Value.GetText();
            var index = fullName.LastIndexOf('#');
            if (index == -1 || index == fullName.Length - 1)
            {
                return "";
            }

            return fullName.Substring(index + 1);
        }

        public TreeTextRange GetNameRange()
        {
            return this.GetTreeTextRange();
        }

        public string GetNamespace()
        {
            var fullName = this.Value.GetText();
            var index = fullName.LastIndexOf('#');
            if (index == -1)
            {
                return "";
            }

            return fullName.Substring(0, index + 1);
        }

        public string GetUri()
        {
            return this.Value == null
                       ? null
                       : this.Value.GetText();
        }

        public IdentifierInfo GetInfo()
        {
            var uriIdentifier = this.Parent as UriIdentifier;
            if (uriIdentifier == null)
            {
                return new IdentifierInfo(IdentifierKind.Other);
            }

            return uriIdentifier.GetInfo();
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
            PsiTreeUtil.ReplaceChild(this, this.Value, name);
        }

        public void SetReferenceName(string shortName)
        {
            this.UriStringReference.SetName(shortName);
        }

        private string GetDeclaredName()
        {
            return this.GetText();
        }
    }
}
