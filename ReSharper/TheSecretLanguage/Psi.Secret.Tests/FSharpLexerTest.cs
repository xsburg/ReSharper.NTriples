using NUnit.Framework;

namespace JetBrains.ReSharper.Psi.Secret.Tests
{
    [TestFixture]
  public class FSharpLexerTest : FSharpLexerTestBase
  {
    [Test, TestCaseSource("files")]
    public void TestLexer(string file)
    {
      this.DoTestFile(file);
    }

    // ReSharper disable StringLiteralTypo
    private readonly string[] files = new[]
    {
      "_test",
      /*"bignum-literals",
      "bytearray-string-literals",
      "bytechar-literal",
      "decimal-literals",
      "do-bang",
      "float32-literals",
      "float32-literals-exponent-form",
      "float64-literals",
      "float64-literals-exponent-form",
      "int-literals",
      "let-arith-1",
      "let-array-1",
      "let-array-2",
      "let-bang",
      "let-list-1",
      "let-list-2",
      "let-list-3",
      "let-list-4",
      "let-list-5",
      "let-list-6",
      "let-list-7",
      "let-list-8",
      "let-list-9",
      "let-mutable",
      "let-tuple-1",
      "let-tuple-2",
      "let-tuple-3",
      "let-tuple-4",
      "module-decl-1",
      "module-decl-2",
      "namespace-decl-1",
      "namespace-decl-2",
      "object-transformation-operators",
      "open-directive-1",
      "open-directive-2",
      "open-directive-3",
      "operators-comparison",
      "operators-functional",
      "preprocessor",
      "raise",
      "return-bang",
      "test1",
      "triple-quoted-string",
      "union-type-1",
      "union-type-2-generic-recursive",
      "union-type-3-generic-recursive",
      "use-bang",
      "active-patterns-multi-case", 
      "active-patterns-one-case", 
      "active-patterns-parameterized", 
      "active-patterns-partial", 
      "identifier-double-backstick-1", 
      "identifier-double-backstick-2", 
      "identifier-double-backstick-3", 
      "verbatim-string-escape-double-quote", 
      "verbatim-string-escape", 
      "verbatim-string-simple",
      "yield-bang"*/
    };
    // ReSharper restore StringLiteralTypo
  }
}