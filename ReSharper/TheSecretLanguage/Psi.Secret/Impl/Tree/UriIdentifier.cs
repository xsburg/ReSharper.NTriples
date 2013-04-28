using System.Collections.Generic;
using System.Xml;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using System.Linq;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class UriIdentifier
    {
        public UriIdentifierKind GetKind()
        {
            var kind = UriIdentifierKind.Other;
            var parent2 = this.Parent;
            while (parent2 != null && !(parent2 is ISentence) && !(parent2 is IAnonymousIdentifier))
            {
                if (parent2 is ISubject)
                {
                    kind = UriIdentifierKind.Subject;
                    break;
                }

                if (parent2 is IPredicate)
                {
                    kind = UriIdentifierKind.Predicate;
                    break;
                }

                if (parent2 is IObjects)
                {
                    kind = UriIdentifierKind.Object;
                    break;
                }

                parent2 = parent2.Parent;
            }

            return kind;
        }

        public IDeclaredElement DescendantDeclaredElement
        {
            get
            {
                return this.Prefix != null
                           ? this.LocalName.DeclaredElement
                           : this.UriStringElement.DeclaredElement;
            }
        }

        public string GetUri(ISecretFile file)
        {
            if (Prefix == null)
            {
                return UriString != null
                           ? UriString.GetText()
                           : null;
            }

            var declaration = ((SecretFile)file).GetDeclaredElements(Prefix.GetText()).FirstOrDefault() as IPrefixDeclaration;
            if (declaration != null && declaration.UriString != null)
            {
                return declaration.UriString.GetText() + this.GetLocalName();
            }

            return null;
        }

        public string GetLocalName()
        {
            return this.LocalName != null ? this.LocalName.GetText() : "";
        }

        public string GetNamespace(ISecretFile file)
        {
            if (Prefix == null)
            {
                if (UriString == null)
                {
                    return null;
                }

                var uri = UriString.GetText();
                var index = uri.LastIndexOf('#');
                if (index == -1)
                {
                    return uri;
                }

                return uri.Substring(0, index + 1);
            }

            var declaration = ((SecretFile)file).GetDeclaredElements(Prefix.GetText()).FirstOrDefault() as IPrefixDeclaration;
            if (declaration != null && declaration.UriString != null)
            {
                return declaration.UriString.GetText();
            }

            return null;
        }
    }

    /*internal partial class UriIdentifier : IDeclaredElement
    {
        #region IDeclaredElement Members

        public IList<IDeclaration> GetDeclarations()
        {
            var sourceFiles = this.GetSourceFiles();
            var result = new List<IDeclaration>();
            foreach (var sourceFile in sourceFiles)
            {
                result.AddRange(this.GetDeclarationsIn(sourceFile));
            }

            return result;
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            var secretFile = sourceFile.GetPsiFile<SecretLanguage>(new DocumentRange(sourceFile.Document, 0)) as SecretFile;
            if (secretFile == null)
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            var fullName = GetFullName(secretFile);

            if (string.IsNullOrEmpty(fullName))
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            return secretFile.GetUriIdentifiers(fullName);
        }

        public string GetUri(ISecretFile file)
        {
            if (Prefix == null)
            {
                return UriString.GetText();
            }

            var declaration = ((SecretFile)file).GetDeclaredElements(Prefix.GetText()).FirstOrDefault() as IPrefixDeclaration;
            if (declaration != null)
            {
                return declaration.UriString.GetText() + LocalName.GetText();
            }

            return null;
        }

        public DeclaredElementType GetElementType()
        {
            return SecretDeclaredElementType.UriIdentifier;
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
            get
            {
                return Prefix != null
                           ? LocalName.GetText()
                           : UriString.GetText();
            }
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
            if (Prefix != null)
            {
                PsiTreeUtil.ReplaceChild(LocalName, LocalName.FirstChild, name);
            }
            else
            {
                PsiTreeUtil.ReplaceChild(this, this.UriString, name);
            }
        }

        public TreeTextRange GetNameRange()
        {
            return this.GetTreeTextRange();
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
            get { return this.GetDeclaredName(); }
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
                }#3#
            }

            return null;
        }#1#

        #endregion

        public UriIdentifierKind GetKind()
        {
            var kind = UriIdentifierKind.Other;
            var parent2 = this.Parent;
            while (parent2 != null && !(parent2 is ISentence) && !(parent2 is IAnonymousIdentifier))
            {
                if (parent2 is ISubject)
                {
                    kind = UriIdentifierKind.Subject;
                    break;
                }

                if (parent2 is IPredicate)
                {
                    kind = UriIdentifierKind.Predicate;
                    break;
                }

                if (parent2 is IObjects)
                {
                    kind = UriIdentifierKind.Object;
                    break;
                }

                parent2 = parent2.Parent;
            }

            return kind;
        }

        public bool IsOpened
        {
            get { return false; }
        }

        private string GetDeclaredName()
        {
            return Prefix != null ? LocalName.GetText() : UriString.GetText();
        }

        private SecretUriIdentifierReference myUriIdentifierReference;

        public SecretUriIdentifierReference UriIdentifierReference
        {
            get
            {
                lock (this)
                {
                    return this.myUriIdentifierReference ?? (this.myUriIdentifierReference = new SecretUriIdentifierReference(this));
                }
            }
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.UriIdentifierReference);
        }

        public void SetReferenceName(string shortName)
        {
            this.UriIdentifierReference.SetName(shortName);
        }
    }*/
}