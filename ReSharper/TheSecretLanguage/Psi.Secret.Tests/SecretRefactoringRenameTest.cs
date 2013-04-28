using System;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class SecretRefactoringRenameTest : RenameTestBase
    {
        protected override String RelativeTestDataPath
        {
            get { return @"Refactoring/Rename"; }
        }
        public SecretRefactoringRenameTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                             .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                             .ToArray();
        }

        private readonly string[] files;

        [Test]
        [TestCaseSource("files")]
        public void TestRefactoringRename(string file)
        {
            this.DoOneTest(file);
        }
    }
}