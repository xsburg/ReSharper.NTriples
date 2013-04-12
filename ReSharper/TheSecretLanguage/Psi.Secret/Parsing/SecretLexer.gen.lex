using JetBrains;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;
using System.Collections;

%%

%unicode

%namespace JetBrains.ReSharper.Psi.Secret.Parsing
%class SecretLexerGenerated
%public
%implements ILexer
%function _locateToken
%type TokenNodeType
%eofval{ 
  currTokenType = null; return currTokenType;
%eofval}

NULL_CHAR=\u0000
CARRIAGE_RETURN_CHAR=\u000D
LINE_FEED_CHAR=\u000A
NEW_LINE_PAIR={CARRIAGE_RETURN_CHAR}{LINE_FEED_CHAR}
NEW_LINE_CHAR=({CARRIAGE_RETURN_CHAR}|{LINE_FEED_CHAR}|(\u0085)|(\u2028)|(\u2029))
NOT_NEW_LINE=([^\u0085\u2028\u2029\u000D\u000A])
NOT_NEW_LINE_NUMBER_WS=([^\#\u0085)\u2028\u2029\n\r\ \t\f\u0009\u000B\u000C])


INPUT_CHARACTER={NOT_NEW_LINE}
ASTERISKS="*"+

%include ../obj/Unicode.lex

WHITE_SPACE_CHAR=({UNICODE_ZS}|(\u0009)|(\u000B)|(\u000C)|(\u200B)|(\uFEFF)|{NULL_CHAR})
WHITE_SPACE=({WHITE_SPACE_CHAR}+)

BIND=(->)
L_PARENTHESES=(\()
R_PARENTHESES=(\))
L_BRACE=(\{)
R_BRACE=(\})
L_BRACKET=(\[)
R_BRACKET=(\])

HAS_KEYWORD=(@has)
IS_KEYWORD=(@is)
FOR_ALL_KEYWORD=(@forAll)
FOR_SOME_KEYWORD=(@forSome)
A_KEYWORD=(a)
OF_KEYWORD=(@of)
IMPLIES=(=>)
SAME_AS=(=)
PREFIX_KEYWORD=(@prefix)
STD_PREFIX_KEYWORD=(@std_prefix)
EXTENSION_KEYWORD=(@extension)
USING_KEYWORD=(@using)
DEFAULT_AXIS_KEYWORD=(@axis-default)

AXIS_KEYWORD=(@?axis)
FUNCTOR_KEYWORD=(@?functor)
META_KEYWORD=(@?meta)
IN_KEYWORD=(@?in|@for)
OUT_KEYWORD=(@?out)
SELECT_KEYWORD=(@?select)
FROM_KEYWORD=(@?from)
NOT_KEYWORD=(@?not)
IF_KEYWORD=(@?if)
TRY_KEYWORD=(@?try)
OR_KEYWORD=(@?or)
IF_NOT_KEYWORD=(@?if-not)
THEN_KEYWORD=(@?then)
ELSE_KEYWORD=(@?else)
ONCE_KEYWORD=(@?once)

NAME_KEY=(:-)
EQUAL_TO=(:=|==)
NOT_EQUAL_TO=(!=)
CONNECT=(<->)
ELLIPSIS=(\.\.\.|\u2026)
DATA_SUFFIX=(\u005E\u005E)

URI_BEGIN=(<)
URI_END=(>)
URI_STRING=([^>]*)

NAME_START_CHAR=([a-zA-Z_0-9\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD])
NAME_CHAR=({NAME_START_CHAR}|[\-\u00B7\u0300-\u036F\u203F-\u2040])
NAME=({NAME_START_CHAR}{NAME_CHAR}*)

TRUE_KEYWORD=(true)
FALSE_KEYWORD=(false)
NULL_KEYWORD=(null)
DOT=(\.)
COMMA=(,)
SEMICOLON=(;)

SINGLE_LINE_COMMENT=("#"{INPUT_CHARACTER}*)

DECIMAL_DIGIT=[0-9]
INTEGER=[+\-]?{DECIMAL_DIGIT}+
EXPONENT_PART=([eE](([+-])?({DECIMAL_DIGIT})*))
DOUBLE={INTEGER}{DOT}{DECIMAL_DIGIT}+{EXPONENT_PART}?

HEX_DIGIT=({DECIMAL_DIGIT}|[A-Fa-f])
SIMPLE_ESCAPE_SEQUENCE=(\\[\"\\rnt])
UNICODE_ESCAPE_SEQUENCE=((\\u{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT})|(\\U{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}))
SINGLE_CHARACTER=([^\\\"\u0000-\u001f])
REGULAR_STRING_LITERAL_CHARACTER=({SINGLE_CHARACTER}|{SIMPLE_ESCAPE_SEQUENCE}|{UNICODE_ESCAPE_SEQUENCE})
REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*\")
TRIPLE_QUOTED_STRING=(\"\"\"{SIMPLE_CHAR_OR_QUOTE_CHAR_SEQUENCE}*\"\"\")
SIMPLE_CHAR_OR_QUOTE_CHAR_SEQUENCE=([^\"]|(\"[^\"])|(\"\"[^\"]))
STRING_LITERAL=({REGULAR_STRING_LITERAL}|{TRIPLE_QUOTED_STRING})

LANG=(@[a-z]+(-[a-z0-9]+)*)

EXPRESSION_TAIL_OPERATOR=([!\u005E])
NAMESPACE_SEPARATOR=(:)
IDENTIFIER=({NAME})
VARIABLE_PREFIX=(\?)

%state URISTRING
%state URIEND
%state NAMESPACESEPARATOR
%state LOCALIDENTIFIER

%%

<YYINITIAL> {HAS_KEYWORD} { currTokenType = makeToken(SecretTokenType.HAS_KEYWORD); return currTokenType; }
<YYINITIAL> {IS_KEYWORD} { currTokenType = makeToken(SecretTokenType.IS_KEYWORD); return currTokenType; }
<YYINITIAL> {FOR_ALL_KEYWORD} { currTokenType = makeToken(SecretTokenType.FOR_ALL_KEYWORD); return currTokenType; }
<YYINITIAL> {FOR_SOME_KEYWORD} { currTokenType = makeToken(SecretTokenType.FOR_SOME_KEYWORD); return currTokenType; }
<YYINITIAL> {A_KEYWORD} { currTokenType = makeToken(SecretTokenType.A_KEYWORD); return currTokenType; }
<YYINITIAL> {OF_KEYWORD} { currTokenType = makeToken(SecretTokenType.OF_KEYWORD); return currTokenType; }
<YYINITIAL> {IMPLIES} { currTokenType = makeToken(SecretTokenType.IMPLIES); return currTokenType; }
<YYINITIAL> {SAME_AS} { currTokenType = makeToken(SecretTokenType.SAME_AS); return currTokenType; }
<YYINITIAL> {PREFIX_KEYWORD} { currTokenType = makeToken(SecretTokenType.PREFIX_KEYWORD); return currTokenType; }
<YYINITIAL> {STD_PREFIX_KEYWORD} { currTokenType = makeToken(SecretTokenType.STD_PREFIX_KEYWORD); return currTokenType; }
<YYINITIAL> {EXTENSION_KEYWORD} { currTokenType = makeToken(SecretTokenType.EXTENSION_KEYWORD); return currTokenType; }
<YYINITIAL> {USING_KEYWORD} { currTokenType = makeToken(SecretTokenType.USING_KEYWORD); return currTokenType; }
<YYINITIAL> {DEFAULT_AXIS_KEYWORD} { currTokenType = makeToken(SecretTokenType.DEFAULT_AXIS_KEYWORD); return currTokenType; }

<YYINITIAL> {AXIS_KEYWORD} { currTokenType = makeToken(SecretTokenType.AXIS_KEYWORD); return currTokenType; }
<YYINITIAL> {FUNCTOR_KEYWORD} { currTokenType = makeToken(SecretTokenType.FUNCTOR_KEYWORD); return currTokenType; }
<YYINITIAL> {META_KEYWORD} { currTokenType = makeToken(SecretTokenType.META_KEYWORD); return currTokenType; }
<YYINITIAL> {IN_KEYWORD} { currTokenType = makeToken(SecretTokenType.IN_KEYWORD); return currTokenType; }
<YYINITIAL> {OUT_KEYWORD} { currTokenType = makeToken(SecretTokenType.OUT_KEYWORD); return currTokenType; }
<YYINITIAL> {SELECT_KEYWORD} { currTokenType = makeToken(SecretTokenType.SELECT_KEYWORD); return currTokenType; }
<YYINITIAL> {FROM_KEYWORD} { currTokenType = makeToken(SecretTokenType.FROM_KEYWORD); return currTokenType; }
<YYINITIAL> {NOT_KEYWORD} { currTokenType = makeToken(SecretTokenType.NOT_KEYWORD); return currTokenType; }
<YYINITIAL> {IF_KEYWORD} { currTokenType = makeToken(SecretTokenType.IF_KEYWORD); return currTokenType; }
<YYINITIAL> {TRY_KEYWORD} { currTokenType = makeToken(SecretTokenType.TRY_KEYWORD); return currTokenType; }
<YYINITIAL> {OR_KEYWORD} { currTokenType = makeToken(SecretTokenType.OR_KEYWORD); return currTokenType; }
<YYINITIAL> {IF_NOT_KEYWORD} { currTokenType = makeToken(SecretTokenType.IF_NOT_KEYWORD); return currTokenType; }
<YYINITIAL> {THEN_KEYWORD} { currTokenType = makeToken(SecretTokenType.THEN_KEYWORD); return currTokenType; }
<YYINITIAL> {ELSE_KEYWORD} { currTokenType = makeToken(SecretTokenType.ELSE_KEYWORD); return currTokenType; }
<YYINITIAL> {ONCE_KEYWORD} { currTokenType = makeToken(SecretTokenType.ONCE_KEYWORD); return currTokenType; }

<YYINITIAL> {TRUE_KEYWORD} { currTokenType = makeToken(SecretTokenType.TRUE_KEYWORD); return currTokenType; }
<YYINITIAL> {FALSE_KEYWORD} { currTokenType = makeToken(SecretTokenType.FALSE_KEYWORD); return currTokenType; }
<YYINITIAL> {NULL_KEYWORD} { currTokenType = makeToken(SecretTokenType.NULL_KEYWORD); return currTokenType; }

<YYINITIAL> {SINGLE_LINE_COMMENT} { return makeToken(SecretTokenType.END_OF_LINE_COMMENT); }

<YYINITIAL> {WHITE_SPACE} { currTokenType = makeToken(SecretTokenType.WHITE_SPACE); return currTokenType; }

<YYINITIAL> {NEW_LINE_PAIR} { yybegin(YYINITIAL); return makeToken(SecretTokenType.NEW_LINE); }
<YYINITIAL> {NEW_LINE_CHAR} { yybegin(YYINITIAL); return makeToken(SecretTokenType.NEW_LINE); }

<YYINITIAL> {INTEGER} { currTokenType = makeToken(SecretTokenType.INTEGER_LITERAL); return currTokenType; }
<YYINITIAL> {DOUBLE} { currTokenType = makeToken(SecretTokenType.DOUBLE_LITERAL); return currTokenType; }
<YYINITIAL> {STRING_LITERAL} { currTokenType = makeToken(SecretTokenType.STRING_LITERAL); return currTokenType; }

<YYINITIAL> {URI_BEGIN} { yybegin(URISTRING); currTokenType = makeToken (SecretTokenType.URI_BEGIN); return currTokenType; }
<URISTRING> {URI_STRING} { yybegin(URIEND); currTokenType = makeToken (SecretTokenType.URI_STRING); return currTokenType; }
<URIEND> {URI_END} { yybegin(YYINITIAL); currTokenType = makeToken (SecretTokenType.URI_END); return currTokenType; }

<YYINITIAL> {IDENTIFIER} { currTokenType = makeToken(SecretTokenType.IDENTIFIER); return currTokenType; }
<YYINITIAL> {NAMESPACE_SEPARATOR} { currTokenType = makeToken(SecretTokenType.NAMESPACE_SEPARATOR); return currTokenType; }

<YYINITIAL> {DOT} { currTokenType = makeToken(SecretTokenType.DOT); return currTokenType; }
<YYINITIAL> {COMMA} { currTokenType = makeToken(SecretTokenType.COMMA); return currTokenType; }
<YYINITIAL> {SEMICOLON} { currTokenType = makeToken(SecretTokenType.SEMICOLON); return currTokenType; }
<YYINITIAL> {VARIABLE_PREFIX} { currTokenType = makeToken(SecretTokenType.VARIABLE_PREFIX); return currTokenType; }

<YYINITIAL> {L_BRACE} { currTokenType = makeToken(SecretTokenType.L_BRACE); return currTokenType; }
<YYINITIAL> {R_BRACE} { currTokenType = makeToken(SecretTokenType.R_BRACE); return currTokenType; }
<YYINITIAL> {L_PARENTHESES} { currTokenType = makeToken(SecretTokenType.L_PARENTHESES); return currTokenType; }
<YYINITIAL> {R_PARENTHESES} { currTokenType = makeToken(SecretTokenType.R_PARENTHESES); return currTokenType; }
<YYINITIAL> {L_BRACKET} { currTokenType = makeToken(SecretTokenType.L_BRACKET); return currTokenType; }
<YYINITIAL> {R_BRACKET} { currTokenType = makeToken(SecretTokenType.R_BRACKET); return currTokenType; }
<YYINITIAL> {BIND} { currTokenType = makeToken(SecretTokenType.BIND); return currTokenType; }

<YYINITIAL> {NAME_KEY} { currTokenType = makeToken(SecretTokenType.NAME_KEY); return currTokenType; }
<YYINITIAL> {EQUAL_TO} { currTokenType = makeToken(SecretTokenType.EQUAL_TO); return currTokenType; }
<YYINITIAL> {NOT_EQUAL_TO} { currTokenType = makeToken(SecretTokenType.NOT_EQUAL_TO); return currTokenType; }
<YYINITIAL> {CONNECT} { currTokenType = makeToken(SecretTokenType.CONNECT); return currTokenType; }
<YYINITIAL> {ELLIPSIS} { currTokenType = makeToken(SecretTokenType.ELLIPSIS); return currTokenType; }
<YYINITIAL> {DATA_SUFFIX} { currTokenType = makeToken(SecretTokenType.DATA_SUFFIX); return currTokenType; }
<YYINITIAL> {EXPRESSION_TAIL_OPERATOR} { currTokenType = makeToken(SecretTokenType.EXPRESSION_TAIL_OPERATOR); return currTokenType; }

<YYINITIAL> {LANG} { currTokenType = makeToken(SecretTokenType.LANG); return currTokenType; }

<YYINITIAL, URISTRING, URIEND, NAMESPACESEPARATOR, LOCALIDENTIFIER> . { return makeToken(SecretTokenType.BAD_CHARACTER); }
<YYINITIAL, URISTRING, URIEND, NAMESPACESEPARATOR, LOCALIDENTIFIER> {NEW_LINE_CHAR} { return makeToken(SecretTokenType.BAD_CHARACTER); }