using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Secret.Resolve;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Impl.Tree
{
    internal partial class UriString : IDeclaredElement, IUriIdentifierDeclaredElement
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

            var fullName = this.GetUri(secretFile);

            if (string.IsNullOrEmpty(fullName))
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            return secretFile.GetUriIdentifiers(fullName);
        }

        public string GetUri(ISecretFile file)
        {
            return Value.GetText();
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
                return this.GetText();
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
            PsiTreeUtil.ReplaceChild(this, this.Value, name);
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
                }#2#
            }

            return null;
        }*/

        #endregion

        public bool IsOpened
        {
            get { return false; }
        }

        private string GetDeclaredName()
        {
            return this.GetText();
        }

        private SecretUriStringReference myUriStringReference;

        public SecretUriStringReference UriStringReference
        {
            get
            {
                lock (this)
                {
                    return this.myUriStringReference ?? (this.myUriStringReference = new SecretUriStringReference(this));
                }
            }
        }

        public string GetNamespace()
        {
            var fullName = Value.GetText();
            var index = fullName.LastIndexOf('#');
            if (index == -1)
            {
                return "";
            }

            return fullName.Substring(0, index + 1);
        }

        public string GetLocalName()
        {
            var fullName = Value.GetText();
            var index = fullName.LastIndexOf('#');
            if (index == -1 || index == fullName.Length - 1)
            {
                return "";
            }

            return fullName.Substring(index + 1);
        }

        public override ReferenceCollection GetFirstClassReferences()
        {
            return new ReferenceCollection(this.UriStringReference);
        }

        public void SetReferenceName(string shortName)
        {
            this.UriStringReference.SetName(shortName);
        }
    }
}