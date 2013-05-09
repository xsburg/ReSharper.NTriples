using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Psi;

namespace ReSharper.NTriples.Feature.Finding.GotoMember
{
    public class NTriplesFileMemberData
    {
        private readonly ContainerDisplayStyle myDisambigStyle;
        private readonly IDeclaredElement myElement;

        public NTriplesFileMemberData(IDeclaredElement element, ContainerDisplayStyle disambigStyle)
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