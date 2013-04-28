using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    internal class UriIdentifierDeclaredElement : IDeclaredElement, IUriIdentifierDeclaredElement
    {
        private readonly IFile myFile;

        public string GetNamespace()
        {
            return this.ns;
        }

        public string GetLocalName()
        {
            return this.localName;
        }

        private readonly IPsiServices myServices;
        private readonly string myName;
        private readonly string localName;
        private string myNewName;
        private readonly string ns;

        public UriIdentifierDeclaredElement(IFile file, string @namespace, string localName, IPsiServices services)
        {
            this.myFile = file;
            this.ns = @namespace;
            this.localName = localName;
            this.myName = localName;
            this.myNewName = localName;
            this.myServices = services;
        }

        public IFile File
        {
            get { return this.myFile; }
        }

        /*public bool ChangeName { get; set; }

        public string NewName
        {
            get { return this.myNewName; }
            set
            {
                this.ChangeName = true;
                this.myNewName = value;
            }
        }*/

        public IPsiServices GetPsiServices()
        {
            return this.myServices;
        }

        public IList<IDeclaration> GetDeclarations()
        {
            var result = new List<IDeclaration>();
            foreach (var sourceFile in this.GetSourceFiles())
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

            var uriIdentifiers = secretFile.GetUriIdentifiers(GetUri());
            return uriIdentifiers;
        }

        public string GetUri()
        {
            return this.GetNamespace() + this.GetLocalName();
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

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.GetNamespace());
        }

        public bool IsSynthetic()
        {
            return false;
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            var cache = this.GetSolution().GetComponent<SecretCache>();
            return new HybridCollection<IPsiSourceFile>(cache.GetFilesContainingUri(this.GetNamespace(), this.GetLocalName()));
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return GetSourceFiles().Contains(sourceFile);
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

        /*public void SetName(string name)
        {
            this.myName = name;
        }*/

        public override bool Equals(object obj)
        {
            var other = obj as IUriIdentifierDeclaredElement;
            if (other == null)
            {
                return false;
            }

            return string.Equals(GetNamespace(), other.GetNamespace()) && string.Equals(GetLocalName(), other.GetLocalName());
        }

        public override int GetHashCode()
        {
            return this.GetUri().GetHashCode();
        }
    }
}