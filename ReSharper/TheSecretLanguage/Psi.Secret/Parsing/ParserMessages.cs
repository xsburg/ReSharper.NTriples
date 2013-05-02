using System;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    // ReSharper disable InconsistentNaming
    public class ParserMessages
    {
        private const string IDS_EXPECTED_SYMBOL = "{0} expected";
        private const string IDS_EXPECTED_TWO_SYMBOLS = "{0} or {1} expected";
        private const string IDS_UNEXPECTED_TOKEN = "Unexpected token";
        
        public const string IDS_DIRECTIVE = "directive";
        public const string IDS_KEYWORD_STATEMENT = "keyword_statements";
        public const string IDS_META = "meta";
        public const string IDS_SENTENCE = "sentence";
        public const string IDS_SMART_VAR = "list or uri";
        public const string IDS_STATEMENT = "statement";
        public const string IDS_URI_IDENTIFIER = "identifier";
        public const string IDS_ANONYMOUS_IDENTIFIER = "anonymous identifier";
        public const string IDS_FACTS = "facts";
        public const string IDS_PREDICATE = "predicate";
        public const string IDS_LITERAL = "literal";
        public const string IDS_LITERAL_KEYWORDS = "literal keywords";
        public const string IDS_IDENTIFIER = "identifier";

        public const string IDS_EXPRESSION = "expression";
        public const string IDS_INFIX_SYMBOL = "symbol";
        public const string IDS_OPERATOR_EXPRESSION = "expression";
        public const string IDS_OVERLOADABLE_OPERATOR = "operator";
        public const string IDS_PATTERN = "pattern";
        public const string IDS_PARAMETERIZED_TYPE_REFERENCE = "parameterized type reference";
        public const string IDS_PREFIX_SYMBOL = "symbol";
        public const string IDS_SPECIFICATION = "specification";
        public const string IDS_TYPE_EXPRESSION = "expression";
        public const string IDS_TYPE_INFORMATION = "type information";
        public const string IDS_TYPE_PARAMETER_LIST = "parameter";
        public const string IDS_TYPE_REPRESENTATION = "type representation";
        public const string IDS_TYPE_SCHEME_EXPRESSION = "expression";
        public const string IDS_UNARY_EXPRESSION = "expression";
        public const string IDS_VALUE_NAME = "value name";
        public const string IDS_VALUE_REFERENCE_EXPRESSION_OLD = "expression";
        public const string IDS_PREFIX_NAME = "prefixName:";
        public const string IDS_PREFIX = "prefix:";

        public static string GetString(string id)
        {
            return id;
        }

        public static string GetUnexpectedTokenMessage()
        {
            return IDS_UNEXPECTED_TOKEN;
        }

        public static string GetExpectedMessage(string expectedSymbol)
        {
            return String.Format(GetString(IDS_EXPECTED_SYMBOL), expectedSymbol).Capitalize();
        }

        public static string GetExpectedMessage(string firstExpectedSymbol, string secondExpectedSymbol)
        {
            return String.Format(GetString(IDS_EXPECTED_TWO_SYMBOLS), firstExpectedSymbol, secondExpectedSymbol).Capitalize();
        }
    }
    // ReSharper restore InconsistentNaming
}