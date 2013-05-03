// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretParserTest.cs
// </summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class SecretParserTest : ParserTestBase
    {
        private readonly string[] files;

        public SecretParserTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
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
