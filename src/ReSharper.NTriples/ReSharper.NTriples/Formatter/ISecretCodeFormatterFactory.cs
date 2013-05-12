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

namespace ReSharper.NTriples.Formatter
{
    internal interface INTriplesCodeFormatterFactory
    {
        [NotNull]
        NTriplesFormattingVisitor CreateFormattingVisitor([NotNull] CodeFormattingContext formattingData);
    }
}
