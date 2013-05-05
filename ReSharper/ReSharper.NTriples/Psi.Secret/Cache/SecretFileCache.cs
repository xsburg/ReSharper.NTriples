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

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    public class SecretFileCache
    {
        public SecretFileCache(IList<SecretUriIdentifierSymbol> uriIdentifiers)
        {
            this.UriIdentifiers = uriIdentifiers;
        }


        public IList<SecretUriIdentifierSymbol> UriIdentifiers { get; private set; }
    }
}
