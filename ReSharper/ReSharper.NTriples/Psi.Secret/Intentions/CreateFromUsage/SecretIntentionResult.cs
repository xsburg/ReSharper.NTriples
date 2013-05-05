// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretIntentionResult.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Application;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.DocumentModel;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Intentions.Impl.TemplateFieldHolders;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Intentions.CreateFromUsage
{
    public class SecretIntentionResult
    {
        private readonly ITreeNode myAnchor;
        private readonly IDeclaration myDeclaration;
        private readonly List<ITemplateFieldHolder> myHolders;
        private readonly DocumentRange myPreferredSelection;

        public SecretIntentionResult(
            List<ITemplateFieldHolder> holders, IDeclaration declaration, ITreeNode anchor, DocumentRange range)
        {
            this.myDeclaration = declaration;
            this.myHolders = holders;
            this.myPreferredSelection = range;
            this.myAnchor = anchor;
        }

        public DocumentRange PreferredSelection
        {
            get
            {
                return this.myPreferredSelection;
            }
        }

        public IDeclaration ResultDeclaration
        {
            get
            {
                return this.myDeclaration;
            }
        }

        public void ExecuteTemplate()
        {
            IDeclaration newDeclaration = this.myDeclaration;
            newDeclaration.AssertIsValid();

            ISolution solution = newDeclaration.GetPsiModule().GetSolution();

            Debug.Assert(Shell.Instance.Invocator != null, "Shell.Instance.Invocator != null");
            Shell.Instance.Invocator.Dispatcher.AssertAccess();

            Assertion.Assert(!PsiManager.GetInstance(solution).HasActiveTransaction, "PSI transaction is active");
            solution.GetComponent<SolutionDocumentTransactionManager>().AssertNotUnderTransaction();

            IFile file = this.myAnchor.GetContainingFile();
            Assertion.Assert(file != null, "fileFullName!= null");
            var item = file.GetSourceFile().ToProjectFile();

            var infos = GetFieldInfos(newDeclaration, this.myHolders);

            var textControl = EditorManager.GetInstance(solution).OpenProjectFile(item, true);
            if (textControl == null)
            {
                if (Shell.Instance.IsInInternalMode || Shell.Instance.IsTestShell)
                {
                    Logger.Fail("textControl != null");
                }
                return;
            }

            if (infos.Length > 0)
            {
                HotspotSession hotspotSession = LiveTemplatesManager.Instance.CreateHotspotSessionAtopExistingText(
                    solution,
                    TextRange.InvalidRange,
                    textControl,
                    LiveTemplatesManager.EscapeAction.LeaveTextAndCaret,
                    infos);
                hotspotSession.Execute();
            }

            Shell.Instance.GetComponent<SecretIntentionResultBehavior>().OnHotspotSessionExecutionStarted(this, textControl);
        }

        private static HotspotInfo[] GetFieldInfos(IDeclaration declaration, IEnumerable<ITemplateFieldHolder> templateArguments)
        {
            return
                templateArguments.Select(t => t.GetInfo(declaration)).Where(hotspotInfo => hotspotInfo.Ranges.Any()).ToArray();
        }
    }
}
