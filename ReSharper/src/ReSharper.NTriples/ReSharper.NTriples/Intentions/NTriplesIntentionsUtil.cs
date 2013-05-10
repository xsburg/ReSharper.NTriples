// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesIntentionsUtil.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Intentions
{
    internal static class NTriplesIntentionsUtil
    {
        public static ISentence AddToTarget(ISentence declarationToAdd, ICreationTarget target)
        {
            var inserter = new NTriplesPrefixInserter(declarationToAdd, target);
            return inserter.InsertRule();
        }
    }
}
