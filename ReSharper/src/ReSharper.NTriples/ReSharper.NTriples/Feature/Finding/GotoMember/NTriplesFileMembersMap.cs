using JetBrains.Util;

namespace ReSharper.NTriples.Feature.Finding.GotoMember
{
    internal class NTriplesFileMembersMap : OneToSetMap<string, NTriplesFileMemberData>
    {
        public static readonly Key<NTriplesFileMembersMap> SecretFileMembersMapKey =
            new Key<NTriplesFileMembersMap>("NTriplesFileMembersMap");
    }
}