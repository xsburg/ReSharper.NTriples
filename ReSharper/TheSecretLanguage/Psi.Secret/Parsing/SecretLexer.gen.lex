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

DELIMITED_COMMENT_SECTION=([^\*]|({ASTERISKS}[^\*\/]))
UNFINISHED_DELIMITED_COMMENT="(*"{DELIMITED_COMMENT_SECTION}*
DELIMITED_COMMENT={UNFINISHED_DELIMITED_COMMENT}{ASTERISKS}"/"

SINGLE_LINE_COMMENT=("//"{INPUT_CHARACTER}*)

DECIMAL_DIGIT=[0-9]
HEX_DIGIT=({DECIMAL_DIGIT}|[A-Fa-f])
OCT_DIGIT=[0-7]
BIN_DIGIT=[0-1]
INTEGER_TYPE_SUFFIX=([UuLl]|UL|Ul|uL|ul|LU|lU|Lu|lu)


DECIMAL_INTEGER_LITERAL=({DECIMAL_DIGIT}+{INTEGER_TYPE_SUFFIX}?)
HEXADECIMAL_INTEGER_LITERAL=(0[Xx]({HEX_DIGIT})*{INTEGER_TYPE_SUFFIX}?)


EXPONENT_PART=([eE](([+-])?({DECIMAL_DIGIT})*))
REAL_TYPE_SUFFIX=[FfDdMm]
REAL_LITERAL=({DECIMAL_DIGIT}*"."{DECIMAL_DIGIT}+({EXPONENT_PART})?{REAL_TYPE_SUFFIX}?)|({DECIMAL_DIGIT}+({EXPONENT_PART}|({EXPONENT_PART}?{REAL_TYPE_SUFFIX})))

SINGLE_CHARACTER=[^\'\\\u0085\u2028\u2029\u000D\u000A]
SIMPLE_ESCAPE_SEQUENCE=(\\[\'\"\\0abfnrtv])
HEXADECIMAL_ESCAPE_SEQUENCE=(\\x{HEX_DIGIT}({HEX_DIGIT}|{HEX_DIGIT}{HEX_DIGIT}|{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT})?)
UNICODE_ESCAPE_SEQUENCE=((\\u{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT})|(\\U{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}))
CHARACTER=({SINGLE_CHARACTER}|{SIMPLE_ESCAPE_SEQUENCE}|{HEXADECIMAL_ESCAPE_SEQUENCE}|{UNICODE_ESCAPE_SEQUENCE})
BAD_ESCAPE_SEQUENCE=((\\u)|(\\[^\'\"\\0abfnrtv]))
CHARACTER_LITERAL=\'({CHARACTER})\'
UNFINISHED_CHARACTER_LITERAL=\'(({CHARACTER})|({BAD_ESCAPE_SEQUENCE}(\'?))|\')
EXCEEDING_CHARACTER_LITERAL=\'{CHARACTER}({CHARACTER}|{BAD_ESCAPE_SEQUENCE})+\'
BYTECHAR_LITERAL={CHARACTER_LITERAL}B

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

REGULAR_STRING_LITERAL_CHARACTER=({SINGLE_REGULAR_STRING_LITERAL_CHARACTER}|{SIMPLE_ESCAPE_SEQUENCE}|{HEXADECIMAL_ESCAPE_SEQUENCE}|{UNICODE_ESCAPE_SEQUENCE})
SINGLE_REGULAR_STRING_LITERAL_CHARACTER=[^\"\\\u0085\u2028\u2029\u000D\u000A]
REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*\")

VERBATIM_STRING_LITERAL=(\@\"{VERBATIM_STRING_LITERAL_CHARACTER}*\")
VERBATIM_STRING_LITERAL_CHARACTER=({SINGLE_VERBATIM_STRING_LITERAL_CHARACTER}|{QUOTE_ESCAPE_SEQUENCE})
SINGLE_VERBATIM_STRING_LITERAL_CHARACTER=[^\"]
QUOTE_ESCAPE_SEQUENCE=(\"\")

TRIPLE_QUOTED_STRING=\"\"\"{SIMPLE_CHAR_OR_QUOTE_CHAR_SEQUENCE}*\"\"\"
SIMPLE_CHAR_OR_QUOTE_CHAR_SEQUENCE=([^\"]|(\"[^\"])|(\"\"[^\"]))

STRING_LITERAL=({REGULAR_STRING_LITERAL}|{VERBATIM_STRING_LITERAL}|{TRIPLE_QUOTED_STRING})
UNFINISHED_REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*)
UNFINISHED_VERBATIM_STRING_LITERAL=(@\"{VERBATIM_STRING_LITERAL_CHARACTER}*)
ERROR_REGULAR_STRING_LITERAL=(\"{REGULAR_STRING_LITERAL_CHARACTER}*{BAD_ESCAPE_SEQUENCE}({BAD_ESCAPE_SEQUENCE}|{REGULAR_STRING_LITERAL_CHARACTER})*\"?)
ERROR_STRING_LITERAL=({UNFINISHED_REGULAR_STRING_LITERAL}|{UNFINISHED_VERBATIM_STRING_LITERAL}|{ERROR_REGULAR_STRING_LITERAL})

BYTEARRAY_STRING_LITERAL=({REGULAR_STRING_LITERAL}|{VERBATIM_STRING_LITERAL})B

HEX_LITERAL=(0[xX]{HEX_DIGIT}+)
OCT_LITERAL=(0[oO]{OCT_DIGIT}+)
BIN_LITERAL=(0[bB]{BIN_DIGIT}+)
UINT_LITERAL = (({DECIMAL_DIGIT}+)|{HEX_LITERAL}|{OCT_LITERAL}|{BIN_LITERAL})
INT_LITERAL = -?{UINT_LITERAL}

INT8_LITERAL = {INT_LITERAL}y
UINT8_LITERAL = {UINT_LITERAL}uy

INT16_LITERAL = {INT_LITERAL}s
UINT16_LITERAL = {UINT_LITERAL}us
INT32_LITERAL = {INT_LITERAL}l
UINT32_LITERAL = {UINT_LITERAL}ul?
NATIVEINT_LITERAL = {INT_LITERAL}n
UNATIVEINT_LITERAL = {UINT_LITERAL}un
INT64_LITERAL = {INT_LITERAL}L
UINT64_LITERAL = {UINT_LITERAL}[uU]L

BIGNUM_LITERAL=({DECIMAL_DIGIT}+[QRZING])

DECIMAL_LITERAL=((-?{DECIMAL_DIGIT}+)|{FLOAT_LITERAL})[Mm]

FLOAT_LITERAL = ((-?{DECIMAL_DIGIT}+"."{DECIMAL_DIGIT}*)|(-?{DECIMAL_DIGIT}+("."{DECIMAL_DIGIT}*)?([eE])(([+-])?){DECIMAL_DIGIT}+))
			
FLOAT32_LITERAL = (({FLOAT_LITERAL}[fF])|({HEX_LITERAL}lf)|({OCT_LITERAL}lf)|({BIN_LITERAL}lf))
FLOAT64_LITERAL = (({FLOAT_LITERAL})|({HEX_LITERAL}LF)|({OCT_LITERAL}LF)|({BIN_LITERAL}LF))

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

%state PPSYMBOL
%state PPDIGITS
%state PPMESSAGE

%% 

<YYINITIAL> {INT_LITERAL} { currTokenType = makeToken (SecretTokenType.INT_LITERAL); return currTokenType; }
<YYINITIAL> {INT8_LITERAL} { currTokenType = makeToken (SecretTokenType.INT8_LITERAL); return currTokenType; }
<YYINITIAL> {UINT8_LITERAL} { currTokenType = makeToken (SecretTokenType.UINT8_LITERAL); return currTokenType; }
<YYINITIAL> {INT16_LITERAL} { currTokenType = makeToken (SecretTokenType.INT16_LITERAL); return currTokenType; }
<YYINITIAL> {UINT16_LITERAL} { currTokenType = makeToken (SecretTokenType.UINT16_LITERAL); return currTokenType; }
<YYINITIAL> {INT32_LITERAL} { currTokenType = makeToken (SecretTokenType.INT32_LITERAL); return currTokenType; }
<YYINITIAL> {UINT32_LITERAL} { currTokenType = makeToken (SecretTokenType.UINT32_LITERAL); return currTokenType; }
<YYINITIAL> {NATIVEINT_LITERAL} { currTokenType = makeToken (SecretTokenType.NATIVEINT_LITERAL); return currTokenType; }
<YYINITIAL> {UNATIVEINT_LITERAL} { currTokenType = makeToken (SecretTokenType.UNATIVEINT_LITERAL); return currTokenType; }
<YYINITIAL> {INT64_LITERAL} { currTokenType = makeToken (SecretTokenType.INT64_LITERAL); return currTokenType; }
<YYINITIAL> {UINT64_LITERAL} { currTokenType = makeToken (SecretTokenType.UINT64_LITERAL); return currTokenType; }

<YYINITIAL> {BIGNUM_LITERAL} { return makeToken(SecretTokenType.BIGNUM_LITERAL); }
<YYINITIAL> {DECIMAL_LITERAL} { return makeToken(SecretTokenType.DECIMAL_LITERAL); }

<YYINITIAL> {FLOAT32_LITERAL} { currTokenType = makeToken (SecretTokenType.FLOAT_LITERAL); return currTokenType; }
<YYINITIAL> {FLOAT64_LITERAL} { currTokenType = makeToken (SecretTokenType.FLOAT_LITERAL); return currTokenType; }

<YYINITIAL> {NEW_LINE_PAIR} { yybegin(YYINITIAL); return makeToken(SecretTokenType.NEW_LINE); }
<YYINITIAL> {NEW_LINE_CHAR} { yybegin(YYINITIAL); return makeToken(SecretTokenType.NEW_LINE); }

<YYINITIAL> {DELIMITED_COMMENT}  { return makeToken(SecretTokenType.C_STYLE_COMMENT); }
<YYINITIAL> {SINGLE_LINE_COMMENT}  { yybegin(YYINITIAL); return makeToken(SecretTokenType.END_OF_LINE_COMMENT); }
<YYINITIAL> {UNFINISHED_DELIMITED_COMMENT} { return makeToken(SecretTokenType.C_STYLE_COMMENT); }

<YYINITIAL> {CHARACTER_LITERAL} { currTokenType = makeToken (SecretTokenType.CHARACTER_LITERAL); return currTokenType; }
<YYINITIAL> {BYTECHAR_LITERAL} { currTokenType = makeToken (SecretTokenType.UINT8_LITERAL); return currTokenType; }
<YYINITIAL> {TYPE_VARIABLE} { currTokenType = makeToken (SecretTokenType.TYPE_VARIABLE); return currTokenType; }
<YYINITIAL> {BYTEARRAY_STRING_LITERAL} { currTokenType = makeToken (SecretTokenType.BYTEARRAY_STRING_LITERAL); return currTokenType; }
<YYINITIAL> {STRING_LITERAL} { currTokenType = makeToken (SecretTokenType.STRING_LITERAL); return currTokenType; }

<YYINITIAL> {WHITE_SPACE} { currTokenType = makeToken(SecretTokenType.WHITE_SPACE); return currTokenType; }

<YYINITIAL> {IDENTIFIER} { currTokenType = makeToken(getKeyword() ?? SecretTokenType.IDENTIFIER); return currTokenType; }

<YYINITIAL> "@" { currTokenType = makeToken(SecretTokenType.AT); return currTokenType; }
<YYINITIAL> "_" { currTokenType = makeToken(SecretTokenType.UNDERSCORE); return currTokenType; }
<YYINITIAL> "<@" { currTokenType = makeToken(SecretTokenType.LQUOTE); return currTokenType; }
<YYINITIAL> "<@@" { currTokenType = makeToken(SecretTokenType.LDQUOTE); return currTokenType; }
<YYINITIAL> "<|" { currTokenType = makeToken(SecretTokenType.BACKWARD_PIPE); return currTokenType; }
<YYINITIAL> "|>" { currTokenType = makeToken(SecretTokenType.FORWARD_PIPE); return currTokenType; }
<YYINITIAL> "@>" { currTokenType = makeToken(SecretTokenType.RQUOTE); return currTokenType; }
<YYINITIAL> "@@>" { currTokenType = makeToken(SecretTokenType.RDQUOTE); return currTokenType; }
<YYINITIAL> "&" { currTokenType = makeToken(SecretTokenType.AMP); return currTokenType; }
<YYINITIAL> "&&" { currTokenType = makeToken(SecretTokenType.AMP_AMP); return currTokenType; }
<YYINITIAL> "||" { currTokenType = makeToken(SecretTokenType.BAR_BAR); return currTokenType; }
<YYINITIAL> "\'" { currTokenType = makeToken(SecretTokenType.QUOTE); return currTokenType; }
<YYINITIAL> "(" { currTokenType = makeToken(SecretTokenType.LPAREN); return currTokenType; }
<YYINITIAL> ")" { currTokenType = makeToken(SecretTokenType.RPAREN); return currTokenType; }
<YYINITIAL> "**" { currTokenType = makeToken(SecretTokenType.STAR_STAR); return currTokenType; }
<YYINITIAL> "*" { currTokenType = makeToken(SecretTokenType.STAR); return currTokenType; }
<YYINITIAL> "," { currTokenType = makeToken(SecretTokenType.COMMA); return currTokenType; }
<YYINITIAL> "->" { currTokenType = makeToken(SecretTokenType.RARROW); return currTokenType; }
<YYINITIAL> "?" { currTokenType = makeToken(SecretTokenType.QMARK); return currTokenType; }
<YYINITIAL> "??" { currTokenType = makeToken(SecretTokenType.QMARK_QMARK); return currTokenType; }
<YYINITIAL> ".." { currTokenType = makeToken(SecretTokenType.DOT_DOT); return currTokenType; }
<YYINITIAL> "." { currTokenType = makeToken(SecretTokenType.DOT); return currTokenType; }
<YYINITIAL> ":" { currTokenType = makeToken(SecretTokenType.COLON); return currTokenType; }
<YYINITIAL> "::" { currTokenType = makeToken(SecretTokenType.COLON_COLON); return currTokenType; }
<YYINITIAL> ":>" { currTokenType = makeToken(SecretTokenType.COLON_GREATER); return currTokenType; }
<YYINITIAL> "@>." { currTokenType = makeToken(SecretTokenType.RQUOTE_DOT); return currTokenType; }
<YYINITIAL> "@@>." { currTokenType = makeToken(SecretTokenType.RDQUOTE_DOT); return currTokenType; }
<YYINITIAL> ">|]" { currTokenType = makeToken(SecretTokenType.GREATER_BAR_RBRACK); return currTokenType; }
<YYINITIAL> ":?>" { currTokenType = makeToken(SecretTokenType.COLON_QMARK_GREATER); return currTokenType; }
<YYINITIAL> ":?" { currTokenType = makeToken(SecretTokenType.COLON_QMARK); return currTokenType; }
<YYINITIAL> ":=" { currTokenType = makeToken(SecretTokenType.COLON_EQUALS); return currTokenType; }
<YYINITIAL> ";;" { currTokenType = makeToken(SecretTokenType.SEMICOLON_SEMICOLON); return currTokenType; }
<YYINITIAL> ";" { currTokenType = makeToken(SecretTokenType.SEMICOLON); return currTokenType; }
<YYINITIAL> "<-" { currTokenType = makeToken(SecretTokenType.LARROW); return currTokenType; }
<YYINITIAL> "=" { currTokenType = makeToken(SecretTokenType.EQUALS); return currTokenType; }
<YYINITIAL> "[" { currTokenType = makeToken(SecretTokenType.LBRACK); return currTokenType; }
<YYINITIAL> "[|" { currTokenType = makeToken(SecretTokenType.LBRACK_BAR); return currTokenType; }
<YYINITIAL> "[<" { currTokenType = makeToken(SecretTokenType.LBRACK_LESS); return currTokenType; }
<YYINITIAL> "]" { currTokenType = makeToken(SecretTokenType.RBRACK); return currTokenType; }
<YYINITIAL> "|]" { currTokenType = makeToken(SecretTokenType.BAR_RBRACK); return currTokenType; }
<YYINITIAL> ">]" { currTokenType = makeToken(SecretTokenType.GREATER_RBRACK); return currTokenType; }
<YYINITIAL> "{" { currTokenType = makeToken(SecretTokenType.LBRACE); return currTokenType; }
<YYINITIAL> "|" { currTokenType = makeToken(SecretTokenType.BAR); return currTokenType; }
<YYINITIAL> "}" { currTokenType = makeToken(SecretTokenType.RBRACE); return currTokenType; }
<YYINITIAL> "$" { currTokenType = makeToken(SecretTokenType.DOLLAR); return currTokenType; }
<YYINITIAL> "%%" { currTokenType = makeToken(SecretTokenType.DPERCENT_OP); return currTokenType; }
<YYINITIAL> "-" { currTokenType = makeToken(SecretTokenType.MINUS); return currTokenType; }
<YYINITIAL> "~" { currTokenType = makeToken(SecretTokenType.TILDE); return currTokenType; }
<YYINITIAL> "`" { currTokenType = makeToken(SecretTokenType.RESERVED); return currTokenType; }
<YYINITIAL> "<>" { currTokenType = makeToken(SecretTokenType.LESS_GREATER); return currTokenType; }
<YYINITIAL> "<=" { currTokenType = makeToken(SecretTokenType.LESS_EQUALS); return currTokenType; }
<YYINITIAL> ">=" { currTokenType = makeToken(SecretTokenType.GREATER_EQUALS); return currTokenType; }
<YYINITIAL> ">>" { currTokenType = makeToken(SecretTokenType.GREATER_GREATER); return currTokenType; }
<YYINITIAL> "<<" { currTokenType = makeToken(SecretTokenType.LESS_LESS); return currTokenType; }
<YYINITIAL> "#" { currTokenType = makeToken(SecretTokenType.HASH); return currTokenType; }

<YYINITIAL> "{" { return makeToken(SecretTokenType.LBRACE); }
<YYINITIAL> "}" { return makeToken(SecretTokenType.RBRACE); }
<YYINITIAL> "[" { return makeToken(SecretTokenType.LBRACKET); }
<YYINITIAL> "]" { return makeToken(SecretTokenType.RBRACKET); }
<YYINITIAL> "(" { return makeToken(SecretTokenType.LPARENTH); }
<YYINITIAL> ")" { return makeToken(SecretTokenType.RPARENTH); }
<YYINITIAL> "." { return makeToken(SecretTokenType.DOT); }
<YYINITIAL> "," { return makeToken(SecretTokenType.COMMA); }
<YYINITIAL> ":" { return makeToken(SecretTokenType.COLON); }
<YYINITIAL> ";" { return makeToken(SecretTokenType.SEMICOLON); }

<YYINITIAL> "+" { return makeToken(SecretTokenType.PLUS); }
<YYINITIAL> "-" { return makeToken(SecretTokenType.MINUS); }
<YYINITIAL> "*" { return makeToken(SecretTokenType.STAR); }
<YYINITIAL> "/" { return makeToken(SecretTokenType.DIV); }
<YYINITIAL> "%" { return makeToken(SecretTokenType.PERCENT); }
<YYINITIAL> "&" { return makeToken(SecretTokenType.AND); }
<YYINITIAL> "|" { return makeToken(SecretTokenType.OR); }
<YYINITIAL> "^^^" { return makeToken(SecretTokenType.BITWISE_XOR); }
<YYINITIAL> "!" { return makeToken(SecretTokenType.EXCLAMATION_OP); }
<YYINITIAL> "~" { return makeToken(SecretTokenType.TILDE); }

<YYINITIAL> "=" { return makeToken(SecretTokenType.EQ); }
<YYINITIAL> "<" { currTokenType = makeToken(SecretTokenType.LESS); return currTokenType; }
<YYINITIAL> ">" { currTokenType = makeToken(SecretTokenType.GREATER); return currTokenType; }
<YYINITIAL> "?" { return makeToken(SecretTokenType.QUEST); }

<YYINITIAL> {REAL_LITERAL}  { return makeToken(SecretTokenType.FLOAT_LITERAL); }
<YYINITIAL> {CHARACTER_LITERAL}  { return makeToken(SecretTokenType.CHARACTER_LITERAL); }
<YYINITIAL> {UNFINISHED_CHARACTER_LITERAL} { return makeToken(SecretTokenType.CHARACTER_LITERAL); }

<YYINITIAL> {STRING_LITERAL}  { return makeToken(SecretTokenType.STRING_LITERAL); }
<YYINITIAL> {ERROR_STRING_LITERAL}  { return makeToken(SecretTokenType.STRING_LITERAL); }

<YYINITIAL> . { currTokenType = makeToken(SecretTokenType.BAD_CHARACTER); return currTokenType; } 

<PPSYMBOL, PPDIGITS, PPMESSAGE> {WHITE_SPACE} { return makeToken (SecretTokenType.WHITE_SPACE); }

<YYINITIAL> "#undef" { yybegin(PPSYMBOL); return makeToken(SecretTokenType.PP_UNDEF_DECLARATION); }
<YYINITIAL> "#define" { yybegin(PPSYMBOL); return makeToken(SecretTokenType.PP_DEFINE_DECLARATION); }
<YYINITIAL> "#if" { yybegin(PPSYMBOL); return makeToken(SecretTokenType.PP_IF_SECTION); }
<YYINITIAL> "#elif" { yybegin(PPSYMBOL); return makeToken(SecretTokenType.PP_ELIF_SECTION); }
<YYINITIAL> "#else" { yybegin(PPSYMBOL); return makeToken(SecretTokenType.PP_ELSE_SECTION); }
<YYINITIAL> "#endif" { yybegin(PPSYMBOL); return makeToken(SecretTokenType.PP_ENDIF); }
<YYINITIAL> "#line" { yybegin(PPDIGITS); return makeToken(SecretTokenType.PP_LINE); }
<YYINITIAL> "#nowarn" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_NO_WARN); }
<YYINITIAL> "#r" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_R); }
<YYINITIAL> "#reference" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_REFERENCE); }
<YYINITIAL> "#q" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_Q); }
<YYINITIAL> "#quit" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_QUIT); }
<YYINITIAL> "#I" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_I); }
<YYINITIAL> "#Include" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_INCLUDE); }
<YYINITIAL> "#help" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_HELP); }
<YYINITIAL> "#load" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_LOAD); }
<YYINITIAL> "#light" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_LIGHT); }
<YYINITIAL> "#time" { yybegin(PPMESSAGE); return makeToken(SecretTokenType.PP_TIME); }

<PPSYMBOL> "||" { return makeToken(SecretTokenType.PP_OR); }
<PPSYMBOL> "&&" { return makeToken(SecretTokenType.PP_AND); }
<PPSYMBOL> "==" { return makeToken(SecretTokenType.PP_EQU); }
<PPSYMBOL> "!=" { return makeToken(SecretTokenType.PP_NEQ); }
<PPSYMBOL> "!" { return makeToken(SecretTokenType.PP_NOT); }
<PPSYMBOL> "true" { return makeToken(SecretTokenType.PP_TRUE); }
<PPSYMBOL> "false" { return makeToken (SecretTokenType.PP_FALSE); }
<PPSYMBOL> "(" { return makeToken(SecretTokenType.PP_LPAR); }
<PPSYMBOL> ")" { return makeToken(SecretTokenType.PP_RPAR); }
<PPSYMBOL> {PP_CONDITIONAL_SYMBOL} { return makeToken(SecretTokenType.PP_CONDITIONAL_SYMBOL); }

<PPDIGITS> {PP_DEC_DIGITS} { return makeToken(SecretTokenType.PP_DEC_DIGITS); }
<PPDIGITS> "," { return makeToken(SecretTokenType.PP_COMMA); }
<PPDIGITS> {PP_FILENAME} { return makeToken(SecretTokenType.PP_FILENAME); }
<PPDIGITS> {PP_BAD_FILENAME} { return makeToken(SecretTokenType.PP_BAD_CHARACTER); }
<PPDIGITS> "default" { return makeToken(SecretTokenType.PP_DEFAULT); }
<PPDIGITS> "hidden" { return makeToken(SecretTokenType.PP_HIDDEN); }
<PPDIGITS> {IDENTIFIER} { return makeToken(SecretTokenType.PP_BAD_CHARACTER); }

<PPMESSAGE> {PP_MESSAGE} { return makeToken(SecretTokenType.PP_MESSAGE); }

<PPSYMBOL, PPDIGITS, PPMESSAGE> {SINGLE_LINE_COMMENT} { return makeToken(SecretTokenType.END_OF_LINE_COMMENT); }
<PPSYMBOL, PPDIGITS, PPMESSAGE> {NEW_LINE_PAIR} { yybegin(YYINITIAL); return makeToken(SecretTokenType.NEW_LINE); }
<PPSYMBOL, PPDIGITS, PPMESSAGE> {NEW_LINE_CHAR} { yybegin(YYINITIAL); return makeToken(SecretTokenType.NEW_LINE); }

<YYINITIAL> . { return makeToken(SecretTokenType.BAD_CHARACTER); }
<PPSYMBOL, PPDIGITS, PPMESSAGE> . { return makeToken(SecretTokenType.PP_BAD_CHARACTER); }