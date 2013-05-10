// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesLexerTest.cs
// </summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using NUnit.Framework;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tests
{
    [TestFixture]
    public class NTriplesLexerTest : NTriplesLexerTestBase
    {
        // ReSharper disable StringLiteralTypo

        private readonly string[] files;

        public NTriplesLexerTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + NTriplesProjectFileType.NTriplesExtension, true)
                             .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                             .ToArray();
        }

        [Test]
        [TestCaseSource("files")]
        public void TestLexer(string file)
        {
            this.DoTestFile(file);
        }

        // ReSharper restore StringLiteralTypo
    }
}
