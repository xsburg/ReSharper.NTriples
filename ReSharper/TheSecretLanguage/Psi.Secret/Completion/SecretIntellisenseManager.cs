using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [SolutionComponent]
    public class SecretIntellisenseManager : LanguageSpecificIntellisenseManager
    {
        public SecretIntellisenseManager(ISettingsStore settingsStore)
            : base(settingsStore)
        {
            this.SettingsStore = settingsStore;
        }

        protected override bool GetIntellisenseEnabledSpecific(IContextBoundSettingsStore boundSettingsStore)
        {
            return boundSettingsStore.GetValue((IntellisenseEnabledSettingPsi setting) => setting.IntellisenseEnabled);
        }
    }
}