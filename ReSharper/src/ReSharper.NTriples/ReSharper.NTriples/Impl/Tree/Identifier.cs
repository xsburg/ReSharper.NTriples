// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   Identifier.cs
// </summary>
// ***********************************************************************

using ReSharper.NTriples.Cache;
using ReSharper.NTriples.Parsing;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Impl.Tree
{
    internal partial class Identifier
    {
        public IdentifierKind GetKind()
        {
            return NTriplesIdentifierFilter.GetIdentifierKind(this);
        }

        public IdentifierInfo GetInfo()
        {
            return NTriplesIdentifierFilter.GetIdentifierInfo(this);
        }
    }
}
