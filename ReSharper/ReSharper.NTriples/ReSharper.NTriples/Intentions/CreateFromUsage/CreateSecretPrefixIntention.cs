// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CreateSecretPrefixIntention.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    [Language(typeof(SecretLanguage))]
    internal class CreateSecretPrefixIntention : ICreateSecretPrefixIntention
    {
        public SecretIntentionResult ExecuteEx(CreateSecretPrefixContext context)
        {
            return SecretPrefixBuilder.Create(context);
        }
    }
}
