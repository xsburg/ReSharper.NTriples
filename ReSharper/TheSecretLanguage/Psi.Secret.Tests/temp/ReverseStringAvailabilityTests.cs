using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.CSharp.Test;
using JetBrains.ReSharper.Intentions.Extensibility;
using NUnit.Framework;

namespace ReSharperSecretLanguage.Tests
{
    [TestFixture]
    public class ReverseStringAvailabilityTests : CSharpContextActionAvailabilityTestBase
    {
        protected override string ExtraPath
        {
            get
            {
                return "ReverseStringAction";
            }
        }

        protected override string RelativeTestDataPath
        {
            get
            {
                return "ReverseStringAction";
            }
        }

        [Test]
        public void AvailabilityTest()
        {
            this.DoTestFiles("availability01.cs");
        }

        protected override IContextAction CreateContextAction(ICSharpContextActionDataProvider dataProvider)
        {
            return new ReverseStringAction(dataProvider);
        }
    }
}
