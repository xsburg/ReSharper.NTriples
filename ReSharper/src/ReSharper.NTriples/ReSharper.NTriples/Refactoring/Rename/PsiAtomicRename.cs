// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiAtomicRename.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.Refactorings.Conflicts;
using JetBrains.ReSharper.Refactorings.Conflicts.New;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using JetBrains.ReSharper.Refactorings.Util;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.Util;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Impl.Tree;
using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Refactoring.Rename
{
    internal class PsiAtomicRename : AtomicRenameBase
    {
        private readonly List<IDeclaration> myDeclarations = new List<IDeclaration>();
        private readonly bool myDoNotShowBindingConflicts;
        private readonly IDeclaredElement myElement;
        private readonly string myNewName;

        private readonly List<IReference> myNewReferences = new List<IReference>();
        private readonly IDeclaredElementPointer<IDeclaredElement> myOriginalElementPointer;

        [CanBeNull]
        private readonly List<IDeclaredElementPointer<IDeclaredElement>> mySecondaryElements;

        private IDeclaredElementPointer<IDeclaredElement> myNewElementPointer;

        public PsiAtomicRename(IDeclaredElement declaredElement, [NotNull] string newName, bool doNotShowBindingConflicts)
        {
            this.myOriginalElementPointer = declaredElement.CreateElementPointer();
            this.myNewName = newName;
            this.myDoNotShowBindingConflicts = doNotShowBindingConflicts;
            this.myElement = declaredElement;
            this.mySecondaryElements = new List<IDeclaredElementPointer<IDeclaredElement>>();
            this.mySecondaryElements =
                RenameRefactoringService.Instance.GetRenameService(NTriplesLanguage.Instance)
                                        .GetSecondaryElements(declaredElement)
                                        .Select(x => x.CreateElementPointer())
                                        .ToList();
            this.BuildDeclarations();
        }


        public override IDeclaredElement NewDeclaredElement
        {
            get
            {
                return this.myNewElementPointer.FindDeclaredElement();
            }
        }

        public override string NewName
        {
            get
            {
                return this.myNewName;
            }
        }

        public override IDeclaredElement PrimaryDeclaredElement
        {
            get
            {
                return this.myOriginalElementPointer.FindDeclaredElement();
            }
        }

        [NotNull]
        public override IList<IDeclaredElement> SecondaryDeclaredElements
        {
            get
            {
                if (this.mySecondaryElements == null)
                {
                    return EmptyList<IDeclaredElement>.InstanceList;
                }
                return this.mySecondaryElements.SelectNotNull(x => x.FindDeclaredElement()).ToList();
            }
        }

        public override void Rename(
            RenameRefactoring executer, IProgressIndicator pi, bool hasConflictsWithDeclarations, IRefactoringDriver driver)
        {
            this.BuildDeclarations();

            //Logger.Assert(myDeclarations.Count > 0, "myDeclarations.Count > 0");

            IDeclaredElement declaredElement = this.myOriginalElementPointer.FindDeclaredElement();
            if (declaredElement == null)
            {
                return;
            }

            IPsiServices psiServices = declaredElement.GetPsiServices();

            IList<IReference> primaryReferences = executer.Workflow.GetElementReferences(this.PrimaryDeclaredElement);
            List<Pair<IDeclaredElement, IList<IReference>>> secondaryElementWithReferences =
                this.SecondaryDeclaredElements.Select(x => Pair.Of(x, executer.Workflow.GetElementReferences(x))).ToList();
            pi.Start(this.myDeclarations.Count + primaryReferences.Count);

            foreach (IDeclaration declaration in this.myDeclarations)
            {
                InterruptableActivityCookie.CheckAndThrow(pi);
                declaration.SetName(this.myNewName);
                pi.Advance();
            }

            psiServices.PsiManager.UpdateCaches();

            IDeclaredElement newDeclaredElement;
            if (this.myDeclarations.Count > 0)
            {
                newDeclaredElement = this.myDeclarations[0].DeclaredElement;
            }
            else
            {
                if (this.myElement is PrefixDeclaredElement)
                {
                    newDeclaredElement = this.myElement;
                    ((PrefixDeclaredElement)this.myElement).ChangeName = true;
                    ((PrefixDeclaredElement)this.myElement).NewName = this.NewName;
                    ((PrefixDeclaredElement)newDeclaredElement).SetName(this.NewName);
                }
                else
                {
                    newDeclaredElement = null;
                }
            }
            Assertion.Assert(newDeclaredElement != null, "The condition (newDeclaredElement != null) is false.");

            this.myNewElementPointer = newDeclaredElement.CreateElementPointer();
            Assertion.Assert(newDeclaredElement.IsValid(), "myNewDeclaredElement.IsValid()");


            this.myNewReferences.Clear();
            OneToSetMap<PsiLanguageType, IReference> references =
                LanguageUtil.SortReferences(primaryReferences.Where(x => x.IsValid()));
            IList<IReference> referencesToRename = new List<IReference>();
            foreach (var pair in references)
            {
                List<IReference> sortedReferences = pair.Value.ToList(); //LanguageUtil.GetSortedReferences(pair.Value);
                foreach (IReference reference in sortedReferences)
                {
                    IReference oldReferenceForConflict = reference;
                    InterruptableActivityCookie.CheckAndThrow(pi);
                    if (reference.IsValid()) // reference may invalidate during additional reference processing
                    {
                        RenameHelperBase rename = executer.Workflow.LanguageSpecific[reference.GetTreeNode().Language];
                        IReference reference1 = rename.TransformProjectedInitializer(reference);
                        DeclaredElementInstance subst = GetSubst(newDeclaredElement, executer);
                        IReference newReference;
                        if (subst != null)
                        {
                            if (subst.Substitution.Domain.IsEmpty())
                            {
                                newReference = reference1.BindTo(subst.Element);
                            }
                            else
                            {
                                newReference = reference1.BindTo(subst.Element, subst.Substitution);
                            }
                        }
                        else
                        {
                            newReference = reference1.BindTo(newDeclaredElement);
                        }
                        if (!(newReference is IImplicitReference))
                        {
                            IDeclaredElement element = newReference.Resolve().DeclaredElement;
                            if (!hasConflictsWithDeclarations && !this.myDoNotShowBindingConflicts &&
                                (element == null || !element.Equals(newDeclaredElement)) && !rename.IsAlias(newDeclaredElement))
                            {
                                driver.AddLateConflict(
                                    () =>
                                    new Conflict(
                                        newReference.GetTreeNode().GetSolution(),
                                        "Usage {0} can not be updated correctly.",
                                        ConflictSeverity.Error,
                                        oldReferenceForConflict),
                                    "not bound");
                            }
                            referencesToRename.Add(newReference);
                        }
                        this.myNewReferences.Insert(0, newReference);
                        rename.AdditionalReferenceProcessing(newDeclaredElement, newReference, this.myNewReferences);
                    }

                    pi.Advance();
                }
            }

            foreach (var pair in secondaryElementWithReferences)
            {
                IDeclaredElement element = pair.First;
                IList<IReference> secondaryReferences = pair.Second;
                foreach (IReference reference in secondaryReferences)
                {
                    InterruptableActivityCookie.CheckAndThrow(pi);
                    if (reference.IsValid())
                    {
                        reference.BindTo(element);
                    }
                }
            }

            if (this.myElement is PrefixDeclaredElement)
            {
                ((PrefixDeclaredElement)this.myElement).ChangeName = false;
                ((PrefixDeclaredElement)this.myElement).SetName(this.NewName);
                foreach (IReference reference in referencesToRename)
                {
                    ((NTriplesPrefixReference)reference).SetName(this.NewName);
                    reference.CurrentResolveResult = null;
                    ((NTriplesFile)((PrefixDeclaredElement)this.myElement).File).ClearTables();
                }
            }
        }

        private static DeclaredElementInstance GetSubst(IDeclaredElement element, RenameRefactoring executer)
        {
            return executer.Workflow.LanguageSpecific[element.PresentationLanguage].GetSubst(element);
        }

        private void BuildDeclarations()
        {
            this.myDeclarations.Clear();

            IDeclaredElement element = this.myOriginalElementPointer.FindDeclaredElement();
            if (element == null)
            {
                return;
            }

            IList<IDeclaration> declarations = new MultyPsiDeclarations(element).AllDeclarations;

            foreach (IDeclaration declaration in declarations)
            {
                this.myDeclarations.Add(declaration);
            }
        }
    }
}
