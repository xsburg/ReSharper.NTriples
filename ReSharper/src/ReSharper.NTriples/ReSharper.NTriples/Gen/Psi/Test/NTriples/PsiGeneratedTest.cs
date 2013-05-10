//------------------------------------------------------------------------------
// <auto-generated>
//     Generated by IntelliJ parserGen
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 0168, 0219, 0108, 0414
// ReSharper disable RedundantNameQualifier
using System;
using System.IO;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
namespace ReSharper.NTriples.Impl.Tree
{
  public partial class PsiGeneratedGetterTestUtil : PsiGetterTestUtil
  {
      internal static void TestNTriplesFile (int level, ReSharper.NTriples.Tree.INTriplesFile param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.ISentence> children = ((ReSharper.NTriples.Tree.INTriplesFile)param).Sentences;
        foreach (ReSharper.NTriples.Tree.ISentence child in children)
        TestSentence (level + 1, (ReSharper.NTriples.Tree.ISentence)child, "Sentences");
      }
    }
      internal static void TestAnonymousIdentifier (int level, ReSharper.NTriples.Tree.IAnonymousIdentifier param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestAxisDirective (int level, ReSharper.NTriples.Tree.IAxisDirective param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestCutStatement (int level, ReSharper.NTriples.Tree.ICutStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestDataLiteral (int level, ReSharper.NTriples.Tree.IDataLiteral param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IDataLiteral)param).String;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "String");
      }
    }
      internal static void TestDirective (int level, ReSharper.NTriples.Tree.IDirective param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestExpression (int level, ReSharper.NTriples.Tree.IExpression param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestExtensionDirective (int level, ReSharper.NTriples.Tree.IExtensionDirective param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestFact (int level, ReSharper.NTriples.Tree.IFact param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        ReSharper.NTriples.Tree.IPredicate child = ((ReSharper.NTriples.Tree.IFact)param).Predicate;
        TestPredicate (level + 1, (ReSharper.NTriples.Tree.IPredicate)child, "Predicate");
      }
      {
        JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IExpression> children = ((ReSharper.NTriples.Tree.IFact)param).Objects;
        foreach (ReSharper.NTriples.Tree.IExpression child in children)
        TestExpression (level + 1, (ReSharper.NTriples.Tree.IExpression)child, "Objects");
      }
      {
        JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IIdentifier> children = ((ReSharper.NTriples.Tree.IFact)param).PredicateIdentifiers;
        foreach (ReSharper.NTriples.Tree.IIdentifier child in children)
        TestIdentifier (level + 1, (ReSharper.NTriples.Tree.IIdentifier)child, "PredicateIdentifiers");
      }
    }
      internal static void TestFacts (int level, ReSharper.NTriples.Tree.IFacts param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestForAllDirective (int level, ReSharper.NTriples.Tree.IForAllDirective param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestForSomeDirective (int level, ReSharper.NTriples.Tree.IForSomeDirective param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestFormula (int level, ReSharper.NTriples.Tree.IFormula param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestFromStatement (int level, ReSharper.NTriples.Tree.IFromStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestFunctorStatement (int level, ReSharper.NTriples.Tree.IFunctorStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestHasExpression (int level, ReSharper.NTriples.Tree.IHasExpression param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestIdentifier (int level, ReSharper.NTriples.Tree.IIdentifier param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestIfStatement (int level, ReSharper.NTriples.Tree.IIfStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestIsOfExpression (int level, ReSharper.NTriples.Tree.IIsOfExpression param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        ReSharper.NTriples.Tree.IExpression child = ((ReSharper.NTriples.Tree.IIsOfExpression)param).Expression;
        TestExpression (level + 1, (ReSharper.NTriples.Tree.IExpression)child, "Expression");
      }
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IIsOfExpression)param).IsKeyword;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "IsKeyword");
      }
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IIsOfExpression)param).OfKeyword;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "OfKeyword");
      }
    }
      internal static void TestKeywordStatement (int level, ReSharper.NTriples.Tree.IKeywordStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestList (int level, ReSharper.NTriples.Tree.IList param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestLiteral (int level, ReSharper.NTriples.Tree.ILiteral param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      if (param is ReSharper.NTriples.Tree.IDataLiteral) TestDataLiteral (level, (ReSharper.NTriples.Tree.IDataLiteral)param, caller);
      else throw new System.InvalidOperationException();
    }
      internal static void TestLiteral_keywords (int level, ReSharper.NTriples.Tree.ILiteral_keywords param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      else throw new System.InvalidOperationException();
    }
      internal static void TestLocalName (int level, ReSharper.NTriples.Tree.ILocalName param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestMeta (int level, ReSharper.NTriples.Tree.IMeta param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestNotStatement (int level, ReSharper.NTriples.Tree.INotStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestObjects (int level, ReSharper.NTriples.Tree.IObjects param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestOrStatement (int level, ReSharper.NTriples.Tree.IOrStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestPredicate (int level, ReSharper.NTriples.Tree.IPredicate param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestPrefix (int level, ReSharper.NTriples.Tree.IPrefix param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestPrefixDeclaration (int level, ReSharper.NTriples.Tree.IPrefixDeclaration param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IPrefixDeclaration)param).Prefix;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "Prefix");
      }
      {
        ReSharper.NTriples.Tree.IPrefixName child = ((ReSharper.NTriples.Tree.IPrefixDeclaration)param).PrefixName;
        TestPrefixName (level + 1, (ReSharper.NTriples.Tree.IPrefixName)child, "PrefixName");
      }
      {
        ReSharper.NTriples.Tree.IPrefixUri child = ((ReSharper.NTriples.Tree.IPrefixDeclaration)param).PrefixUri;
        TestPrefixUri (level + 1, (ReSharper.NTriples.Tree.IPrefixUri)child, "PrefixUri");
      }
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IPrefixDeclaration)param).UriString;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "UriString");
      }
    }
      internal static void TestPrefixName (int level, ReSharper.NTriples.Tree.IPrefixName param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestPrefixUri (int level, ReSharper.NTriples.Tree.IPrefixUri param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestSentence (int level, ReSharper.NTriples.Tree.ISentence param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        ReSharper.NTriples.Tree.IDirective child = ((ReSharper.NTriples.Tree.ISentence)param).Directive;
        TestDirective (level + 1, (ReSharper.NTriples.Tree.IDirective)child, "Directive");
      }
      {
        ReSharper.NTriples.Tree.IStatement child = ((ReSharper.NTriples.Tree.ISentence)param).Statement;
        TestStatement (level + 1, (ReSharper.NTriples.Tree.IStatement)child, "Statement");
      }
    }
      internal static void TestSentences (int level, ReSharper.NTriples.Tree.ISentences param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.ISentence> children = ((ReSharper.NTriples.Tree.ISentences)param).SentenceList;
        foreach (ReSharper.NTriples.Tree.ISentence child in children)
        TestSentence (level + 1, (ReSharper.NTriples.Tree.ISentence)child, "SentenceList");
      }
    }
      internal static void TestSmartVar (int level, ReSharper.NTriples.Tree.ISmartVar param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestStatement (int level, ReSharper.NTriples.Tree.IStatement param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        ReSharper.NTriples.Tree.IFacts child = ((ReSharper.NTriples.Tree.IStatement)param).FactsElement;
        TestFacts (level + 1, (ReSharper.NTriples.Tree.IFacts)child, "FactsElement");
      }
      {
        ReSharper.NTriples.Tree.ISubject child = ((ReSharper.NTriples.Tree.IStatement)param).Subject;
        TestSubject (level + 1, (ReSharper.NTriples.Tree.ISubject)child, "Subject");
      }
      {
        JetBrains.ReSharper.Psi.Tree.TreeNodeCollection<ReSharper.NTriples.Tree.IFact> children = ((ReSharper.NTriples.Tree.IStatement)param).Facts;
        foreach (ReSharper.NTriples.Tree.IFact child in children)
        TestFact (level + 1, (ReSharper.NTriples.Tree.IFact)child, "Facts");
      }
    }
      internal static void TestSubject (int level, ReSharper.NTriples.Tree.ISubject param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestUriIdentifier (int level, ReSharper.NTriples.Tree.IUriIdentifier param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        ReSharper.NTriples.Tree.ILocalName child = ((ReSharper.NTriples.Tree.IUriIdentifier)param).LocalName;
        TestLocalName (level + 1, (ReSharper.NTriples.Tree.ILocalName)child, "LocalName");
      }
      {
        ReSharper.NTriples.Tree.IPrefix child = ((ReSharper.NTriples.Tree.IUriIdentifier)param).Prefix;
        TestPrefix (level + 1, (ReSharper.NTriples.Tree.IPrefix)child, "Prefix");
      }
      {
        ReSharper.NTriples.Tree.IUriString child = ((ReSharper.NTriples.Tree.IUriIdentifier)param).UriStringElement;
        TestUriString (level + 1, (ReSharper.NTriples.Tree.IUriString)child, "UriStringElement");
      }
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IUriIdentifier)param).UriString;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "UriString");
      }
    }
      internal static void TestUriIdentifiers (int level, ReSharper.NTriples.Tree.IUriIdentifiers param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestUriString (int level, ReSharper.NTriples.Tree.IUriString param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
      {
        JetBrains.ReSharper.Psi.Tree.ITokenNode child = ((ReSharper.NTriples.Tree.IUriString)param).Value;
        TestTokenNode (level + 1, (JetBrains.ReSharper.Psi.Tree.ITokenNode)child, "Value");
      }
    }
      internal static void TestUseExternalDirective (int level, ReSharper.NTriples.Tree.IUseExternalDirective param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestVariableIdentifier (int level, ReSharper.NTriples.Tree.IVariableIdentifier param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
      internal static void TestVariables (int level, ReSharper.NTriples.Tree.IVariables param, String caller)
    {
      if (!CanVisitFurther(param)) return;
      VisitElement (level, param, caller);
    }
    }
}
