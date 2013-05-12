// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesWordIndexLanguageProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl.Caches2.WordIndex;

namespace ReSharper.NTriples.Impl
{
    internal class NTriplesWordIndexLanguageProvider : IWordIndexLanguageProvider
    {
        public bool CaseSensitiveIdentifiers
        {
            get
            {
                return true;
            }
        }

        public bool IsIdentifierFirstLetter(char ch)
        {
            return ch.IsLetterFast() || ch == '_';
        }

        public bool IsIdentifierSecondLetter(char ch)
        {
            return ch.IsLetterOrDigitFast() || ch == '_';
        }
    }
}
