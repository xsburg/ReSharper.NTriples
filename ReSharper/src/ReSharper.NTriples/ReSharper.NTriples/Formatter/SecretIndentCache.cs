// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretIndentCache.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.NTriples.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Formatter
{
    public class SecretIndentCache : IndentCache<ISecretTreeNode>
    {
        public SecretIndentCache(
            ICodeFormatterImpl codeFormatter,
            Func<ITreeNode, CustomIndentType, string> customLineIndenter,
            AlignmentTabFillStyle tabFillStyle,
            GlobalFormatSettings formatSettings)
            : base(codeFormatter, customLineIndenter, tabFillStyle, formatSettings)
        {
        }
    }
}
