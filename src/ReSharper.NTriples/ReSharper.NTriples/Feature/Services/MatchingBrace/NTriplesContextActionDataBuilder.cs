// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesContextActionDataBuilder.cs
// </summary>
// ***********************************************************************

using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.TextControl;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Feature.Services.MatchingBrace
{
    [ContextActionDataBuilder(typeof(INTriplesContextActionDataProvider))]
    public class NTriplesContextActionDataBuilder : IContextActionDataBuilder
    {
        public IContextActionDataProvider Build(ISolution solution, ITextControl textControl)
        {
            if (!solution.GetPsiServices().Caches.IsIdle.Value)
            {
                return null;
            }

            IPsiSourceFile psiSourceFile = textControl.Document.GetPsiSourceFile(solution);
            if (psiSourceFile == null || !psiSourceFile.IsValid())
            {
                return null;
            }

            var file =
                psiSourceFile.GetPsiFile<NTriplesLanguage>(new DocumentRange(textControl.Document, textControl.Caret.Offset())) as
                INTriplesFile;
            if (file == null || !file.IsValid() || !file.Language.Is<NTriplesLanguage>())
            {
                return null;
            }

            return new NTriplesContextActionDataProvider(solution, textControl, file);
        }
    }
}
