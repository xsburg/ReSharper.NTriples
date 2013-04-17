// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretNamespacePrefixReference.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public class SecretNamespacePrefixReference : SecretReferenceBase
    {
        public SecretNamespacePrefixReference(ITreeNode node)
            : base(node)
        {
        }

        public override IReference BindTo(IDeclaredElement element)
        {
            var namespacePrefix = (INamespacePrefix)this.GetTreeNode();
            if (namespacePrefix.Parent != null)
            {
                PsiTreeUtil.ReplaceChild(namespacePrefix, namespacePrefix.FirstChild, element.ShortName);
                namespacePrefix.SetName(element.ShortName);
            }
            return this;
        }

        public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
        {
            return this.BindTo(element);
        }

        public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
        {
            var file = this.TreeNode.GetContainingFile() as SecretFile;
            if (file == null)
            {
                return EmptySymbolTable.INSTANCE;
            }
            return file.FileNamespacePrefixSymbolTable;
        }

        public override ResolveResultWithInfo ResolveWithoutCache()
        {
            ISymbolTable table = this.GetReferenceSymbolTable(true);
            IList<DeclaredElementInstance> elements = new List<DeclaredElementInstance>();
            {
                IList<ISymbolInfo> infos = table.GetSymbolInfos(this.GetName());
                foreach (ISymbolInfo info in infos)
                {
                    var element = new DeclaredElementInstance(info.GetDeclaredElement(), EmptySubstitution.INSTANCE);
                    elements.Add(element);
                }
            }
            if (elements.Count == 0)
            {
                var ruleName = this.myOwner as NamespacePrefix;
                // Unresolved namespaces creation
                /* if( PsiTreeUtil.HasParent<InterfacesDefinition>(this.myOwner) && (ruleName != null))
        {
          elements = new List<DeclaredElementInstance>{new DeclaredElementInstance(new UnresolvedRuleInterfacesDeclaredElement(ruleName.GetSourceFile(), this.GetName(), this.myOwner.GetPsiServices()))};
        }*/
            }

            return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements), ResolveErrorType.OK);
        }

        public void SetName(string shortName)
        {
            this.Name = shortName;
        }
    }
}
