// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretCompletionTest.cs
// </summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;
using ReSharper.NTriples.Impl;

namespace ReSharper.NTriples.Tests
{
    [TestFixture]
    [TestReferences("System.Core.dll")]
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class NTriplesCompletionTest : CodeCompletionTestBase
    {
        private readonly string[] files;

        public NTriplesCompletionTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                             .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                             .ToArray();
        }

        protected override bool ExecuteAction
        {
            get
            {
                return true;
            }
        }

        protected override String RelativeTestDataPath
        {
            get
            {
                return @"completion";
            }
        }

        [Test]
        [TestCaseSource("files")]
        public void TestCompletion(string file)
        {
            this.DoOneTest(file);
        }
    }
}
