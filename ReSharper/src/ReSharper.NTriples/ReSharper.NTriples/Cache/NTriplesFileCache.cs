// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesFileCache.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;

namespace ReSharper.NTriples.Cache
{
    public class NTriplesFileCache
    {
        public NTriplesFileCache(
            IList<NTriplesUriIdentifierSymbol> uriIdentifiers, IList<NTriplesPrefixDeclarationSymbol> prefixDeclarationData)
        {
            this.UriIdentifiers = uriIdentifiers;
            this.PrefixDeclarations = prefixDeclarationData;
        }

        public IList<NTriplesPrefixDeclarationSymbol> PrefixDeclarations { get; private set; }
        public IList<NTriplesUriIdentifierSymbol> UriIdentifiers { get; private set; }
    }
}
