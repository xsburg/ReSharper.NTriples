// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiRenamesFactory.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.ReSharper.Refactorings.Rename;
using JetBrains.ReSharper.Refactorings.RenameModel;

namespace JetBrains.ReSharper.Psi.Secret.Refactoring.Rename
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
            /*
      if (declaredElement is RuleDeclaration)
      {
        var ruleDeclaration = declaredElement as RuleDeclaration;
        foreach (IDeclaredElement element in ruleDeclaration.DerivedParserMethods)
        {
          yield return new PsiDerivedElementRename(element, "parse" + NameToCamelCase(newName),
            doNotAddBindingConflicts);
        }
        foreach (IDeclaredElement element in ruleDeclaration.DerivedClasses)
        {
          yield return new PsiDerivedElementRename(element, NameToCamelCase(newName),
            doNotAddBindingConflicts);
        }
        foreach (IDeclaredElement element in ruleDeclaration.DerivedInterfaces)
        {
          yield return new PsiDerivedElementRename(element, ruleDeclaration.InterfacePrefix + NameToCamelCase(newName),
            doNotAddBindingConflicts);
        }
        foreach (IDeclaredElement element in ruleDeclaration.DerivedVisitorMethods)
        {
          yield return new PsiDerivedElementRename(element, ruleDeclaration.VisitorMethodPrefix + NameToCamelCase(newName) + ruleDeclaration.VisitorMethodSuffix,
            doNotAddBindingConflicts);
        }
      }*/
        }

        public override bool IsApplicable(IDeclaredElement declaredElement)
        {
            if (declaredElement.PresentationLanguage.Is<SecretLanguage>())
            {
                return true;
            }
            return false;
        }
    }
}
