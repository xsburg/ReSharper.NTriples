// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretRefactoringRenameTest.cs
// </summary>
// ***********************************************************************

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
        private readonly string[] files;

        public SecretRefactoringRenameTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                             .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                             .ToArray();
        }

        protected override String RelativeTestDataPath
        {
            get
            {
                return @"Refactoring/Rename";
            }
        }

        [Test]
        [TestCaseSource("files")]
        public void TestRefactoringRename(string file)
        {
            this.DoOneTest(file);
        }
    }
}
