// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesParserTest.cs
// </summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tests
{
    [TestFileExtension(NTriplesProjectFileType.NTriplesExtension)]
    public class NTriplesParserTest : ParserTestBase
    {
        private readonly string[] files;

        public NTriplesParserTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + NTriplesProjectFileType.NTriplesExtension, true)
                             .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                             .ToArray();
        }

        protected override string RelativeTestDataPath
        {
            get
            {
                return @"Parsing";
            }
        }

        [Test]
        [TestCaseSource("files")]
        public void TestParser(string file)
        {
            this.DoOneTest(file);
        }
    }
}
