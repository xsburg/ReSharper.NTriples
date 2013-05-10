// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesRefactoringRenameTest.cs
// </summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.Refactorings;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tests
{
    [TestFixture]
    [TestFileExtension(NTriplesProjectFileType.NTriplesExtension)]
    public class NTriplesRefactoringRenameTest : RenameTestBase
    {
        private readonly string[] files;

        public NTriplesRefactoringRenameTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + NTriplesProjectFileType.NTriplesExtension, true)
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
