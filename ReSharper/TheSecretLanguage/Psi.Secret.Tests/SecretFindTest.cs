// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretFindTest.cs
// </summary>
// ***********************************************************************

using JetBrains.ReSharper.Psi.Find.Test;
using JetBrains.ReSharper.Psi.Search;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
    [Category("Find")]
    [TestNetFramework4]
    [TestFileExtension(SecretProjectFileType.SecretExtension)]
    public class SecretFindTest : FindUsagesTestBase
    {
        private readonly string[] files;

        public SecretFindTest()
        {
            this.files = this.GetFilesToTest();
        }

        protected override string RelativeTestDataPath
        {
            get
            {
                return @"Find";
            }
        }

        protected override SearchPattern SearchPattern
        {
            get
            {
                return SearchPattern.FIND_USAGES | SearchPattern.FIND_IMPLEMENTORS_USAGES | SearchPattern.FIND_RELATED_ELEMENTS;
            }
        }

        [Test]
        [TestCaseSource("files")]
        public void TestFind(string file)
        {
            this.DoOneTest(file);
        }
    }
}
