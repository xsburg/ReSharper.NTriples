using JetBrains.ReSharper.Feature.Services.Occurences;

namespace JetBrains.ReSharper.Psi.Secret.Feature.Finding.GotoMember
{
    public class SecretFileMemberData
    {
        private readonly ContainerDisplayStyle myDisambigStyle;
        private readonly IDeclaredElement myElement;

        public SecretFileMemberData(IDeclaredElement element, ContainerDisplayStyle disambigStyle)
        {
            this.myElement = element;
            this.myDisambigStyle = disambigStyle;
        }

        public ContainerDisplayStyle ContainerDisplayStyle
        {
            get
            {
                return this.myDisambigStyle;
            }
        }

        public IDeclaredElement Element
        {
            get
            {
                return this.myElement;
            }
        }
    }
}