// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesFileMembersMap.cs
// </summary>
// ***********************************************************************

using JetBrains.Util;

namespace ReSharper.NTriples.Feature.Finding.GotoMember
{
    internal class NTriplesFileMembersMap : OneToSetMap<string, NTriplesFileMemberData>
    {
        public static readonly Key<NTriplesFileMembersMap> NTriplesFileMembersMapKey =
            new Key<NTriplesFileMembersMap>("NTriplesFileMembersMap");
    }
}
