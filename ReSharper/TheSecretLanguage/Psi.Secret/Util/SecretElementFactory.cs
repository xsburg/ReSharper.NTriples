// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretElementFactory.cs
// </summary>
// ***********************************************************************

using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Util
{
    public abstract class SecretElementFactory
    {
        protected ISolution Solution { get; set; }

        public static SecretElementFactory GetInstance([NotNull] IPsiModule module)
        {
            return new SecretElementFactoryImpl(module);
        }

        public static SecretElementFactory GetInstance([NotNull] ITreeNode context)
        {
            return new SecretElementFactoryImpl(context.GetPsiModule());
        }

        public abstract ILocalName CreateLocalNameExpression(string name);
        public abstract ISentence CreatePrefixDeclarationSentence(string name, string uri);

        public abstract IPrefix CreatePrefixExpression(string name);
        public abstract IPrefixName CreatePrefixNameExpression(string name);
        public abstract IUriString CreateUriStringExpression(string name);
    }
}
