// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   UriIdentifierDeclaredElement.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Impl.Tree;

namespace ReSharper.NTriples.Resolve
{
    internal class UriIdentifierDeclaredElement : IDeclaredElement, IUriIdentifierDeclaredElement
    {
        private readonly bool filterDeclarations;
        private readonly IdentifierKind kind;
        private readonly string localName;
        private readonly IFile myFile;

        private readonly string myName;
        private readonly IPsiServices myServices;
        private readonly string ns;
        private readonly IUriIdentifierDeclaredElement parent;
        private string myNewName;

        public UriIdentifierDeclaredElement(
            IFile file,
            string @namespace,
            string localName,
            IdentifierKind kind,
            IPsiServices services,
            bool filterDeclarations = false,
            IUriIdentifierDeclaredElement parent = null)
        {
            this.myFile = file;
            this.ns = @namespace;
            this.localName = localName;
            this.myName = localName;
            this.myNewName = localName;
            this.myServices = services;
            this.filterDeclarations = filterDeclarations;
            this.parent = parent;
            this.kind = kind;
        }

        public bool CaseSensistiveName
        {
            get
            {
                return true;
            }
        }

        public IFile File
        {
            get
            {
                return this.myFile;
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
                return this.myName;
            }
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

        public static IList<IDeclaration> GetDeclarations(IUriIdentifierDeclaredElement declaredElement)
        {
            var result = new List<IDeclaration>();
            foreach (var sourceFile in GetSourceFiles(declaredElement))
            {
                result.AddRange(GetDeclarationsIn(sourceFile, declaredElement));
            }

            return result;
        }

        public static IList<IDeclaration> GetDeclarationsIn(
            IPsiSourceFile sourceFile, IUriIdentifierDeclaredElement declaredElement)
        {
            var secretFile = sourceFile.GetPsiFile<NTriplesLanguage>(new DocumentRange(sourceFile.Document, 0)) as NTriplesFile;
            if (secretFile == null)
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            var uri = declaredElement.GetUri();
            if (string.IsNullOrEmpty(uri))
            {
                return EmptyList<IDeclaration>.InstanceList;
            }

            var uriIdentifiers = secretFile.GetUriIdentifiers(uri);
            return uriIdentifiers;
        }

        public static HybridCollection<IPsiSourceFile> GetSourceFiles(IUriIdentifierDeclaredElement declaredElement)
        {
            var cache = declaredElement.GetSolution().GetComponent<NTriplesCache>();
            return new HybridCollection<IPsiSourceFile>(
                cache.GetFilesContainingUri(declaredElement.GetNamespace(), declaredElement.GetLocalName()));
        }

        public override bool Equals(object obj)
        {
            var other = obj as IUriIdentifierDeclaredElement;
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.GetNamespace(), other.GetNamespace()) &&
                   string.Equals(this.GetLocalName(), other.GetLocalName());
        }

        public IList<IDeclaration> GetDeclarations()
        {
            if (this.parent != null)
            {
                return this.parent.GetDeclarations();
            }

            var result = new List<IDeclaration>();
            foreach (var sourceFile in this.GetSourceFiles())
            {
                result.AddRange(this.GetDeclarationsIn(sourceFile));
            }

            return result;
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            if (this.parent != null)
            {
                return this.parent.GetDeclarationsIn(sourceFile);
            }

            var declarations = GetDeclarationsIn(sourceFile, this);
            if (this.filterDeclarations)
            {
                return NTriplesIdentifierFilter.GetImportantSubjects(declarations).Cast<IDeclaration>().ToArray();
            }

            return declarations;
        }

        public DeclaredElementType GetElementType()
        {
            return NTriplesDeclaredElementType.UriIdentifier;
        }

        public override int GetHashCode()
        {
            return this.GetUri().GetHashCode();
        }

        public IdentifierKind GetKind()
        {
            return this.kind;
        }

        public string GetLocalName()
        {
            return this.localName;
        }

        public string GetNamespace()
        {
            return this.ns;
        }

        public IPsiServices GetPsiServices()
        {
            return this.myServices;
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            return GetSourceFiles(this);
        }

        public string GetUri()
        {
            return this.GetNamespace() + this.GetLocalName();
        }

        public XmlNode GetXMLDescriptionSummary(bool inherit)
        {
            return null;
        }

        public XmlNode GetXMLDoc(bool inherit)
        {
            return null;
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return this.GetSourceFiles().Contains(sourceFile);
        }

        public bool IsSynthetic()
        {
            return false;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.GetNamespace());
        }
    }
}
