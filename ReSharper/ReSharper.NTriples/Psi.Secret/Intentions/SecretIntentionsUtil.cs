// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretIntentionsUtil.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Feature.Services.Intentions.DataProviders;
using JetBrains.ReSharper.Psi.Secret.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Intentions
{
    internal static class SecretIntentionsUtil
    {
        public static ISentence AddToTarget(ISentence declarationToAdd, ICreationTarget target)
        {
            var inserter = new SecretPrefixInserter(declarationToAdd, target);
            return inserter.InsertRule();
        }
    }
}
