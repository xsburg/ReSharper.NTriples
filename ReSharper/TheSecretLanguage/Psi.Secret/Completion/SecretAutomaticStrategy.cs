using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [SolutionComponent]
    public class SecretAutomaticStrategy : IAutomaticCodeCompletionStrategy
    {
        private readonly SettingsScalarEntry mySettingsScalarEntry;

        public SecretAutomaticStrategy(ISettingsStore settingsStore)
        {
            this.mySettingsScalarEntry = settingsStore.Schema.GetScalarEntry((SecretAutopopupEnabledSettingsKey key) => key.OnIdent);
        }

        public AutopopupType IsEnabledInSettings(IContextBoundSettingsStore settingsStore, ITextControl textControl)
        {
            return (AutopopupType)settingsStore.GetValue(this.mySettingsScalarEntry, null);
        }

        public bool AcceptTyping(char c, ITextControl textControl, IContextBoundSettingsStore boundSettingsStore)
        {
            return IsIdentStart(c);
        }

        public bool ProcessSubsequentTyping(char c, ITextControl textControl)
        {
            return IsIdentBody(c);
        }

        public bool AcceptsFile(IFile file, ITextControl textControl)
        {
            return file is ISecretFile;
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
            return char.IsLetter(c) || c == ':';
        }

        private static bool IsIdentBody(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }
    }
}