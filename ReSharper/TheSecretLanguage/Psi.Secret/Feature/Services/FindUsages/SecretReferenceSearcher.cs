// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretReferenceSearcher.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Finder;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Services.FindUsages
{
    internal class SecretReferenceSearcher : IDomainSpecificSearcher
    {
        private readonly HashSet<IDeclaredElement> myElements;
        private readonly bool myHasUnnamedElement;
        private readonly HashSet<string> myNames;
        private readonly bool mySearchForLateBound;

        public SecretReferenceSearcher(
            IDomainSpecificSearcherFactory searchWordsProvider, IEnumerable<IDeclaredElement> elements, bool searchForLateBound)
        {
            this.mySearchForLateBound = searchForLateBound;
            this.myElements = new HashSet<IDeclaredElement>(elements);

            this.myNames = new HashSet<string>();
            foreach (IDeclaredElement element in this.myElements)
            {
                foreach (string name in searchWordsProvider.GetAllPossibleWordsInFile(element))
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        this.myHasUnnamedElement = true;
                        continue;
                    }
                    this.myNames.Add(name);
                }

                string shortName = element.ShortName;
                if (!string.IsNullOrEmpty(shortName))
                {
                    this.myNames.Add(shortName);
                }
            }
        }

        public bool ProcessElement<TResult>(ITreeNode element, IFindResultConsumer<TResult> consumer)
        {
            Assertion.Assert(element != null, "The condition (element != null) is false.");

            var names = new JetHashSet<string>(this.myNames);

            FindExecution result;

            if (this.mySearchForLateBound)
            {
                result =
                    new LateBoundReferenceSourceFileProcessor<TResult>(
                        element,
                        consumer,
                        this.myElements,
                        this.myHasUnnamedElement
                            ? null
                            : names,
                        names).Run();
            }
            else
            {
                result =
                    new ReferenceSearchSourceFileProcessor<TResult>(
                        element,
                        true,
                        consumer,
                        this.myElements,
                        this.myHasUnnamedElement
                            ? null
                            : names,
                        names).Run();
            }
            return result == FindExecution.Stop;
        }

        public bool ProcessProjectItem<TResult>(IPsiSourceFile sourceFile, IFindResultConsumer<TResult> consumer)
        {
            if (!this.CanContainReferencesTo(sourceFile))
            {
                return false;
            }

            IFile psiFile = sourceFile.GetPsiFile<SecretLanguage>(new DocumentRange(sourceFile.Document, 0));
            return psiFile != null && this.ProcessElement(psiFile, consumer);
        }

        private bool CanContainReferencesTo(IPsiSourceFile sourceFile)
        {
            return Equals(sourceFile.PrimaryPsiLanguage, SecretLanguage.Instance);
        }
    }
}
