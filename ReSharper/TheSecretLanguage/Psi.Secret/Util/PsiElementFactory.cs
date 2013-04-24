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

        public abstract IPrefixName CreatePrefixExpression(string name);

        /*public abstract IRuleName CreateIdentifierExpression(string name);

    public abstract IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters = false);

    public abstract IRuleDeclaration CreateRuleDeclaration(string name, bool hasBraceParameters, IList<Pair<string, string>> variableParameters);*/
    }
}
