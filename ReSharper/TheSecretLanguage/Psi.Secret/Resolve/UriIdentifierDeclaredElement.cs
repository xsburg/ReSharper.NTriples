using System.Collections.Generic;
using System.Xml;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    internal class UriIdentifierDeclaredElement : IDeclaredElement
    {
        private readonly IFile myFile;
        private readonly IPsiServices myServices;
        private string myName;
        private string myNewName;

        public UriIdentifierDeclaredElement(IFile file, string name, IPsiServices services)
        {
            this.myFile = file;
            this.myName = name;
            this.myNewName = name;
            this.myServices = services;
        }

        public IFile File
        {
            get { return this.myFile; }
        }

        public bool ChangeName { get; set; }

        public string NewName
        {
            get { return this.myNewName; }
            set
            {
                this.ChangeName = true;
                this.myNewName = value;
            }
        }

        public IPsiServices GetPsiServices()
        {
            return this.myServices;
        }

        public IList<IDeclaration> GetDeclarations()
        {
            // TODO
            return EmptyList<IDeclaration>.InstanceList;
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            var secretFile = sourceFile.GetPsiFile<SecretLanguage>(new DocumentRange(sourceFile.Document, 0)) as SecretFile;
            if (secretFile == null)
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            return secretFile.GetSubjects();
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

        public bool IsValid()
        {
            return true;
        }

        public bool IsSynthetic()
        {
            return false;
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            return new HybridCollection<IPsiSourceFile> { this.myFile.GetSourceFile() };
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return sourceFile == this.myFile.GetSourceFile();
        }

        public string ShortName
        {
            get { return this.myName; }
        }

        public bool CaseSensistiveName
        {
            get { return true; }
        }

        public PsiLanguageType PresentationLanguage
        {
            get { return SecretLanguage.Instance; }
        }

        public void SetName(string name)
        {
            this.myName = name;
        }
    }
}