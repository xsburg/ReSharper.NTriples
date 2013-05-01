using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret.Cache;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Resolve
{
    public class SecretLocalNameReference : SecretReferenceBase
    {
        public SecretLocalNameReference(ITreeNode node)
            : base(node)
        {
        }

        public override IReference BindTo(IDeclaredElement element)
        {
            var localName = (ILocalName)this.GetTreeNode();
            if (localName.Parent != null)
            {
                PsiTreeUtil.ReplaceChild(localName, localName.FirstChild, element.ShortName);
                localName.SetReferenceName(element.ShortName);
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

            var localName = (LocalName)this.myOwner;
            var @namespace = localName.GetNamespace();
            var cache = this.myOwner.GetSolution().GetComponent<SecretCache>();
            if (@namespace != null)
            {
                var psiServices = this.myOwner.GetPsiServices();

                var elements = cache.GetAllUriIdentifiersInNamespace(@namespace)
                                    .Distinct(x => x.LocalName)
                                    .Select(x => new UriIdentifierDeclaredElement(file, x.Namespace, x.LocalName, x.Kind, psiServices));

                var symbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
                return symbolTable;
            }

            return EmptySymbolTable.INSTANCE;
        }

        public override ResolveResultWithInfo ResolveWithoutCache()
        {
            return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(new List<DeclaredElementInstance>()), ResolveErrorType.OK);
        }

        public ResolveResultWithInfo ResolveVirtualReferences()
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

            return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements), ResolveErrorType.OK);
        }

        public void SetName(string shortName)
        {
            this.Name = shortName;
        }
    }
}