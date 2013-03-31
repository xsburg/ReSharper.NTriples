// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretTokenType.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    // ReSharper disable InconsistentNaming
    public static partial class SecretTokenType
    {
        // todo: this is (arguably) wrong
        public static readonly TokenNodeType BAD_CHARACTER = new GenericTokenNodeType("BAD_CHARACTER");
        public static readonly TokenNodeType BIGNUM_LITERAL = new GenericTokenNodeType("BIGNUM_LITERAL", "123I");

        public static readonly TokenNodeType BYTEARRAY_STRING_LITERAL = new GenericTokenNodeType(
            "BYTEARRAY_STRING_LITERAL", "\"hello ASCII\"B");

        public static readonly TokenNodeType CHAMELEON = new GenericTokenNodeType("CHAMELEON");
        public static readonly TokenNodeType CHARACTER_LITERAL = new GenericTokenNodeType("CHARACTER_LITERAL", "'C'");

        public static readonly TokenNodeType C_STYLE_COMMENT = new CommentNodeType("C_STYLE_COMMENT", "(* comment *)");
        public static readonly TokenNodeType DECIMAL_LITERAL = new GenericTokenNodeType("DECIMAL_LITERAL", "42.0m");
        public static readonly TokenNodeType END_OF_LINE_COMMENT = new EndOfLineCommentNodeType();

        /// <summary>
        ///     Special token for some implementation details.
        ///     It should never be returned from the F# lexer.
        /// </summary>
        public static readonly TokenNodeType EOF = new GenericTokenNodeType("EOF");

        public static readonly TokenNodeType FLOAT_LITERAL = new GenericTokenNodeType("FLOAT_LITERAL", "42.0");

        // parser non-skippable
        public static readonly TokenNodeType IDENTIFIER = new IdentifierNodeType();
        public static readonly NodeTypeSet IDENTIFIER_KEYWORDS;
        public static readonly TokenNodeType INT16_LITERAL = new GenericTokenNodeType("INT16_LITERAL", "42s");
        public static readonly TokenNodeType INT32_LITERAL = new GenericTokenNodeType("INT32_LITERAL", "42l");
        public static readonly TokenNodeType INT64_LITERAL = new GenericTokenNodeType("INT64_LITERAL", "42L");
        public static readonly TokenNodeType INT8_LITERAL = new GenericTokenNodeType("INT8_LITERAL", "42y");
        public static readonly TokenNodeType INT_LITERAL = new GenericTokenNodeType("INT_LITERAL", "42");
        public static readonly NodeTypeSet KEYWORDS;
        public static readonly NodeTypeSet LITERALS;
        public static readonly TokenNodeType NATIVEINT_LITERAL = new GenericTokenNodeType("NATIVEINT_LITERAL", "42n");
        public static readonly TokenNodeType NEW_LINE = new NewLineNodeType();

        // todo: get rid of all of these, not all applicable
        public static readonly TokenNodeType PP_BAD_CHARACTER = new GenericTokenNodeType("PP_BAD_CHARACTER");
        public static readonly TokenNodeType PP_BAD_DIRECTIVE = new GenericTokenNodeType("PP_BAD_DIRECTIVE");
        public static readonly TokenNodeType PP_CONDITIONAL_SYMBOL = new GenericTokenNodeType("PP_CONDITIONAL_SYMBOL");
        public static readonly TokenNodeType PP_DEC_DIGITS = new GenericTokenNodeType("PP_DEC_DIGITS");
        public static readonly TokenNodeType PP_DEFAULT = new GenericTokenNodeType("PP_DEFAULT");
        public static readonly TokenNodeType PP_FILENAME = new GenericTokenNodeType("PP_FILENAME");
        public static readonly TokenNodeType PP_HIDDEN = new GenericTokenNodeType("PP_HIDDEN");
        public static readonly TokenNodeType PP_MESSAGE = new GenericTokenNodeType("PP_MESSAGE");
        public static readonly TokenNodeType PP_SKIPPED_LINE = new GenericTokenNodeType("PP_SKIPPED_LINE");
        public static readonly NodeTypeSet PREPROCESSOR;
        public static readonly TokenNodeType STRING_LITERAL = new GenericTokenNodeType("STRING_LITERAL", "\"Annuit cœptis\"");
        public static readonly NodeTypeSet TYPE_KEYWORDS;
        public static readonly TokenNodeType TYPE_VARIABLE = new GenericTokenNodeType("TYPE_VARIABLE", "'a");
        public static readonly TokenNodeType UINT16_LITERAL = new GenericTokenNodeType("UINT16_LITERAL", "42us");
        public static readonly TokenNodeType UINT32_LITERAL = new GenericTokenNodeType("UINT32_LITERAL", "42ul");
        public static readonly TokenNodeType UINT64_LITERAL = new GenericTokenNodeType("UINT64_LITERAL", "42UL");
        public static readonly TokenNodeType UINT8_LITERAL = new GenericTokenNodeType("UINT8_LITERAL", "42uy");
        public static readonly TokenNodeType UNATIVEINT_LITERAL = new GenericTokenNodeType("UNATIVEINT_LITERAL", "42un");
        public static readonly TokenNodeType WHITE_SPACE = new WhitespaceNodeType();

        static SecretTokenType()
        {
            KEYWORDS = new NodeTypeSet
                (
                ABSTRACT_KEYWORD,
                AND_KEYWORD,
                AS_KEYWORD,
                ASSERT_KEYWORD,
                BASE_KEYWORD,
                BEGIN_KEYWORD,
                CLASS_KEYWORD,
                DEFAULT_KEYWORD,
                DELEGATE_KEYWORD,
                DO_KEYWORD,
                DONE_KEYWORD,
                DOWNCAST_KEYWORD,
                DOWNTO_KEYWORD,
                ELIF_KEYWORD,
                ELSE_KEYWORD,
                END_KEYWORD,
                EXCEPTION_KEYWORD,
                EXTERN_KEYWORD,
                FALSE_KEYWORD,
                FINALLY_KEYWORD,
                FOR_KEYWORD,
                FUN_KEYWORD,
                FUNCTION_KEYWORD,
                GLOBAL_KEYWORD,
                IF_KEYWORD,
                IN_KEYWORD,
                INHERIT_KEYWORD,
                INLINE_KEYWORD,
                INTERFACE_KEYWORD,
                INTERNAL_KEYWORD,
                LAZY_KEYWORD,
                LET_KEYWORD,
                MATCH_KEYWORD,
                MEMBER_KEYWORD,
                MODULE_KEYWORD,
                MUTABLE_KEYWORD,
                NAMESPACE_KEYWORD,
                NEW_KEYWORD,
                NOT_KEYWORD,
                NULL_KEYWORD,
                OF_KEYWORD,
                OPEN_KEYWORD,
                OR_KEYWORD,
                OVERRIDE_KEYWORD,
                PRIVATE_KEYWORD,
                PUBLIC_KEYWORD,
                REC_KEYWORD,
                RETURN_KEYWORD,
                STATIC_KEYWORD,
                STRUCT_KEYWORD,
                THEN_KEYWORD,
                TO_KEYWORD,
                TRUE_KEYWORD,
                TRY_KEYWORD,
                TYPE_KEYWORD,
                UPCAST_KEYWORD,
                USE_KEYWORD,
                VAL_KEYWORD,
                VOID_KEYWORD,
                WHEN_KEYWORD,
                WHILE_KEYWORD,
                WITH_KEYWORD,
                YIELD_KEYWORD,
                // ML keywords
                ASR_ML_KEYWORD,
                LAND_ML_KEYWORD,
                LOR_ML_KEYWORD,
                LSL_ML_KEYWORD,
                LSR_ML_KEYWORD,
                LXOR_ML_KEYWORD,
                MOD_ML_KEYWORD,
                SIG_ML_KEYWORD,
                // reserved
                ATOMIC_RESERVED_KEYWORD,
                BREAK_RESERVED_KEYWORD,
                CHECKED_RESERVED_KEYWORD,
                COMPONENT_RESERVED_KEYWORD,
                CONST_RESERVED_KEYWORD,
                CONSTRAINT_RESERVED_KEYWORD,
                CONSTRUCTOR_RESERVED_KEYWORD,
                CONTINUE_RESERVED_KEYWORD,
                EAGER_RESERVED_KEYWORD,
                EVENT_RESERVED_KEYWORD,
                EXTERNAL_RESERVED_KEYWORD,
                FIXED_RESERVED_KEYWORD,
                FUNCTOR_RESERVED_KEYWORD,
                INCLUDE_RESERVED_KEYWORD,
                METHOD_RESERVED_KEYWORD,
                MIXIN_RESERVED_KEYWORD,
                OBJECT_RESERVED_KEYWORD,
                PARALLEL_RESERVED_KEYWORD,
                PROCESS_RESERVED_KEYWORD,
                PROTECTED_RESERVED_KEYWORD,
                PURE_RESERVED_KEYWORD,
                SEALED_RESERVED_KEYWORD,
                TAILCALL_RESERVED_KEYWORD,
                TRAIT_RESERVED_KEYWORD,
                VIRTUAL_RESERVED_KEYWORD,
                VOLATILE_RESERVED_KEYWORD,
                // object transformation ops (postfixed by OP because they are just too weird)
                BOX_OP,
                HASH_OP,
                SIZEOF_OP,
                TYPEOF_OP,
                TYPEDEFOF_OP,
                UNBOX_OP,
                REF_OP
                );

            IDENTIFIER_KEYWORDS = new NodeTypeSet
                (
                YIELD_KEYWORD,
                LET_KEYWORD
                );

            TYPE_KEYWORDS = new NodeTypeSet
                (
                VOID_KEYWORD
                );

            LITERALS = new NodeTypeSet
                (
                STRING_LITERAL,
                BYTEARRAY_STRING_LITERAL,
                CHARACTER_LITERAL,
                FLOAT_LITERAL,
                INT_LITERAL,
                INT8_LITERAL,
                UINT8_LITERAL,
                INT16_LITERAL,
                UINT16_LITERAL,
                INT32_LITERAL,
                UINT32_LITERAL,
                NATIVEINT_LITERAL,
                UNATIVEINT_LITERAL,
                INT64_LITERAL,
                UINT64_LITERAL,
                BIGNUM_LITERAL,
                DECIMAL_LITERAL,
                TRUE_KEYWORD,
                FALSE_KEYWORD,
                NULL_KEYWORD
                );

            PREPROCESSOR = new NodeTypeSet
                (
                PP_BAD_CHARACTER,
                PP_CONDITIONAL_SYMBOL,
                PP_SKIPPED_LINE,
                PP_DEC_DIGITS,
                PP_FILENAME,
                PP_DEFAULT,
                PP_HIDDEN,
                PP_MESSAGE,
                PP_BAD_DIRECTIVE,
                PP_SHARP,
                PP_DEFINE_DECLARATION,
                PP_UNDEF_DECLARATION,
                PP_IF_SECTION,
                PP_ELIF_SECTION,
                PP_ELSE_SECTION,
                PP_ENDIF,
                PP_R,
                PP_Q,
                PP_QUIT,
                PP_I,
                PP_HELP,
                PP_LINE,
                PP_LIGHT,
                PP_LOAD,
                PP_COMMA,
                PP_OR,
                PP_AND,
                PP_EQU,
                PP_NEQ,
                PP_NOT,
                PP_TRUE,
                PP_FALSE,
                PP_LPAR,
                PP_RPAR
                );

            TYPE_KEYWORDS = new NodeTypeSet
                (
                OBJ_KEYWORD,
                EXN_KEYWORD,
                NATIVEINT_KEYWORD,
                UNATIVEINT_KEYWORD,
                STRING_KEYWORD,
                FLOAT32_KEYWORD,
                SINGLE_KEYWORD,
                FLOAT_KEYWORD,
                DOUBLE_KEYWORD,
                SBYTE_KEYWORD,
                BYTE_KEYWORD,
                INT16_KEYWORD,
                UINT16_KEYWORD,
                INT32_KEYWORD,
                INT_KEYWORD,
                UINT32_KEYWORD,
                INT64_KEYWORD,
                UINT64_KEYWORD,
                CHAR_KEYWORD,
                BOOL_KEYWORD,
                DECIMAL_KEYWORD
                );
        }

        private class CommentNodeType : GenericTokenNodeType
        {
            public CommentNodeType(string s, string representation) : base(s, representation)
            {
            }

            public override bool IsComment
            {
                get
                {
                    return true;
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new Comment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private sealed class EndOfLineCommentNodeType : CommentNodeType
        {
            public EndOfLineCommentNodeType()
                : base("END_OF_LINE_COMMENT", "// comment")
            {
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                if (endOffset - startOffset > 2 && buffer[startOffset.Offset + 2] == '/' &&
                    (endOffset - startOffset == 3 || buffer[startOffset.Offset + 3] != '/'))
                {
                    return new DocComment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
                }
                return new Comment(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private class FixedTokenElement : SecretTokenBase
        {
            private readonly FixedTokenNodeType keywordTokenNodeType;

            public FixedTokenElement(FixedTokenNodeType keywordTokenNodeType)
            {
                this.keywordTokenNodeType = keywordTokenNodeType;
            }

            public override NodeType NodeType
            {
                get
                {
                    return this.keywordTokenNodeType;
                }
            }

            public override string GetText()
            {
                return this.keywordTokenNodeType.TokenRepresentation;
            }

            public override int GetTextLength()
            {
                return this.keywordTokenNodeType.TokenRepresentation.Length;
            }
        }

        private class FixedTokenNodeType : GenericTokenNodeType
        {
            private readonly string representation;

            public FixedTokenNodeType(string s, string representation = null)
                : base(s, representation)
            {
                this.representation = representation;
            }

            public override string TokenRepresentation
            {
                get
                {
                    return this.representation;
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new FixedTokenElement(this);
            }
        }

        private class GenericTokenNodeType : SecretTokenNodeType
        {
            private readonly string representation;

            public GenericTokenNodeType(string s, string representation = "")
                : base(s)
            {
                this.representation = representation;
            }

            public override string TokenRepresentation
            {
                get
                {
                    return this.representation;
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new SecretGenericToken(this, buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private sealed class IdentifierNodeType : SecretTokenNodeType
        {
            public IdentifierNodeType() : base("IDENTIFIER")
            {
            }

            public override string TokenRepresentation
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new Identifier(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private abstract class KeywordTokenNodeType : FixedTokenNodeType
        {
            public KeywordTokenNodeType(string s, string representation) : base(s, representation)
            {
            }
        }

        private sealed class NewLineNodeType : SecretTokenNodeType
        {
            public NewLineNodeType() : base("NEW_LINE")
            {
            }

            public override string TokenRepresentation
            {
                get
                {
                    return "\r\n";
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new NewLine(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }

        private abstract class SecretTokenNodeType : TokenNodeType
        {
            protected SecretTokenNodeType(string s)
                : base(s)
            {
            }

            public override bool IsComment
            {
                get
                {
                    return false;
                }
            }

            public override bool IsConstantLiteral
            {
                get
                {
                    return LITERALS[this];
                }
            }

            public override bool IsIdentifier
            {
                get
                {
                    return this == IDENTIFIER || this == TYPE_VARIABLE;
                }
            }

            public override bool IsKeyword
            {
                get
                {
                    return KEYWORDS[this];
                }
            }

            public override bool IsStringLiteral
            {
                get
                {
                    return this == STRING_LITERAL;
                }
            }

            public override bool IsWhitespace
            {
                get
                {
                    return this == WHITE_SPACE || this == NEW_LINE;
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                throw new InvalidOperationException();
            }
        }

        private sealed class WhitespaceNodeType : SecretTokenNodeType
        {
            public WhitespaceNodeType() : base("WHITE_SPACE")
            {
            }

            public override string TokenRepresentation
            {
                get
                {
                    return " ";
                }
            }

            public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
            {
                return new Whitespace(buffer.GetText(new TextRange(startOffset.Offset, endOffset.Offset)));
            }
        }
    }

    // ReSharper restore InconsistentNaming
}
