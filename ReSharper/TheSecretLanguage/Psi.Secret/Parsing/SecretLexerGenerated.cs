// ***********************************************************************
// <author>Stephan B</author>
// <copyright company="Comindware">
//   Copyright (c) Comindware 2010-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLexerGenerated.cs
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    public partial class SecretLexerGenerated : ILexer<SecretLexerState>
    {
        protected static readonly Dictionary<string, TokenNodeType> keywords = new Dictionary<string, TokenNodeType>();
        protected static readonly Dictionary<NodeType, string> tokenTypesToText = new Dictionary<NodeType, string>();
        private static readonly HashSet<string> bangKeywords = new HashSet<string>();
        private TokenNodeType currTokenType;

        static SecretLexerGenerated()
        {
            Action<string, TokenNodeType> add = (k, v) => keywords.Add(k.ToLowerInvariant(), v);
            Action<string, TokenNodeType> addBang = (k, v) =>
            {
                add(k, v);
                bangKeywords.Add(k.ToLowerInvariant().Substring(0, k.Length - 1));
            };

            // Computation expression keywords
            addBang("LET!", SecretTokenType.LET_BANG_KEYWORD);
            addBang("USE!", SecretTokenType.USE_BANG_KEYWORD);
            addBang("DO!", SecretTokenType.DO_BANG_KEYWORD);
            addBang("YIELD!", SecretTokenType.YIELD_BANG_KEYWORD);
            addBang("RETURN!", SecretTokenType.RETURN_BANG_KEYWORD);

            add("ABSTRACT", SecretTokenType.ABSTRACT_KEYWORD);
            add("AND", SecretTokenType.AND_KEYWORD);
            add("AS", SecretTokenType.AS_KEYWORD);
            add("ASSERT", SecretTokenType.ASSERT_KEYWORD);
            add("BASE", SecretTokenType.BASE_KEYWORD);
            add("BEGIN", SecretTokenType.BEGIN_KEYWORD);
            add("CLASS", SecretTokenType.CLASS_KEYWORD);
            add("DEFAULT", SecretTokenType.DEFAULT_KEYWORD);
            add("DELEGATE", SecretTokenType.DELEGATE_KEYWORD);
            add("DO", SecretTokenType.DO_KEYWORD);
            add("DONE", SecretTokenType.DONE_KEYWORD);
            add("DOWNCAST", SecretTokenType.DOWNCAST_KEYWORD);
            add("DOWNTO", SecretTokenType.DOWNTO_KEYWORD);
            add("ELIF", SecretTokenType.ELIF_KEYWORD);
            add("ELSE", SecretTokenType.ELSE_KEYWORD);
            add("END", SecretTokenType.END_KEYWORD);
            add("EXCEPTION", SecretTokenType.EXCEPTION_KEYWORD);
            add("EXTERN", SecretTokenType.EXTERN_KEYWORD);
            add("FINALLY", SecretTokenType.FINALLY_KEYWORD);
            add("FOR", SecretTokenType.FOR_KEYWORD);
            add("FUN", SecretTokenType.FUN_KEYWORD);
            add("FUNCTION", SecretTokenType.FUNCTION_KEYWORD);
            add("GLOBAL", SecretTokenType.GLOBAL_KEYWORD);
            add("IF", SecretTokenType.IF_KEYWORD);
            add("IN", SecretTokenType.IN_KEYWORD);
            add("INHERIT", SecretTokenType.INHERIT_KEYWORD);
            add("INLINE", SecretTokenType.INLINE_KEYWORD);
            add("INTERFACE", SecretTokenType.INTERFACE_KEYWORD);
            add("INTERNAL", SecretTokenType.INTERNAL_KEYWORD);
            add("LAZY", SecretTokenType.LAZY_KEYWORD);
            add("LET", SecretTokenType.LET_KEYWORD);
            add("MATCH", SecretTokenType.MATCH_KEYWORD);
            add("MEMBER", SecretTokenType.MEMBER_KEYWORD);
            add("MODULE", SecretTokenType.MODULE_KEYWORD);
            add("MUTABLE", SecretTokenType.MUTABLE_KEYWORD);
            add("NAMESPACE", SecretTokenType.NAMESPACE_KEYWORD);
            add("NEW", SecretTokenType.NEW_KEYWORD);
            add("NOT", SecretTokenType.NOT_KEYWORD);
            add("NULL", SecretTokenType.NULL_KEYWORD);
            add("OF", SecretTokenType.OF_KEYWORD);
            add("OPEN", SecretTokenType.OPEN_KEYWORD);
            add("OR", SecretTokenType.OR_KEYWORD);
            add("OVERRIDE", SecretTokenType.OVERRIDE_KEYWORD);
            add("PRIVATE", SecretTokenType.PRIVATE_KEYWORD);
            add("PUBLIC", SecretTokenType.PUBLIC_KEYWORD);
            add("REC", SecretTokenType.REC_KEYWORD);
            add("RETURN", SecretTokenType.RETURN_KEYWORD);
            add("STATIC", SecretTokenType.STATIC_KEYWORD);
            add("STRUCT", SecretTokenType.STRUCT_KEYWORD);
            add("THEN", SecretTokenType.THEN_KEYWORD);
            add("TO", SecretTokenType.TO_KEYWORD);
            add("TRUE", SecretTokenType.TRUE_KEYWORD);
            add("TRY", SecretTokenType.TRY_KEYWORD);
            add("TYPE", SecretTokenType.TYPE_KEYWORD);
            add("UPCAST", SecretTokenType.UPCAST_KEYWORD);
            add("USE", SecretTokenType.USE_KEYWORD);
            add("VAL", SecretTokenType.VAL_KEYWORD);
            add("VOID", SecretTokenType.VOID_KEYWORD);
            add("WHEN", SecretTokenType.WHEN_KEYWORD);
            add("WHILE", SecretTokenType.WHILE_KEYWORD);
            add("WITH", SecretTokenType.WITH_KEYWORD);
            add("YIELD", SecretTokenType.YIELD_KEYWORD);

            // ML keywords
            add("ASR", SecretTokenType.ASR_ML_KEYWORD);
            add("LAND", SecretTokenType.LAND_ML_KEYWORD);
            add("LOR", SecretTokenType.LOR_ML_KEYWORD);
            add("LSL", SecretTokenType.LSL_ML_KEYWORD);
            add("LSR", SecretTokenType.LSR_ML_KEYWORD);
            add("LXOR", SecretTokenType.LXOR_ML_KEYWORD);
            add("MOD", SecretTokenType.MOD_ML_KEYWORD);
            add("SIG", SecretTokenType.SIG_ML_KEYWORD);

            // reserved 
            add("ATOMIC", SecretTokenType.ATOMIC_RESERVED_KEYWORD);
            add("BREAK", SecretTokenType.BREAK_RESERVED_KEYWORD);
            add("CHECKED", SecretTokenType.CHECKED_RESERVED_KEYWORD);
            add("COMPONENT", SecretTokenType.COMPONENT_RESERVED_KEYWORD);
            add("CONST", SecretTokenType.CONST_RESERVED_KEYWORD);
            add("CONSTRAINT", SecretTokenType.CONSTRAINT_RESERVED_KEYWORD);
            add("CONSTRUCTOR", SecretTokenType.CONSTRUCTOR_RESERVED_KEYWORD);
            add("CONTINUE", SecretTokenType.CONTINUE_RESERVED_KEYWORD);
            add("EAGER", SecretTokenType.EAGER_RESERVED_KEYWORD);
            add("EVENT", SecretTokenType.EVENT_RESERVED_KEYWORD);
            add("EXTERNAL", SecretTokenType.EXTERNAL_RESERVED_KEYWORD);
            add("FIXED", SecretTokenType.FIXED_RESERVED_KEYWORD);
            add("FUNCTOR", SecretTokenType.FUNCTOR_RESERVED_KEYWORD);
            add("INCLUDE", SecretTokenType.INCLUDE_RESERVED_KEYWORD);
            add("METHOD", SecretTokenType.METHOD_RESERVED_KEYWORD);
            add("MIXIN", SecretTokenType.MIXIN_RESERVED_KEYWORD);
            add("OBJECT", SecretTokenType.OBJECT_RESERVED_KEYWORD);
            add("PARALLEL", SecretTokenType.PARALLEL_RESERVED_KEYWORD);
            add("PROCESS", SecretTokenType.PROCESS_RESERVED_KEYWORD);
            add("PROTECTED", SecretTokenType.PROTECTED_RESERVED_KEYWORD);
            add("PURE", SecretTokenType.PURE_RESERVED_KEYWORD);
            add("SEALED", SecretTokenType.SEALED_RESERVED_KEYWORD);
            add("TAILCALL", SecretTokenType.TAILCALL_RESERVED_KEYWORD);
            add("TRAIT", SecretTokenType.TRAIT_RESERVED_KEYWORD);
            add("VIRTUAL", SecretTokenType.VIRTUAL_RESERVED_KEYWORD);
            add("VOLATILE", SecretTokenType.VOLATILE_RESERVED_KEYWORD);

            // object transformation ops (postfixed by OP because they are just too weird)
            add("BOX", SecretTokenType.BOX_OP);
            add("HASH", SecretTokenType.HASH_OP);
            add("SIZEOF", SecretTokenType.SIZEOF_OP);
            add("TYPEOF", SecretTokenType.TYPEOF_OP);
            add("TYPEDEFOF", SecretTokenType.TYPEDEFOF_OP);
            add("UNBOX", SecretTokenType.UNBOX_OP);
            add("REF", SecretTokenType.REF_OP);
        }

        public IBuffer Buffer
        {
            get
            {
                return this.yy_buffer;
            }
        }

        public SecretLexerState CurrentPosition
        {
            get
            {
                SecretLexerState tokenPosition;
                tokenPosition.currTokenType = this.currTokenType;
                tokenPosition.yy_buffer_index = this.yy_buffer_index;
                tokenPosition.yy_buffer_start = this.yy_buffer_start;
                tokenPosition.yy_buffer_end = this.yy_buffer_end;
                tokenPosition.yy_lexical_state = this.yy_lexical_state;
                return tokenPosition;
            }
            set
            {
                this.currTokenType = value.currTokenType;
                this.yy_buffer_index = value.yy_buffer_index;
                this.yy_buffer_start = value.yy_buffer_start;
                this.yy_buffer_end = value.yy_buffer_end;
                this.yy_lexical_state = value.yy_lexical_state;
            }
        }

        public int TokenEnd
        {
            get
            {
                this.locateToken();
                return this.yy_buffer_end;
            }
        }

        public int TokenStart
        {
            get
            {
                this.locateToken();
                return this.yy_buffer_start;
            }
        }

        public TokenNodeType TokenType
        {
            get
            {
                this.locateToken();
                return this.currTokenType;
            }
        }

        object ILexer.CurrentPosition
        {
            get
            {
                return this.CurrentPosition;
            }
            set
            {
                this.CurrentPosition = (SecretLexerState)value;
            }
        }

        public void Advance()
        {
            this.locateToken();
            this.currTokenType = null;
        }

        public void Start()
        {
            this.Start(0, this.yy_buffer.Length, YYINITIAL);
        }

        internal TokenNodeType getKeyword()
        {
            var text = this.yytext();

            // if it's not a bang keyword, just yield it
            if (bangKeywords.Contains(text) &&
                this.yy_buffer_end < this.yy_buffer.Length &&
                this.yy_buffer[this.yy_buffer_end] == '!')
            {
                this.yy_buffer_end++;
                this.yy_buffer_index++;
                return keywords.GetValue(text + '!');
            }
            return keywords.GetValueSafe(text);
        }

        private void Start(int startOffset, int endOffset, uint state)
        {
            this.yy_buffer_index = startOffset;
            this.yy_buffer_start = startOffset;
            this.yy_buffer_end = startOffset;
            this.yy_eof_pos = endOffset;
            this.yy_lexical_state = (int)state;
            this.currTokenType = null;
        }

        private void locateToken()
        {
            if (this.currTokenType == null)
            {
                this.currTokenType = this._locateToken();
            }
        }

        private TokenNodeType makeToken(TokenNodeType type)
        {
            return this.currTokenType = type;
        }
    }
}
