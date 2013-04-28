using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [SettingsKey(typeof(IntellisenseEnabledSettingsKey), "Override VS IntelliSense for Secret")]
    public class IntellisenseEnabledSettingPsi
    {
        [SettingsEntry(false, "Secret (*.n3 files)")]
        public bool IntellisenseEnabled;
    }
}
