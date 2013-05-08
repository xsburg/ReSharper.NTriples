// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   SecretLexerGenerated.cs
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using JetBrains;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace ReSharper.NTriples.Parsing
{
    public partial class SecretLexerGenerated : ILexer<SecretLexerState>
    {
        protected static readonly Dictionary<string, TokenNodeType> keywords = new Dictionary<string, TokenNodeType>();
        protected static readonly Dictionary<NodeType, string> tokenTypesToText = new Dictionary<NodeType, string>();
        private static readonly HashSet<string> bangKeywords = new HashSet<string>();
        private TokenNodeType currTokenType;

        static SecretLexerGenerated()
        {
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
