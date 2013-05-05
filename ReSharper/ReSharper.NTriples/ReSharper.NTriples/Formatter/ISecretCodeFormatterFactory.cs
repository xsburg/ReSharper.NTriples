// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   IPsiCodeFormatterFactory.cs
// </summary>
// ***********************************************************************

using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    internal interface ISecretCodeFormatterFactory
    {
        [NotNull]
        SecretFormattingVisitor CreateFormattingVisitor([NotNull] CodeFormattingContext formattingData);
    }
}
