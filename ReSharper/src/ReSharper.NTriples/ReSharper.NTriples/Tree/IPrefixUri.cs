// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IPrefixUri.cs
// </summary>
// ***********************************************************************

using ReSharper.NTriples.Resolve;

namespace ReSharper.NTriples.Tree
{
    public partial interface IPrefixUri
    {
        SecretPrefixUriReference PrefixUriReference { get; }
        void SetName(string shortName);
    }
}
