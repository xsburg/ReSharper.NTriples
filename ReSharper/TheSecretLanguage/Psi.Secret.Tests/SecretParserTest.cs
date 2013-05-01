using System;
using System.IO;
using JetBrains.ReSharper.FeaturesTests.Finding.FindUsages;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.PsiTests.parsing;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using System.Linq;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class SecretParserTest : ParserTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return @"Parsing"; }
        }

        public SecretParserTest()
        {
            files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                        .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                        .ToArray();
        }

        private readonly string[] files;

        [Test]
        [TestCaseSource("files")]
        public void TestParser(string file)
        {
            this.DoOneTest(file);
        }
    }
}