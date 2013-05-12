// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   ICreateNTriplesPrefixIntention.cs
// </summary>
// ***********************************************************************

namespace ReSharper.NTriples.Intentions.CreateFromUsage
{
    public interface ICreateNTriplesPrefixIntention
    {
        NTriplesIntentionResult ExecuteEx(CreateNTriplesPrefixContext context);
    }
}
