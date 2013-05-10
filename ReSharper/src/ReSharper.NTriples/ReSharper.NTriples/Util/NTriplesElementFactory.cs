// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretElementFactory.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Tree;
using IStatement = ReSharper.NTriples.Tree.IStatement;

namespace ReSharper.NTriples.Util
{
    public abstract class NTriplesElementFactory
    {
        protected ISolution Solution { get; set; }

        public static NTriplesElementFactory GetInstance([NotNull] IPsiModule module)
        {
            return new NTriplesElementFactoryImpl(module);
        }

        public static NTriplesElementFactory GetInstance([NotNull] ITreeNode context)
        {
            return new NTriplesElementFactoryImpl(context.GetPsiModule());
        }

        public abstract ILocalName CreateLocalNameExpression(string name);
        public abstract ISentence CreatePrefixDeclarationSentence(string name, string uri);

        public abstract IPrefix CreatePrefixExpression(string name);
        public abstract IPrefixName CreatePrefixNameExpression(string name);
        public abstract IUriString CreateUriStringExpression(string name);
        public abstract IPrefixUri CreatePrefixUriExpression(string name);
        public abstract ISentence CreateSentence(string subject, IDictionary<string, string[]> facts);
    }
}
