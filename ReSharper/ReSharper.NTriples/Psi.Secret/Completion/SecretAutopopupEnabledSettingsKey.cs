// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretAutopopupEnabledSettingsKey.cs
// </summary>
// ***********************************************************************

using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;

namespace JetBrains.ReSharper.Psi.Secret.Completion
{
    [SettingsKey(typeof(AutopopupEnabledSettingsKey), "Secret")]
    public class SecretAutopopupEnabledSettingsKey
    {
        [SettingsEntry(AutopopupType.HardAutopopup, "After colon")]
        public AutopopupType AfterColon;

        [SettingsEntry(AutopopupType.HardAutopopup, "On letters and digits")]
        public AutopopupType OnIdent;
    }
}
