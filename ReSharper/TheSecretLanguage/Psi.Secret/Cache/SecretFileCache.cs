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