// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretContextActionDataProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.TextControl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Feature.Services.MatchingBrace
{
    public class NTriplesContextActionDataProvider
        : CachedContextActionDataProviderBase<ISecretFile>, INTriplesContextActionDataProvider
    {
        public NTriplesContextActionDataProvider(ISolution solution, ITextControl textControl, ISecretFile file)
            : base(solution, textControl, file)
        {
        }
    }
}
