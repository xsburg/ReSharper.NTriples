// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   UnresolvedNamespacePrefixDeclaredElement.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Xml;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    internal class UnresolvedNamespacePrefixDeclaredElement : IDeclaredElement
    {
        private readonly IPsiSourceFile myFile;
        private readonly string myName;
        private readonly IPsiServices myServices;

        public UnresolvedNamespacePrefixDeclaredElement(IPsiSourceFile file, string name, IPsiServices services)
        {
            this.myFile = file;
            this.myName = name;
            this.myServices = services;
        }

        public bool CaseSensistiveName
        {
            get
            {
                return true;
            }
        }

        public PsiLanguageType PresentationLanguage
        {
            get
            {
                return SecretLanguage.Instance;
            }
        }

        public string ShortName
        {
            get
            {
                return this.myName;
            }
        }

        public IList<IDeclaration> GetDeclarations()
        {
            return EmptyList<IDeclaration>.InstanceList;
        }

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return EmptyList<IDeclaration>.InstanceList;
        }

        public DeclaredElementType GetElementType()
        {
            return SecretDeclaredElementType.Prefix;
        }

        public IPsiServices GetPsiServices()
        {
            return this.myServices;
        }

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            return new HybridCollection<IPsiSourceFile>
                {
                    this.myFile
                };
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
            return false;
        }

        public bool IsSynthetic()
        {
            return false;
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
