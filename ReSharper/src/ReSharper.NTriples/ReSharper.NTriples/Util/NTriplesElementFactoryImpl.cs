// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretElementFactoryImpl.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.Text;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Util
{
    public class NTriplesElementFactoryImpl : NTriplesElementFactory
    {
        private readonly NTriplesLanguageService myLanguageService;
        private readonly IPsiModule myModule;

        public NTriplesElementFactoryImpl([NotNull] IPsiModule module)
            : this(module, module.GetSolution())
        {
        }

        private NTriplesElementFactoryImpl([NotNull] IPsiModule module, [NotNull] ISolution solution)
        {
            this.myModule = module;
            this.Solution = solution;
            this.Solution.GetPsiServices();
            this.myLanguageService = (NTriplesLanguageService)NTriplesLanguage.Instance.LanguageService();
        }

        public override ILocalName CreateLocalNameExpression(string name)
        {
            var text = string.Format("foo:{0} a false.", name);
            var node = this.CreateSecretFile(text);

            var expression = node.SentencesEnumerable.First().Statement.Subject.FirstChild;
            if (expression != null)
            {
                var identifier = (IIdentifier)expression.FirstChild;
                if (identifier != null)
                {
                    var uriIdentifier = (IUriIdentifier)identifier.FirstChild;

                    if (uriIdentifier != null && uriIdentifier.LocalName != null)
                    {
                        return uriIdentifier.LocalName;
                    }
                }
            }

            throw new ElementFactoryException(string.Format("Cannot create file '{0}'", text));
        }

        public override ISentence CreatePrefixDeclarationSentence(string name, string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                uri = "uri";
            }

            var text = string.Format("@prefix {0}: <{1}>.", name, uri);
            var file = this.CreateSecretFile(text, true);
            var sentence = file.SentencesEnumerable.First();
            return sentence;
        }

        public override IPrefix CreatePrefixExpression(string name)
        {
            var text = string.Format("{0}:bar a false.", name);
            var node = this.CreateSecretFile(text);

            var expression = node.SentencesEnumerable.First().Statement.Subject.FirstChild;
            if (expression != null)
            {
                var identifier = (IIdentifier)expression.FirstChild;
                if (identifier != null)
                {
                    var uriIdentifier = (IUriIdentifier)identifier.FirstChild;

                    if (uriIdentifier != null && uriIdentifier.Prefix != null)
                    {
                        return uriIdentifier.Prefix;
                    }
                }
            }

            throw new ElementFactoryException(string.Format("Cannot create file '{0}'", text));
        }

        public override IPrefixName CreatePrefixNameExpression(string name)
        {
            var text = string.Format("@prefix {0}: <http://foo.bar>.", name);
            var node = this.CreateSecretFile(text);

            var prefixDeclaration = (IPrefixDeclaration)node.SentencesEnumerable.First().Directive.FirstChild;
            if (prefixDeclaration != null && prefixDeclaration.PrefixName != null)
            {
                return prefixDeclaration.PrefixName;
            }

            throw new ElementFactoryException(string.Format("Cannot create file '{0}'", text));
        }

        public override IUriString CreateUriStringExpression(string name)
        {
            var text = string.Format("<{0}> a false.", name);
            var node = this.CreateSecretFile(text);

            var expression = node.SentencesEnumerable.First().Statement.Subject.FirstChild;
            if (expression != null)
            {
                var identifier = (IIdentifier)expression.FirstChild;
                if (identifier != null)
                {
                    var uriIdentifier = (IUriIdentifier)identifier.FirstChild;

                    if (uriIdentifier != null && uriIdentifier.UriStringElement != null)
                    {
                        return uriIdentifier.UriStringElement;
                    }
                }
            }

            throw new ElementFactoryException(string.Format("Cannot create file '{0}'", text));
        }

        public override IPrefixUri CreatePrefixUriExpression(string name)
        {
            var text = string.Format("@prefix foo: <{0}>.", name);
            var node = this.CreateSecretFile(text);

            var prefixDeclaration = (IPrefixDeclaration)node.SentencesEnumerable.First().Directive.FirstChild;
            if (prefixDeclaration != null && prefixDeclaration.PrefixUri != null)
            {
                return prefixDeclaration.PrefixUri;
            }

            throw new ElementFactoryException(string.Format("Cannot create file '{0}'", text));
        }

        public override ISentence CreateSentence(string subject, IDictionary<string, string[]> facts)
        {
            var separator = facts.Count > 1 || (facts.Count == 1 && facts.First().Value.Length > 1)
                                ? Environment.NewLine + Indent
                                : " ";
            var factsText = string.Join(";" + Environment.NewLine + Indent, facts.Select(this.GetFact));
            var text = string.Format("{0}{1}{2}.", subject, separator, factsText);
            var file = this.CreateSecretFile(text, true);
            var sentence = file.SentencesEnumerable.First();
            return sentence;
        }

        private string GetFact(KeyValuePair<string, string[]> pair)
        {
            var predicate = pair.Key;
            var objectsText = pair.Value.Length > 1
                                  ? string.Join(",", pair.Value.Select(o => Environment.NewLine + Indent + Indent + o))
                                  : pair.Value[0];
            return string.Format("{0} {1}", predicate, objectsText);
        }

        private const string Indent = "    ";
        private SecretParser CreateParser(string text)
        {
            return
                (SecretParser)
                this.myLanguageService.CreateParser(
                    this.myLanguageService.GetPrimaryLexerFactory().CreateLexer(new StringBuffer(text)), null, null);
        }

        private ISecretFile CreateSecretFile(string text, bool restoreWhitespaces = false)
        {
            var node = this.CreateParser(text).ParseSecretFile(false, restoreWhitespaces) as ISecretFile;
            if (node == null)
            {
                throw new ElementFactoryException(string.Format("Cannot create file '{0}'", text));
            }

            SandBox.CreateSandBoxFor(node, this.myModule);
            return node;
        }

        /*public override IRuleName CreateIdentifierExpression(string name)
    {
      var expression = (IRuleName)this.CreateExpression("$0", name);
      return expression;
    }

    public override IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters = false)
    {
      string braceParameters = "";
      if(hasBraceParameters)
      {
        braceParameters = " {ROLE, getter} ";
      }
      var node = this.CreateParser(name + braceParameters + "\n" + ":" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name + braceParameters + "\n" + ":" + "\n" + ";"));
      }
      SandBox.CreateSandBoxFor(node, this.myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name));
    }

    public override IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters, IList<Pair<string, string>> variableParameters)
    {
      if(variableParameters.Count == 0)
      {
        return this.CreateRuleDeclaration(name, hasBraceParameters);
      }

      string braceParameters = "";
      if (hasBraceParameters)
      {
        braceParameters = " {ROLE, getter} ";
      }

      string variableParametersString = " [";
      foreach (var variableParameter in variableParameters)
      {
        variableParametersString = variableParametersString + variableParameter.Second + " " + variableParameter.First + ",";
      }
      variableParametersString = variableParametersString.Substring(0, variableParametersString.Length - 1) + "]";

      var node = this.CreateParser(name + braceParameters + variableParametersString + "\n" + ":" + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name + braceParameters + variableParametersString + "\n" + ":" + "\n" + ";"));
      }
      SandBox.CreateSandBoxFor(node, this.myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        return ruleDeclaration;
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", name));
    }

    private ITreeNode CreateExpression(string format, string name)
    {
      var node = this.CreateParser(name + "\n" + ":" + name + "\n" + ";").ParsePsiFile(false) as IPsiFile;
      if (node == null)
      {
        throw new ElementFactoryException(string.Format("Cannot create expression '{0}'", format));
      }
      SandBox.CreateSandBoxFor(node, this.myModule);
      var ruleDeclaration = node.FirstChild as IRuleDeclaration;
      if (ruleDeclaration != null)
      {
        IRuleBody ruleBody = ruleDeclaration.Body;
        ITreeNode child = ruleBody.FirstChild;
        while (child != null && ! (child is IPsiExpression))
        {
          child = child.NextSibling;
        }
        while (child != null && !(child is IRuleName))
        {
          child = child.FirstChild;
        }
        if (child != null)
        {
          return child;
        }
      }
      throw new ElementFactoryException(string.Format("Cannot create expression '{0}'" + name, format));
    }*/
    }
}
