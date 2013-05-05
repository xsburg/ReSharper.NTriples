// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   IUriString.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.NTriples.Tree
{
    public partial interface IUriString : IDeclaration //, IChameleonNode
    {
        string GetUri();
        void SetReferenceName(string shortName);
    }
}
