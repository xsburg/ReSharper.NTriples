// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCodeFormatterFactory.cs
// </summary>
// ***********************************************************************

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using ReSharper.NTriples.Impl;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    [ProjectFileType(typeof(SecretProjectFileType))]
    public class SecretCodeFormatterFactory : ISecretCodeFormatterFactory
    {
        public SecretFormattingVisitor CreateFormattingVisitor(CodeFormattingContext context)
        {
            return new SecretFormattingVisitor(context);
        }
    }
}
