// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretPrefixReference.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Impl.Tree;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Resolve
{
    public class SecretPrefixReference : SecretReferenceBase
    {
        public SecretPrefixReference(ITreeNode node)
            : base(node)
        {
        }

        public override IReference BindTo(IDeclaredElement element)
        {
            var namespacePrefix = (IPrefix)this.GetTreeNode();
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

            return file.FilePrefixesSymbolTable;
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
                var ruleName = this.myOwner as Prefix;
                // Unresolved namespaces creation
                if (ruleName != null)
                {
                    elements = new List<DeclaredElementInstance>
                        {
                            new DeclaredElementInstance(
                                new UnresolvedNamespacePrefixDeclaredElement(
                                    ruleName.GetSourceFile(), this.GetName(), this.myOwner.GetPsiServices()))
                        };
                }
            }

            return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements), ResolveErrorType.OK);
        }

        public void SetName(string shortName)
        {
            this.Name = shortName;
        }
    }
}
