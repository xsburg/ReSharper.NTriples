using JetBrains.ReSharper.IntentionsTests;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Intentions.QuickFix;

namespace ReSharper.NTriples.Tests
{
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    [TestFixture]
    public class FactsSimplificationQuickFixTest : QuickFixTestBase<FactsSimplificationQuickFix>
    {
        private readonly string[] files;

        public FactsSimplificationQuickFixTest()
        {
            this.files = this.GetFilesToTest();
        }

        [Test]
        [TestCaseSource("files")]
        public void TestQuickFix(string file)
        {
            this.DoOneTest(file);
        }

        protected override string RelativeTestDataPath
        {
            get { return @"Intentions\QuickFixes\FactsSimplification"; }
        }
    }
}