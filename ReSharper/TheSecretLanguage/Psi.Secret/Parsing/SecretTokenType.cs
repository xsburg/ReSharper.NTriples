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

        public static readonly TokenNodeType STRING_LITERAL = new GenericTokenNodeType("STRING_LITERAL", "\"Annuit cœptis\"");
        public static readonly NodeTypeSet TYPE_KEYWORDS;
        public static readonly TokenNodeType TYPE_VARIABLE = new GenericTokenNodeType("TYPE_VARIABLE", "'a");
        public static readonly TokenNodeType WHITE_SPACE = new WhitespaceNodeType();

        static SecretTokenType()
        {
            KEYWORDS = new NodeTypeSet
                (
                    NULL_KEYWORD,
                    TRUE_KEYWORD,
                    FALSE_KEYWORD,
                    HAS_KEYWORD,
                    IS_KEYWORD,
                    FOR_ALL_KEYWORD,
                    FOR_SOME_KEYWORD,
                    A_KEYWORD,
                    OF_KEYWORD,
                    PREFIX_KEYWORD,
                    STD_PREFIX_KEYWORD,
                    EXTENSION_KEYWORD,
                    USING_KEYWORD,
                    DEFAULT_AXIS_KEYWORD,
                    AXIS_KEYWORD,
                    FUNCTOR_KEYWORD,
                    META_KEYWORD,
                    IN_KEYWORD,
                    OUT_KEYWORD,
                    SELECT_KEYWORD,
                    FROM_KEYWORD,
                    NOT_KEYWORD,
                    IF_KEYWORD,
                    TRY_KEYWORD,
                    OR_KEYWORD,
                    IF_NOT_KEYWORD,
                    THEN_KEYWORD,
                    ELSE_KEYWORD,
                    ONCE_KEYWORD
                );

            IDENTIFIER_KEYWORDS = new NodeTypeSet
                (
                //YIELD_KEYWORD,
                //LET_KEYWORD
                );

            TYPE_KEYWORDS = new NodeTypeSet
                (
                //VOID_KEYWORD
                );

            LITERALS = new NodeTypeSet
                (
                STRING_LITERAL,
                FLOAT_LITERAL,
                INT_LITERAL,
                INT8_LITERAL,
                INT16_LITERAL,
                INT32_LITERAL,
                NATIVEINT_LITERAL,
                INT64_LITERAL,
                TRUE_KEYWORD,
                FALSE_KEYWORD,
                NULL_KEYWORD
                );

            TYPE_KEYWORDS = new NodeTypeSet
                (
                /*OBJ_KEYWORD,
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
                DECIMAL_KEYWORD*/
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
                    return "identifier";
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
