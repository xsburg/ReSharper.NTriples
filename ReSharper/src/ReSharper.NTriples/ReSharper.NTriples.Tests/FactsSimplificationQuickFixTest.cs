// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   FactsSimplificationQuickFixTest.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.IntentionsTests;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Intentions.QuickFix;

namespace ReSharper.NTriples.Tests
{
    [TestFileExtension(NTriplesProjectFileType.NTriplesExtension)]
    [TestFixture]
    public class FactsSimplificationQuickFixTest : QuickFixTestBase<FactsSimplificationQuickFix>
    {
        private readonly string[] files;

        public FactsSimplificationQuickFixTest()
        {
            this.files = this.GetFilesToTest();
        }

        protected override string RelativeTestDataPath
        {
            get
            {
                return @"Intentions\QuickFixes\FactsSimplification";
            }
        }

        [Test]
        [TestCaseSource("files")]
        public void TestQuickFix(string file)
        {
            this.DoOneTest(file);
        }
    }
}
