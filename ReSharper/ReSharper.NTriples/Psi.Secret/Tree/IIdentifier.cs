// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IIdentifier.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Secret.Cache;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public partial interface IIdentifier
    {
        IdentifierKind GetKind();
    }
}
