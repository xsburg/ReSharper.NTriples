// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   PsiElementFactory.cs
// </summary>
// ***********************************************************************

using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Util
{
    public abstract class PsiElementFactory
    {
        protected ISolution Solution { get; set; }

        public static PsiElementFactory GetInstance([NotNull] IPsiModule module)
        {
            return new PsiElementFactoryImpl(module);
        }

        public static PsiElementFactory GetInstance([NotNull] ITreeNode context)
        {
            return new PsiElementFactoryImpl(context.GetPsiModule());
        }

        public abstract IPrefix CreatePrefixExpression(string name);
        public abstract IPrefixName CreatePrefixNameExpression(string name);
        public abstract ILocalName CreateLocalNameExpression(string name);
        public abstract IUriIdentifier CreateUriIdentifierExpression(string name);
    }
}
