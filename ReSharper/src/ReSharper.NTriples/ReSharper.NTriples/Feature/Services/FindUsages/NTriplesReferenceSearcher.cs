// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesReferenceSearcher.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Finder;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Feature.Services.FindUsages
{
    internal class NTriplesReferenceSearcher : IDomainSpecificSearcher
    {
        private readonly HashSet<IDeclaredElement> myElements;
        private readonly bool myHasUnnamedElement;
        private readonly HashSet<string> myNames;
        private readonly bool mySearchForLateBound;

        public NTriplesReferenceSearcher(
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
                var psiSourceFile = element.GetSourceFile();
                foreach (var myElement in this.myElements)
                {
                    var declarations = myElement.GetDeclarationsIn(psiSourceFile);
                    foreach (var declaration in declarations)
                    {
                        var refs = declaration.GetFirstClassReferences();
                        foreach (var r in refs)
                        {
                            consumer.Accept(new FindResultReference(r, declaration.DeclaredElement));
                        }
                    }
                }

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

            IFile psiFile = sourceFile.GetPsiFile<NTriplesLanguage>(new DocumentRange(sourceFile.Document, 0));
            return psiFile != null && this.ProcessElement(psiFile, consumer);
        }

        private bool CanContainReferencesTo(IPsiSourceFile sourceFile)
        {
            return Equals(sourceFile.PrimaryPsiLanguage, NTriplesLanguage.Instance);
        }
    }
}
