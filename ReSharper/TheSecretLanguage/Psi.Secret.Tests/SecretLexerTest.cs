// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLexerTest.cs
// </summary>
// ***********************************************************************

using System.IO;
using System.Linq;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
    public class SecretLexerTest : SecretLexerTestBase
    {
        // ReSharper disable StringLiteralTypo

        public SecretLexerTest()
        {
            files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                        .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                        .ToArray();
        }

        private readonly string[] files;

        [Test]
        [TestCaseSource("files")]
        public void TestLexer(string file)
        {
            this.DoTestFile(file);
        }

        // ReSharper restore StringLiteralTypo
    }
}
