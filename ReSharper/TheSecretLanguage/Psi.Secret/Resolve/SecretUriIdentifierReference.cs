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
    public class SecretUriIdentifierReference : SecretReferenceBase
    {
        public SecretUriIdentifierReference(ITreeNode node)
            : base(node)
        {
        }

        public override IReference BindTo(IDeclaredElement element)
        {
            var localName = (IUriIdentifier)this.GetTreeNode();
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
            
            var cache = this.myOwner.GetSolution().GetComponent<SecretCache>();
            var uriIdentifier = this.myOwner.GetParent<IUriIdentifier>();
            var prefix = uriIdentifier.Prefix.GetText();
            var prefixDeclarations = file.GetDeclaredElements(prefix).FirstOrDefault();
            if (prefixDeclarations != null)
            {
                var prefixDeclaration = prefixDeclarations.GetDeclarationsIn(this.myOwner.GetSourceFile()).FirstOrDefault() as PrefixDeclaration;
                if (prefixDeclaration != null)
                {
                    var @namespace = prefixDeclaration.NamespaceUri;
                    var psiServices = this.myOwner.GetPsiServices();

                    var elements = cache.GetAllUriIdentifiersInNamespace(@namespace)
                                        .Select(x => new UriIdentifierDeclaredElement(file, x.LocalName, psiServices));
                    
                    var symbolTable = ResolveUtil.CreateSymbolTable(elements, 0);
                    return symbolTable;
                }
            }

            return EmptySymbolTable.INSTANCE;
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

            return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements), ResolveErrorType.OK);
        }

        public void SetName(string shortName)
        {
            this.Name = shortName;
        }
    }
}