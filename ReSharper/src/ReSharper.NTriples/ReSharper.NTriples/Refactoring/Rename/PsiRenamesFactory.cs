// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiRenamesFactory.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Refactoring.Rename
{
    [FeaturePart]
    public class PsiRenamesFactory : AtomicRenamesFactory
    {
        public static string NameFromCamelCase(string s)
        {
            string firstLetter = s.Substring(0, 1);
            firstLetter = firstLetter.ToLower();
            s = firstLetter + s.Substring(1, s.Length - 1);
            return s;
        }

        public static string NameToCamelCase(string s)
        {
            string firstLetter = s.Substring(0, 1);
            firstLetter = firstLetter.ToUpper();
            s = firstLetter + s.Substring(1, s.Length - 1);
            return s;
        }

        public override RenameAvailabilityCheckResult CheckRenameAvailability(IDeclaredElement element)
        {
            return RenameAvailabilityCheckResult.CanBeRenamed;
        }

        public override IEnumerable<AtomicRenameBase> CreateAtomicRenames(
            IDeclaredElement declaredElement, string newName, bool doNotAddBindingConflicts)
        {
            yield return new PsiDerivedElementRename(declaredElement, newName, doNotAddBindingConflicts);
            // TODO: uriSymbols should be declared elements, find it and rename if it is localName rename etc. 
        }

        public override bool IsApplicable(IDeclaredElement declaredElement)
        {
            return declaredElement.PresentationLanguage.Is<SecretLanguage>();
        }
    }
}
