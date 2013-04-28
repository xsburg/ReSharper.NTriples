using System.IO;
using System.Linq;
using FindUsagesTestBase = JetBrains.ReSharper.Psi.Find.Test.FindUsagesTestBase;
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
        protected override string RelativeTestDataPath
        {
            get { return @"Find"; }
        }

        protected override SearchPattern SearchPattern
        {
            get { return SearchPattern.FIND_USAGES | SearchPattern.FIND_IMPLEMENTORS_USAGES | SearchPattern.FIND_RELATED_ELEMENTS; }
        }

        public SecretFindTest()
        {
            this.files = this.TestDataPath2.GetDirectoryEntries("*" + SecretProjectFileType.SecretExtension, true)
                        .Select(f => Path.GetFileNameWithoutExtension(f.FullPath))
                        .ToArray();
        }

        private readonly string[] files;

        [Test]
        [TestCaseSource("files")]
        public void TestFind(string file)
        {
            this.DoOneTest(file);
        }
    }
}