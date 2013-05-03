// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IPrefix.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Secret.Resolve;

namespace JetBrains.ReSharper.Psi.Secret.Tree
{
    public partial interface IPrefix
    {
        SecretPrefixReference PrefixReference { get; }
        void SetName(string shortName);
    }
}
