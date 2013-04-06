using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class SecretParserTest : ParserTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return @"parsing"; }
        }

        [Test]
        public void TestSingleLineTriplet()
        {
            this.DoNamedTest();
        }
    }
}