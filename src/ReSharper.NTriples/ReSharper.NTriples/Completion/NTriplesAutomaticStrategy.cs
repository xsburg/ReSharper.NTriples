// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesAutomaticStrategy.cs
// </summary>
// ***********************************************************************

using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Completion
{
    [SolutionComponent]
    public class NTriplesAutomaticStrategy : IAutomaticCodeCompletionStrategy
    {
        private readonly SettingsScalarEntry mySettingsScalarEntry;

        public NTriplesAutomaticStrategy(ISettingsStore settingsStore)
        {
            this.mySettingsScalarEntry =
                settingsStore.Schema.GetScalarEntry((NTriplesAutopopupEnabledSettingsKey key) => key.OnIdent);
        }

        public CodeCompletionType CodeCompletionType
        {
            get
            {
                return CodeCompletionType.AutomaticCompletion;
            }
        }

        public PsiLanguageType Language
        {
            get
            {
                return NTriplesLanguage.Instance;
            }
        }

        public bool AcceptTyping(char c, ITextControl textControl, IContextBoundSettingsStore boundSettingsStore)
        {
            return IsIdentStart(c);
        }

        public bool AcceptsFile(IFile file, ITextControl textControl)
        {
            return file is INTriplesFile;
        }

        public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
        {
            return (AutopopupType)settingsStore.GetValue(this.mySettingsScalarEntry, null);
        }

        public bool ProcessSubsequentTyping(char c, ITextControl textControl)
        {
            return IsIdentBody(c);
        }

        private static bool IsIdentBody(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_' || c == '@';
        }

        private static bool IsIdentStart(char c)
        {
            return char.IsLetter(c) || c == ':' || c == '@';
        }
    }
}
