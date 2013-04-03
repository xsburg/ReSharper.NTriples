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

BIND=(->)
L_PARENTHESES=(\()
R_PARENTHESES=(\))
L_BRACE=({)
R_BRACE=(})
L_BRACKET=(\[)
R_BRACKET=(\])

HAS=(@has)
IS=(@is)
FOR_ALL=(@forAll)
FOR_SOME=(@forSome)
A=(a)
OF=(@of)
IMPLIES=(=>)
SAME_AS=(=)
PREFIX=(@prefix)
STD_PREFIX=(@std_prefix)
EXTENSION=(@extension)
USING=(@using)
DEFAULT_AXIS=(@axis-default)

AXIS=(@?axis)
FUNCTOR=(@?functor)
META=(@?meta)
IN=(@?in|@for)
OUT=(@?out)
SELECT=(@?select)
FROM=(@?from)
NOT=(@?not)
IF=(@?if)
TRY=(@?try)
OR=(@?or)
IF_NOT=(@?if-not)
THEN=(@?then)
ELSE=(@?else)
ONCE=(@?once)

NAME_KEY=(:-)
EQUAL_TO=(:=|==)
NOT_EQUAL_TO=(!=)
CONNECT=(<->)
ELLIPSIS=(\.\.\.|\u2026)

URI_BEGIN=(<)
URI_END=(>)
URI_STRING=([^>]*)

NAME_START_CHAR=([a-zA-Z_0-9\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD])
NAME_CHAR=({NAME_START_CHAR}|[\-\u00B7\u0300-\u036F\u203F-\u2040])
NAME=({NAME_START_CHAR}{NAME_CHAR}*)

TRUE=(true)
FALSE=(false)
NULL=(null)
DOT=(\.)
SEMICOLON=(;)

SINGLE_LINE_COMMENT=("#"{INPUT_CHARACTER}*)

DECIMAL_DIGIT=[0-9]
EXPONENT_PART=([eE](([+-])?({DECIMAL_DIGIT})*))
DOUBLE={INTEGER}{DOT}{DECIMAL_DIGIT}+(EXPONENT_PART)?
INTEGER=[+\-]?{DECIMAL_DIGIT}+

HEX_DIGIT=({DECIMAL_DIGIT}|[A-Fa-f])
SIMPLE_ESCAPE_SEQUENCE=(\\[\"\\rnt])
UNICODE_ESCAPE_SEQUENCE=((\\u{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT})|(\\U{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}))
SINGLE_CHARACTER=[^\\"\u0000-\u001f]
CHARACTER=({SINGLE_CHARACTER}|{SIMPLE_ESCAPE_SEQUENCE}|{UNICODE_ESCAPE_SEQUENCE})
REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*\")
TRIPLE_QUOTED_STRING=\"\"\"{SIMPLE_CHAR_OR_QUOTE_CHAR_SEQUENCE}*\"\"\"
SIMPLE_CHAR_OR_QUOTE_CHAR_SEQUENCE=([^\"]|(\"[^\"])|(\"\"[^\"]))
STRING_LITERAL=({REGULAR_STRING_LITERAL}|{TRIPLE_QUOTED_STRING})

LANG=(@[a-z]+(-[a-z0-9]+)*)

NS_PREFIX=({NAME}?:)
Q_NAME=({NS_PREFIX}{NAME}?)
VARIABLE=(\?{NAME}?)





















DELIMITED_COMMENT_SECTION=([^\*]|({ASTERISKS}[^\*\/]))
UNFINISHED_DELIMITED_COMMENT="(*"{DELIMITED_COMMENT_SECTION}*
DELIMITED_COMMENT={UNFINISHED_DELIMITED_COMMENT}{ASTERISKS}"/"


DECIMAL_DIGIT_CHARACTER={UNICODE_ND}
CONNECTING_CHARACTER={UNICODE_PC}
COMBINING_CHARACTER=({UNICODE_MC}|{UNICODE_MN})
FORMATTING_CHARACTER={UNICODE_CF}
LETTER_CHARACTER=({UNICODE_LL}|{UNICODE_LM}|{UNICODE_LO}|{UNICODE_LT}|{UNICODE_LU}|{UNICODE_NL}|{UNICODE_ESCAPE_SEQUENCE})

SINGLE_QUOTE=\'
IDENTIFIER_SURROUNDED_BY_DOUBLE_BACKTICK=``([^`\n\r\t]|`[^`\n\r\t])+``
IDENTIFIER_START_CHARACTER=({LETTER_CHARACTER}|(\u005F))
IDENTIFIER_PART_CHARACTER=({LETTER_CHARACTER}|{DECIMAL_DIGIT_CHARACTER}|{CONNECTING_CHARACTER}|{COMBINING_CHARACTER}|{FORMATTING_CHARACTER}|{SINGLE_QUOTE})
IDENTIFIER=({LETTER_CHARACTER}|{IDENTIFIER_START_CHARACTER}{IDENTIFIER_PART_CHARACTER}+|{IDENTIFIER_SURROUNDED_BY_DOUBLE_BACKTICK})

TYPE_VARIABLE=\'{IDENTIFIER}


NOT_NUMBER_SIGN=[^#]
PP_NUMBER_SIGN=#

PP_BAD_DIRECTIVE=(define|undef|if|elif|else|endif|error|warning|region|endregion|line|pragma)({IDENTIFIER}|{DECIMAL_DIGIT})

PP_FILENAME_CHARACTER=[^\"\r\n\u0085\u2028\u2029]

PP_FILENAME=(\"{PP_FILENAME_CHARACTER}+\")
PP_BAD_FILENAME=(\"{PP_FILENAME_CHARACTER}+)
PP_DEC_DIGITS={DECIMAL_DIGIT}+

PP_CONDITIONAL_SYMBOL={IDENTIFIER}

WHITE_SPACE=({WHITE_SPACE_CHAR}+)
END_LINE={NOT_NEW_LINE}*(({PP_NEW_LINE_PAIR})|({PP_NEW_LINE_CHAR}))

PP_MESSAGE={INPUT_CHARACTER}*




%state URISTRING
%state URIEND

%%

<YYINITIAL> {URI_BEGIN} { yybegin(URISTRING); currTokenType = makeToken (SecretTokenType.URI_BEGIN); return currTokenType; }
<URISTRING> {URI_STRING} { yybegin(URIEND); currTokenType = makeToken (SecretTokenType.URI_STRING); return currTokenType; }
<URIEND> {URI_END} { yybegin(YYINITIAL); currTokenType = makeToken (SecretTokenType.URI_END); return currTokenType; }

<YYINITIAL> {SINGLE_LINE_COMMENT} { return makeToken(SecretTokenType.END_OF_LINE_COMMENT); }

<YYINITIAL> {WHITE_SPACE} { currTokenType = makeToken(SecretTokenType.WHITE_SPACE); return currTokenType; }

<YYINITIAL> {INTEGER} { currTokenType = makeToken(SecretTokenType.INTEGER); return currTokenType; }
<YYINITIAL> {DOUBLE} { currTokenType = makeToken(SecretTokenType.DOUBLE); return currTokenType; }
<YYINITIAL> {STRING_LITERAL} { currTokenType = makeToken(SecretTokenType.STRING_LITERAL); return currTokenType; }

<YYINITIAL> {TRUE} { currTokenType = makeToken(SecretTokenType.TRUE_KEYWORD); return currTokenType; }
<YYINITIAL> {FALSE} { currTokenType = makeToken(SecretTokenType.FALSE_KEYWORD); return currTokenType; }
<YYINITIAL> {NULL} { currTokenType = makeToken(SecretTokenType.NULL_KEYWORD); return currTokenType; }
<YYINITIAL> {DOT} { currTokenType = makeToken(SecretTokenType.DOT); return currTokenType; }
<YYINITIAL> {SEMICOLON} { currTokenType = makeToken(SecretTokenType.SEMICOLON); return currTokenType; }
<YYINITIAL> {LANG} { currTokenType = makeToken(SecretTokenType.LANG); return currTokenType; }
<YYINITIAL> {Q_NAME} { currTokenType = makeToken(SecretTokenType.Q_NAME); return currTokenType; }
<YYINITIAL> {NS_PREFIX} { currTokenType = makeToken(SecretTokenType.NS_PREFIX); return currTokenType; }
<YYINITIAL> {VARIABLE} { currTokenType = makeToken(SecretTokenType.VARIABLE); return currTokenType; }

<YYINITIAL> . { return makeToken(SecretTokenType.BAD_CHARACTER); }