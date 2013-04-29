// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretContextActionDataBuilder.cs
// </summary>
// ***********************************************************************

using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Services.MatchingBrace
{
    [ContextActionDataBuilder(typeof(ISecretContextActionDataProvider))]
    public class SecretContextActionDataBuilder : IContextActionDataBuilder
    {
        public IContextActionDataProvider Build(ISolution solution, ITextControl textControl)
        {
            if (!solution.GetPsiServices().CacheManager.IsIdle)
            {
                return null;
            }

            IPsiSourceFile psiSourceFile = textControl.Document.GetPsiSourceFile(solution);
            if (psiSourceFile == null || !psiSourceFile.IsValid())
            {
                return null;
            }

            var file =
                psiSourceFile.GetPsiFile<SecretLanguage>(new DocumentRange(textControl.Document, textControl.Caret.Offset())) as
                ISecretFile;
            if (file == null || !file.IsValid() || !file.Language.Is<SecretLanguage>())
            {
                return null;
            }

            return new SecretContextActionDataProvider(solution, textControl, file);
        }
    }
}
