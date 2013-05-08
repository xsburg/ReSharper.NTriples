using JetBrains.ReSharper.IntentionsTests;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Intentions.QuickFix;

namespace ReSharper.NTriples.Tests
{
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    [TestFixture]
    public class StatementsSimplificationQuickFixTest : QuickFixTestBase<StatementsSimplificationQuickFix>
    {
        private readonly string[] files;

        public StatementsSimplificationQuickFixTest()
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
            get { return @"Intentions\QuickFixes\StatementsSimplification"; }
        }
    }
}