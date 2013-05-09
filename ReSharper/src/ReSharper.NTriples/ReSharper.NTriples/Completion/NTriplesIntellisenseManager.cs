// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretIntellisenseManager.cs
// </summary>
// ***********************************************************************

using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace ReSharper.NTriples.Completion
{
    [SolutionComponent]
    public class NTriplesIntellisenseManager : LanguageSpecificIntellisenseManager
    {
        public NTriplesIntellisenseManager(ISettingsStore settingsStore)
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
