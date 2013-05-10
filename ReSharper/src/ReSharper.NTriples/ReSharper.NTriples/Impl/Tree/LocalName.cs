// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   LocalName.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Resolve;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class LocalName : IUriIdentifierDeclaredElement
    {
        private readonly IList<IPsiSourceFile> filesScope = new List<IPsiSourceFile>();
        private NTriplesLocalNameReference myLocalNameReference;

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

        public NTriplesLocalNameReference LocalNameReference
        {
            get
            {
                lock (this)
                {
                    return this.myLocalNameReference ?? (this.myLocalNameReference = new NTriplesLocalNameReference(this));
                }
            }
        }

        public PsiLanguageType PresentationLanguage
        {
            get
            {
                return NTriplesLanguage.Instance;
            }
        }

        public bool ScopeToMainFile { get; set; }

        public string ShortName
        {
            get
            {
                return this.GetText();
            }
        }

        public IList<IDeclaration> GetDeclarations()
        {
            if (this.ScopeToMainFile)
            {
                return UriIdentifierDeclaredElement.GetDeclarationsIn(this.GetSourceFile(), this);
            }

            return UriIdentifierDeclaredElement.GetDeclarations(this);
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            if (this.filesScope.Contains(sourceFile) && this.GetSourceFile() != sourceFile)
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            return UriIdentifierDeclaredElement.GetDeclarationsIn(sourceFile, this);
        }

        public DeclaredElementType GetElementType()
        {
            return NTriplesDeclaredElementType.UriIdentifier;
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.LocalNameReference);
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
            return this.GetText();
        }

        public TreeTextRange GetNameRange()
        {
            return this.GetTreeTextRange();
        }

        public string GetNamespace()
        {
            var uriIdentifier = this.Parent as UriIdentifier;
            if (uriIdentifier == null)
            {
                return null;
            }

            var prefixElement = uriIdentifier.Prefix;
            if (prefixElement == null)
            {
                return uriIdentifier.GetUri();
            }

            var prefix = prefixElement.PrefixReference.GetName();
            var secretFile = (NTriplesFile)this.GetContainingFile();
            if (secretFile == null)
            {
                return prefix;
            }

            var declaration = secretFile.GetPrefixDeclaredElements(prefix).FirstOrDefault() as IPrefixDeclaration;
            if (declaration != null && declaration.UriString != null)
            {
                return declaration.UriString.GetText();
            }

            return prefix;
        }

        public string GetUri()
        {
            var ns = this.GetNamespace();
            if (ns == null)
            {
                return null;
            }

            return ns + this.GetText();
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
            PsiTreeUtil.ReplaceChild(this, this.FirstChild, name);
        }

        public void SetReferenceName(string shortName)
        {
            this.LocalNameReference.SetName(shortName);
        }

        private string GetDeclaredName()
        {
            return this.GetText();
        }
    }
}
