// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   Class1.cs
// </summary>
// ***********************************************************************

using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Secret;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Services;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [IntellisensePart]
    internal class SecretCodeCompletionContextProvider : CodeCompletionContextProviderBase
    {
        public override bool IsApplicable(CodeCompletionContext context)
        {
            var psiFile = context.File as SecretFile;
            return psiFile != null;
        }

        public override ISpecificCodeCompletionContext GetCompletionContext(CodeCompletionContext context)
        {
            var unterminatedContext = new SecretReparsedCompletionContext(context.File, context.SelectedTreeRange, "aaa");
            unterminatedContext.Init();
            IReference referenceToComplete = unterminatedContext.Reference;
            ITreeNode elementToComplete = unterminatedContext.TreeNode;
            if (elementToComplete == null)
            {
                return null;
            }

            TreeTextRange referenceRange = referenceToComplete != null ? referenceToComplete.GetTreeTextRange() : GetElementRange(elementToComplete);
            TextRange referenceDocumentRange = unterminatedContext.ToDocumentRange(referenceRange);
            if (!referenceDocumentRange.IsValid)
            {
                return null;
            }

            if (!referenceDocumentRange.Contains(context.CaretDocumentRange.TextRange))
            {
                return null;
            }

            TextLookupRanges ranges = GetTextLookupRanges(context, referenceDocumentRange);
            return new SecretCodeCompletionContext(context, ranges, unterminatedContext);
        }

        private static TreeTextRange GetElementRange(ITreeNode element)
        {
            var tokenNode = element as ITokenNode;

            if (tokenNode != null)
            {
                if (tokenNode.GetTokenType().IsIdentifier || tokenNode.GetTokenType().IsKeyword)
                {
                    return tokenNode.GetTreeTextRange();
                }
            }

            return new TreeTextRange(element.GetTreeTextRange().EndOffset);
        }
    }

    [Language(typeof(SecretLanguage))]
    public class PsiCompletionItemsProviderKeywords : ItemsProviderOfSpecificContext<SecretCodeCompletionContext>
    {
        protected override void AddItemsGroups(SecretCodeCompletionContext context, GroupedItemsCollector collector, IntellisenseManager intellisenseManager)
        {
            base.AddItemsGroups(context, collector, intellisenseManager);
            //collector.AddRanges(EvaluateRanges(context));
            //collector.AddFilter(new KeywordsBetterFilter());
        }

        protected override bool IsAvailable(SecretCodeCompletionContext context)
        {
            CodeCompletionType type = context.BasicContext.CodeCompletionType;
            return type == CodeCompletionType.AutomaticCompletion || type == CodeCompletionType.BasicCompletion;
        }

        private static TextLookupItemBase CreateKeyworkLookupItem(string x)
        {
            return new SecretKeywordLookupItem(x, GetSuffix());
        }

        private static string GetSuffix()
        {
            return " ";
        }

        protected override bool AddLookupItems(SecretCodeCompletionContext context, GroupedItemsCollector collector)
        {
            var secretFile = context.BasicContext.File as ISecretFile;
            if (secretFile == null)
            {
                return false;
            }

            var keywords = KeywordCompletionUtil.GetAplicableKeywords(secretFile, context.BasicContext.SelectedTreeRange).Select(CreateKeyworkLookupItem);
            foreach (TextLookupItemBase textLookupItem in keywords)
            {
                textLookupItem.InitializeRanges(context.Ranges, context.BasicContext);
                collector.AddAtDefaultPlace(textLookupItem);
            }

            return true;
        }

        /*private TextLookupRanges EvaluateRanges(ISpecificCodeCompletionContext context)
        {
            var file = context.BasicContext.File as IPsiFile;

            DocumentRange selectionRange = context.BasicContext.SelectedRange;

            if (file != null)
            {
                var token = file.FindNodeAt(selectionRange) as ITokenNode;

                if (token != null)
                {
                    DocumentRange tokenRange = token.GetNavigationRange();

                    var insertRange = new TextRange(tokenRange.TextRange.StartOffset, selectionRange.TextRange.EndOffset);
                    var replaceRange = new TextRange(tokenRange.TextRange.StartOffset, Math.Max(tokenRange.TextRange.EndOffset, selectionRange.TextRange.EndOffset));

                    return new TextLookupRanges(insertRange, false, replaceRange);
                }
            }
            return new TextLookupRanges(TextRange.InvalidRange, false, TextRange.InvalidRange);
        }*/
    }

    /*[Language(typeof(SecretLanguage))]
    internal class PsiCodeCompletionItemsProvider : ItemsProviderWithReference<SecretCodeCompletionContext, PsiReferenceBase, SecretFile>
    {
        protected override bool IsAvailable(SecretCodeCompletionContext context)
        {
            if (!((context.BasicContext.CodeCompletionType == CodeCompletionType.BasicCompletion) || (context.BasicContext.CodeCompletionType == CodeCompletionType.AutomaticCompletion)))
            {
                return false;
            }

            IReference reference = context.ReparsedContext.Reference;
            if (reference == null)
            {
                return false;
            }

            return true;
        }
    }*/

    public class SecretCodeCompletionContext : SpecificCodeCompletionContext
    {
        public SecretCodeCompletionContext(CodeCompletionContext context, TextLookupRanges completionRanges, SecretReparsedCompletionContext reparsedContext)
            : base(context)
        {
            ReparsedContext = reparsedContext;
            Ranges = completionRanges;
        }

        public TextLookupRanges Ranges { get; private set; }

        public SecretReparsedCompletionContext ReparsedContext { get; private set; }

        public override string ContextId
        {
            get { return "PsiSpecificContext"; }
        }
    }

    public class SecretReparsedCompletionContext : ReparsedCodeCompletionContext
    {
        public SecretReparsedCompletionContext([NotNull] IFile file, TreeTextRange range, string newText)
            : base(file, range, newText)
        {
        }

        protected override IReparseContext GetReparseContext(IFile file, TreeTextRange range)
        {
            return new TrivialReparseContext(file, range);
        }

        protected override IReference FindReference(TreeTextRange referenceRange, ITreeNode treeNode)
        {
            return treeNode.FindReferencesAt(referenceRange).FirstOrDefault();
        }
    }

    /*
    public abstract class PsiReferenceBase : TreeReferenceBase<ITreeNode>, ICompleteableReference
    {
        protected readonly ITreeNode TreeNode;
        protected string Name;

        protected PsiReferenceBase(ITreeNode node)
            : base(node)
        {
            TreeNode = node;
            Name = node.GetText();
        }

        #region ICompleteableReference Members

        public new ITreeNode GetTreeNode()
        {
            return TreeNode;
        }

        public override string GetName()
        {
            return Name;
        }

        public override IAccessContext GetAccessContext()
        {
            return new ElementAccessContext(myOwner);
        }

        public ISymbolTable GetCompletionSymbolTable()
        {
            return GetReferenceSymbolTable(false);
        }

        public override TreeTextRange GetTreeTextRange()
        {
            return new TreeTextRange(new TreeOffset(TreeNode.GetNavigationRange().TextRange.StartOffset), TreeNode.GetText().Length);
        }

        #endregion

        public override ResolveResultWithInfo ResolveWithoutCache()
        {
            ISymbolTable table = GetReferenceSymbolTable(true);
            IList<DeclaredElementInstance> elements = new List<DeclaredElementInstance>();
            {
                IList<ISymbolInfo> infos = table.GetSymbolInfos(GetName());
                foreach (ISymbolInfo info in infos)
                {
                    var element = new DeclaredElementInstance(info.GetDeclaredElement(), EmptySubstitution.INSTANCE);
                    elements.Add(element);
                }
            }
            return new ResolveResultWithInfo(ResolveResultFactory.CreateResolveResultFinaly(elements),
              ResolveErrorType.OK);
        }
    }*/
}