// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesContextActionDataProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Bulbs;
using JetBrains.ReSharper.Daemon.CaretDependentFeatures;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.TextControl;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Feature.Services.MatchingBrace
{
    public class NTriplesContextActionDataProvider
        : CachedContextActionDataProviderBase<INTriplesFile>, INTriplesContextActionDataProvider
    {
        public NTriplesContextActionDataProvider(ISolution solution, ITextControl textControl, INTriplesFile file)
            : base(solution, textControl, file)
        {
        }

        [SolutionComponent]
        public class Current : CurrentContextActionDataProviderBase<INTriplesContextActionDataProvider, NTriplesLanguage, INTriplesFile>
        {
            public Current(Lifetime lifetime, IShellLocks shellLocks, ContextManager contextManager, AsyncCommitService asyncCommitService, ISolution solution, ITextControlManager textControlManager, IPsiFiles psiFiles)
                : base(lifetime, shellLocks, contextManager, asyncCommitService, solution, textControlManager, psiFiles)
            {
            }

            protected override ContextKeyWithValueBase<INTriplesContextActionDataProvider> GetContextKey()
            {
                return ContextKey.Instance;
            }

            protected override INTriplesContextActionDataProvider CreateDataProvider(ISolution solution, ITextControl textControl, INTriplesFile psiFile)
            {
                return new NTriplesContextActionDataProvider(solution, textControl, psiFile);
            }
        }

        public sealed class ContextKey : ContextKeyWithValueBase<INTriplesContextActionDataProvider>
        {
            public static readonly ContextKey Instance = new ContextKey();

            static ContextKey()
            {
            }

            private ContextKey()
            {
            }
        }
    }
}
