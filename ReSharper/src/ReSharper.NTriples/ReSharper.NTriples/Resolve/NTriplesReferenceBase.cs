// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretReferenceBase.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.NTriples.Resolve
{
    public abstract class NTriplesReferenceBase : TreeReferenceBase<ITreeNode>, ICompleteableReference
    {
        protected readonly ITreeNode TreeNode;
        protected string Name;

        protected NTriplesReferenceBase(ITreeNode node)
            : base(node)
        {
            this.TreeNode = node;
            this.Name = node.GetText();
        }

        public override IAccessContext GetAccessContext()
        {
            return new ElementAccessContext(this.myOwner);
        }

        public ISymbolTable GetCompletionSymbolTable()
        {
            return this.GetReferenceSymbolTable(false);
        }

        public override string GetName()
        {
            return this.Name;
        }

        public new ITreeNode GetTreeNode()
        {
            return this.TreeNode;
        }

        public override TreeTextRange GetTreeTextRange()
        {
            return new TreeTextRange(
                new TreeOffset(this.TreeNode.GetNavigationRange().TextRange.StartOffset), this.TreeNode.GetText().Length);
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

            return new ResolveResultWithInfo(
                ResolveResultFactory.CreateResolveResultFinaly(elements),
                ResolveErrorType.OK);
        }
    }
}
