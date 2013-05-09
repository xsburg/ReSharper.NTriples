// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretFileCache.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;

namespace ReSharper.NTriples.Cache
{
    public class NTriplesFileCache
    {
        public NTriplesFileCache(IList<NTriplesUriIdentifierSymbol> uriIdentifiers)
        {
            this.UriIdentifiers = uriIdentifiers;
        }


        public IList<NTriplesUriIdentifierSymbol> UriIdentifiers { get; private set; }
    }
}
