// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   N3WordIndexProvider.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.Impl.Caches2.WordIndex;

namespace JetBrains.ReSharper.Psi.Secret
{
    public class N3WordIndexProvider : IWordIndexLanguageProvider
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
            return WordIndexTokenizerUtil.IsLetterFast(ch) || ch == '_' || ch == '$';
        }

        public bool IsIdentifierSecondLetter(char ch)
        {
            return WordIndexTokenizerUtil.IsLetterOrDigitFast(ch) || ch == '_' || ch == '$';
        }
    }
}
