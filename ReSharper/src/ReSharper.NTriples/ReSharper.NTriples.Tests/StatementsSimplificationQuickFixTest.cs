// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   StatementsSimplificationQuickFixTest.cs
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
    public class StatementsSimplificationQuickFixTest : QuickFixTestBase<StatementsSimplificationQuickFix>
    {
        private readonly string[] files;

        public StatementsSimplificationQuickFixTest()
        {
            this.files = this.GetFilesToTest();
        }

        protected override string RelativeTestDataPath
        {
            get
            {
                return @"Intentions\QuickFixes\StatementsSimplification";
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
