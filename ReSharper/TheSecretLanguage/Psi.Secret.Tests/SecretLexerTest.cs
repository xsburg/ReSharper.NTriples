// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLexerTest.cs
// </summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
    public class SecretLexerTest : SecretLexerTestBase
    {
        // ReSharper disable StringLiteralTypo

        private readonly string[] files;

        public SecretLexerTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
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
