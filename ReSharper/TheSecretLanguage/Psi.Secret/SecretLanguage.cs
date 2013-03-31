// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLanguage.cs
// </summary>
// ***********************************************************************

using JetBrains.Annotations;

namespace JetBrains.ReSharper.Psi.Secret
{
    [LanguageDefinition(LanguageName)]
    public class SecretLanguage : KnownLanguage
    {
        public const string LanguageName = "Secret";

        [UsedImplicitly]
        public static readonly SecretLanguage Instance;

        protected SecretLanguage([NotNull] string name) : base(name)
        {
        }

        protected SecretLanguage([NotNull] string name, [NotNull] string presentableName) : base(name, presentableName)
        {
        }

        private SecretLanguage() : base(LanguageName, "The Great Secret")
        {
        }
    }
}
