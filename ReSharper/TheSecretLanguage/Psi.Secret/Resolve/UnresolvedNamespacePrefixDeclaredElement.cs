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

        #region IDeclaredElement Members

        public IPsiServices GetPsiServices()
        {
            return this.myServices;
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
            return new HybridCollection<IPsiSourceFile> { this.myFile };
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return false;
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

        #endregion
    }
}