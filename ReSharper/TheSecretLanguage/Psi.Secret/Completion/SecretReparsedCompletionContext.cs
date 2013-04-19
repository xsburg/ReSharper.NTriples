// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretReparsedCompletionContext.cs
// </summary>
// ***********************************************************************

using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    public class SecretReparsedCompletionContext : ReparsedCodeCompletionContext
    {
        public SecretReparsedCompletionContext([NotNull] IFile file, TreeTextRange range, string newText)
            : base(file, range, newText)
        {
        }

        protected override IReference FindReference(TreeTextRange referenceRange, ITreeNode treeNode)
        {
            return treeNode.FindReferencesAt(referenceRange).FirstOrDefault();
        }

        protected override IReparseContext GetReparseContext(IFile file, TreeTextRange range)
        {
            return new TrivialReparseContext(file, range);
        }
    }
}
