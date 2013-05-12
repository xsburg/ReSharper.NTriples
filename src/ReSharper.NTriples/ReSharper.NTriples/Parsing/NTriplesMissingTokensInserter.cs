// ***********************************************************************
// <author>Stephan Burguchev</author>
// <copyright company="Stephan Burguchev">
//   Copyright (c) Stephan Burguchev 2012-2013. All rights reserved.
// </copyright>
// <summary>
//   NTriplesMissingTokensInserter.cs
// </summary>
// ***********************************************************************

using System;
using JetBrains.Application;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.DataStructures;
using ReSharper.NTriples.Impl;
using ReSharper.NTriples.Tree;

namespace ReSharper.NTriples.Parsing
{
    internal sealed class NTriplesMissingTokensInserter : MissingTokenInserterBase
    {
        private readonly ILexer myLexer;
        private new readonly DataIntern<string> myWhitespaceIntern = new DataIntern<string>();

        private NTriplesMissingTokensInserter(
            ILexer lexer, ITokenOffsetProvider offsetProvider, SeldomInterruptChecker interruptChecker)
            : base(offsetProvider, interruptChecker)
        {
            this.myLexer = lexer;
        }

        public static void Run(
            TreeElement node,
            ILexer lexer,
            ITokenOffsetProvider offsetProvider,
            bool trimTokens,
            SeldomInterruptChecker interruptChecker)
        {
            Assertion.Assert(node.parent == null, "node.parent == null");

            var root = node as CompositeElement;
            if (root == null)
            {
                return;
            }

            var inserter = new NTriplesMissingTokensInserter(lexer, offsetProvider, interruptChecker);
            lexer.Start();

            if (trimTokens)
            {
                using (var container = new DummyContainer(root))
                {
                    inserter.Run(container);
                }
            }
            else
            {
                var terminator = new EofToken(lexer.Buffer);
                root.AppendNewChild(terminator);
                inserter.Run(root);
                root.DeleteChildRange(terminator, terminator);
            }
        }

        protected override void ProcessLeafElement(TreeElement element)
        {
            int leafOffset = this.GetLeafOffset(element).Offset;

            // Check if some tokens are missed before this leaf
            if (this.myLexer.TokenType != null && this.myLexer.TokenStart < leafOffset)
            {
                // Find out the right place to insert tokens to
                TreeElement anchor = element;
                CompositeElement parent = anchor.parent;
                while (anchor == parent.firstChild && parent.parent != null)
                {
                    anchor = parent;
                    parent = parent.parent;
                }

                // proceed with inserting tokens
                while (this.myLexer.TokenType != null && this.myLexer.TokenStart < leafOffset)
                {
                    LeafElementBase token = this.CreateMissingToken();

                    parent.AddChildBefore(token, anchor);

                    this.myLexer.Advance();
                }
            }

            // skip all tokens which lie inside given leaf element
            int leafEndOffset = leafOffset + element.GetTextLength();
            if ((element is IClosedChameleonBody) && (this.myLexer is CachingLexer))
            {
                ((CachingLexer)this.myLexer).FindTokenAt(leafEndOffset);
            }
            else
            {
                while (this.myLexer.TokenType != null && this.myLexer.TokenStart < leafEndOffset)
                {
                    this.myLexer.Advance();
                }
            }
        }

        private LeafElementBase CreateMissingToken()
        {
            TokenNodeType tokenType = this.myLexer.TokenType;
            if (tokenType == NTriplesTokenType.WHITE_SPACE || tokenType == NTriplesTokenType.NEW_LINE)
            {
                string text = this.myLexer.GetCurrTokenText();
                if (tokenType == NTriplesTokenType.WHITE_SPACE)
                {
                    return new Whitespace(this.myWhitespaceIntern.Intern(text));
                }
                return new NewLine(text);
            }

            return TreeElementFactory.CreateLeafElement(this.myLexer);
        }

        private sealed class DummyContainer : CompositeElement, IDisposable
        {
            public DummyContainer(TreeElement element)
            {
                this.AppendNewChild(element);
            }

            public override PsiLanguageType Language
            {
                get
                {
                    return NTriplesLanguage.Instance;
                }
            }

            public override NodeType NodeType
            {
                get
                {
                    return DummyNodeType.Instance;
                }
            }

            public void Dispose()
            {
                this.DeleteChildRange(this.firstChild, this.firstChild);
            }

            private sealed class DummyNodeType : CompositeNodeType
            {
                public static readonly NodeType Instance = new DummyNodeType();

                private DummyNodeType()
                    : base("DummyContainer")
                {
                }

                //public override PsiLanguageType LanguageType { get { return UnknownLanguage.Instance; } }
                public override CompositeElement Create()
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private sealed class EofToken : BindedToBufferLeafElement
        {
            public EofToken(IBuffer buffer)
                : base(NTriplesTokenType.EOF, buffer, new TreeOffset(buffer.Length), new TreeOffset(buffer.Length))
            {
            }

            public override PsiLanguageType Language
            {
                get
                {
                    return NTriplesLanguage.Instance;
                }
            }
        }
    }
}
