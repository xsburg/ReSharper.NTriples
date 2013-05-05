using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Finding.GotoMember
{
    internal class SecretFileMembersMap : OneToSetMap<string, SecretFileMemberData>
    {
        public static readonly Key<SecretFileMembersMap> SecretFileMembersMapKey =
            new Key<SecretFileMembersMap>("SecretFileMembersMap");
    }
}