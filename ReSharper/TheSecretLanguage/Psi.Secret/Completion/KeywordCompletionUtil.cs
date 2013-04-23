// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   KeywordCompletionUtil.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Secret.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using IIdentifier = JetBrains.ReSharper.Psi.Secret.Tree.IIdentifier;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    internal static class KeywordCompletionUtil
    {
        public static IEnumerable<string> GetAplicableKeywords(ISecretFile file, TreeTextRange referenceRange)
        {
            var list = new List<string>();
            var token = file.FindNodeAt(referenceRange) as ITokenNode;
            if (token == null)
            {
                return list;
            }

            // identifier/literal_keywords
            var identifier = token.GetParent<IIdentifier>(2);
            if (identifier != null)
            {
                list.Add("true");
                list.Add("false");
                list.Add("null");
            }

            // predicate
            list.Add("a");
            // predicate/hasExpression
            list.Add("@has");
            // predicate/isOfExpression
            list.Add("@is");
            list.Add("@of");

            // meta
            list.Add("in");
            list.Add("@for");
            list.Add("out");
            list.Add("axis");
            list.Add("meta");

            // directives (starts directive, global scope)
            list.Add("@prefix");
            list.Add("@std_prefix");
            list.Add("@extension");
            list.Add("@using");
            list.Add("@axis-default");
            list.Add("@forAll");
            list.Add("@forSome");

            return list;
        }
    }
    [SolutionComponent]
    public class PsiAutomaticStrategy : IAutomaticCodeCompletionStrategy
    {
        private readonly SettingsScalarEntry mySettingsScalarEntry;

        public PsiAutomaticStrategy(ISettingsStore settingsStore)
        {
            mySettingsScalarEntry = settingsStore.Schema.GetScalarEntry((SecretAutopopupEnabledSettingsKey key) => key.OnIdent);
        }

        public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
        {
            return (AutopopupType)settingsStore.GetValue(mySettingsScalarEntry, null);
        }

        public bool AcceptTyping(char c, ITextControl textControl, IContextBoundSettingsStore boundSettingsStore)
        {
            return true;
            if (!IsIdentStart(c))
            {
                return false;
            }

            return true;
        }

        public bool ProcessSubsequentTyping(char c, ITextControl textControl)
        {
            return true;
            return (IsIdentStart(c) || char.IsDigit(c) || c == '_');
        }

        public bool AcceptsFile(IFile file, ITextControl textControl)
        {
            return true;
            return this.MatchTokenType(file, textControl, token => token.IsIdentifier || token.IsKeyword);
        }

        public CodeCompletionType CodeCompletionType
        {
            get { return CodeCompletionType.AutomaticCompletion; }
        }

        public PsiLanguageType Language
        {
            get { return SecretLanguage.Instance; }
        }

        private static bool IsIdentStart(char c)
        {
            return char.IsLetter(c);
        }
    }

    [SolutionComponent]
    public class SecretIntellisenseManager : LanguageSpecificIntellisenseManager
    {
        public SecretIntellisenseManager(ISettingsStore settingsStore)
            : base(settingsStore)
        {
            SettingsStore = settingsStore;
        }

        protected override bool GetIntellisenseEnabledSpecific(IContextBoundSettingsStore boundSettingsStore)
        {
            return boundSettingsStore.GetValue((IntellisenseEnabledSettingPsi setting) => setting.IntellisenseEnabled);
        }
    }

    [SettingsKey(typeof(AutopopupEnabledSettingsKey), "Secret")]
    public class SecretAutopopupEnabledSettingsKey
    {
        [SettingsEntry(AutopopupType.HardAutopopup, "After colon")]
        public AutopopupType AfterColon;

        [SettingsEntry(AutopopupType.HardAutopopup, "On letters and digits")]
        public AutopopupType OnIdent;
    }

    [SettingsKey(typeof(IntellisenseEnabledSettingsKey), "Override VS IntelliSense for Secret")]
    public class IntellisenseEnabledSettingPsi
    {
        [SettingsEntry(false, "Secret (.n3 files and embedded Secret)")]
        public bool IntellisenseEnabled;
    }
}
