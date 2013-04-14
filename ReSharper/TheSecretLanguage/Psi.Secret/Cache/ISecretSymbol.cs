// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   ISecretSymbol.cs
// </summary>
// ***********************************************************************

using JetBrains.Annotations;

namespace JetBrains.ReSharper.Psi.Secret.Cache
{
    /// <summary>
    ///     Customization point for pdi properties
    /// </summary>
    [CannotApplyEqualityOperator]
    public interface ISecretSymbol
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
