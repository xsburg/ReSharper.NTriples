// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesPrefixUriReference.cs
// </summary>
// ***********************************************************************

using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Impl.Tree;
using ReSharper.NTriples.Tree;
using ReSharper.NTriples.Util;

namespace ReSharper.NTriples.Resolve
{
    public class NTriplesPrefixUriReference : NTriplesReferenceBase
    {
        public NTriplesPrefixUriReference(ITreeNode node)
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
            var file = this.TreeNode.GetContainingFile() as NTriplesFile;
            if (file == null)
            {
                return EmptySymbolTable.INSTANCE;
            }

            var psiServices = file.GetPsiServices();
            var cache = this.TreeNode.GetSolution().GetComponent<NTriplesCache>();
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
