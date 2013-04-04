// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLexerTest.cs
// </summary>
// ***********************************************************************

using System.Linq;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
    public class SecretLexerTest : SecretLexerTestBase
    {
        public SecretLexerTest()
        {
            var extension = this.GetType().BaseType.GetCustomAttributes(true).OfType<TestFileExtensionAttribute>().Single().Extension;
            // TODO: generate files list automatically and use exclude list to ignore unnecesserily files
        }

        // ReSharper disable StringLiteralTypo

        private readonly string[] files = new[]
            {
                "comment",
                "double",
                "keywords",
                "operators",
                "integer",
                "regular-string-bad-escaped",
                "regular-string-escaped",
                "regular-string-simple",
                "triple-quoted-string",
                "rule-complex",
                "triplet-simple",
                "triplet-complex",
                "triplet-anonymous",
                "uri-identifier",
            };

        [Test]
        [TestCaseSource("files")]
        public void TestLexer(string file)
        {
            this.DoTestFile(file);
        }

        // ReSharper restore StringLiteralTypo
    }
}
