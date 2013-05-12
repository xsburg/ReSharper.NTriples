// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   INTriplesSymbol.cs
// </summary>
// ***********************************************************************

using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace ReSharper.NTriples.Cache
{
    /// <summary>
    ///     Customization point for pdi properties
    /// </summary>
    [CannotApplyEqualityOperator]
    public interface INTriplesSymbol
    {
        string Name { get; }

        /// <summary>
        ///     Offset of symbol declaration in the source fileFullName tree
        /// </summary>
        int Offset { get; }

        /// <summary>
        ///     Owner PSI source fileFullName
        /// </summary>
        IPsiSourceFile SourceFile { get; }
    }
}
