using System.Collections.Generic;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ProjectModel;
using System.Linq;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public class SecretPrefixUriReference : SecretReferenceBase
    {
        public SecretPrefixUriReference(ITreeNode node)
            : base(node)
        {
        }

        public override IReference BindTo(IDeclaredElement element)
        {
            var namespacePrefix = (IPrefixUri)this.GetTreeNode();
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
            
            var psiServices = file.GetPsiServices();
            var cache = this.TreeNode.GetSolution().GetComponent<SecretCache>();
            var uriList = cache.GetAllPrefixDeclarationSymbols().Select(s => s.Uri).Distinct().ToArray();
            var elements = uriList.Select(u => new PrefixUriDeclaredElement(file, u, psiServices));
            return ResolveUtil.CreateSymbolTable(elements, 0);
        }

        public void SetName(string shortName)
        {
            this.Name = shortName;
        }
    }
}