// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesContextActionDataProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.TextControl;
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
    }
}
