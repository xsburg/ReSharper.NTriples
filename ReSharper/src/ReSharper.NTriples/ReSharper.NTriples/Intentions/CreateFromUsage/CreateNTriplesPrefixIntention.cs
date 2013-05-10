// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   CreateNTriplesPrefixIntention.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    [Language(typeof(NTriplesLanguage))]
    internal class CreateNTriplesPrefixIntention : ICreateNTriplesPrefixIntention
    {
        public NTriplesIntentionResult ExecuteEx(CreateNTriplesPrefixContext context)
        {
            return NTriplesPrefixBuilder.Create(context);
        }
    }
}
