// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IntellisenseEnabledSettingPsi.cs
// </summary>
// ***********************************************************************

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
