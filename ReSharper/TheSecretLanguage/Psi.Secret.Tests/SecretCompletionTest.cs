using System;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
    [TestReferences("System.Core.dll")]
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class SecretCompletionTest : CodeCompletionTestBase
    {
        protected override String RelativeTestDataPath
        {
            get { return @"completion"; }
        }
        public SecretCompletionTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                        .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                        .ToArray();
        }

        private readonly string[] files;

        [Test]
        [TestCaseSource("files")]
        public void TestCompletion(string file)
        {
            this.DoOneTest(file);
        }

        protected override bool ExecuteAction
        {
            get { return true; }
        }
    }
}