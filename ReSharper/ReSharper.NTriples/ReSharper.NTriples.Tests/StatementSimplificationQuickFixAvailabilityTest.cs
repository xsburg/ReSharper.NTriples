using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Test;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.CodeInspections.Highlightings;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Tests
{
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    [TestFixture]
    public class StatementSimplificationQuickFixAvailabilityTest : QuickFixAvailabilityTestBase
    {
        private readonly string[] files;

        protected override string RelativeTestDataPath
        {
            get { return @"Intentions\QuickFixes"; }
        }

        public StatementSimplificationQuickFixAvailabilityTest()
        {
            this.files = this.GetFilesToTest();
        }

        protected override bool HighlightingPredicate(IHighlighting highlighting, IPsiSourceFile psiSourceFile)
        {
            return highlighting is SuggestionRangeHighlighting<ISentence>;
        }

        [Test]
        [Ignore]
        [TestCaseSource("files")]
        public void TestQuickFixAvailability(string file)
        {
            this.DoOneTest(file);
        }
    }
}