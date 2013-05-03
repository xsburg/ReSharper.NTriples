using JetBrains.Application;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Secret.Impl.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Psi.Secret.Parsing
{
    internal class SecretParser : SecretParserGenerated, ISecretParser
    {
        private readonly ILexer originalLexer;
        private readonly SeldomInterruptChecker myCheckForInterrupt;
        protected IPsiSourceFile SourceFile;

        public SecretParser(ILexer lexer)
        {
            this.originalLexer = lexer;
            myCheckForInterrupt = new SeldomInterruptChecker();
            setLexer(new SecretFilteringLexer(lexer));
        }

        public IFile ParseFile()
        {
            var file = (SecretFile)parseSecretFile();
            InsertMissingTokens(file, false);
            return file;
        }

        protected override TreeElement createToken()
        {
            LeafElementBase element = TreeElementFactory.CreateLeafElement(myLexer.TokenType, myLexer.Buffer, myLexer.TokenStart, myLexer.TokenEnd);
            SetOffset(element, myLexer.TokenStart);
            myLexer.Advance();
            return element;
        }

        public override TreeElement parsePrefix()
        {
            CompositeElement result = null;
            try
            {
                result = TreeElementFactory.CreateCompositeElement(ElementType.PREFIX);
                var tokenType = this.myLexer.TokenType;
                TreeElement tempParsingResult;
                if (tokenType == TokenType.PREFIX_IDENTIFIER)
                {
                    tempParsingResult = this.match(TokenType.PREFIX_IDENTIFIER);
                    result.AppendNewChild(tempParsingResult);
                }
                else if (tokenType == TokenType.IDENTIFIER || tokenType == TokenType.NAMESPACE_SEPARATOR)
                {
                    tokenType = this.myLexer.TokenType;
                    if (tokenType == TokenType.IDENTIFIER)
                    {
                        tempParsingResult = this.match(TokenType.IDENTIFIER);
                        result.AppendNewChild(tempParsingResult);
                    }
                    // The namespace separator parsing is moved into the uri identifier to be it's direct child
                }
                else
                {
                    if (result.firstChild == null)
                    {
                        result = null;
                    }
                    throw new UnexpectedToken(ErrorMessages.GetErrorMessage23());
                }
            }
            catch (SyntaxError e)
            {
                if (e.ParsingResult != null && result != null)
                {
                    result.AppendNewChild(e.ParsingResult);
                }
                if (result != null)
                {
                    e.ParsingResult = result;
                }
                throw;
            }
            return result;
        }

        public override TreeElement parsePrefixName()
        {
            CompositeElement result = null;
            try
            {
                result = TreeElementFactory.CreateCompositeElement(ElementType.PREFIX_NAME);
                TokenNodeType tokenType = this.myLexer.TokenType;
                TreeElement tempParsingResult;
                if (tokenType == TokenType.PREFIX_IDENTIFIER)
                {
                    tempParsingResult = this.match(TokenType.PREFIX_IDENTIFIER);
                    result.AppendNewChild(tempParsingResult);
                }
                else if (tokenType == TokenType.IDENTIFIER || tokenType == TokenType.NAMESPACE_SEPARATOR)
                {
                    tokenType = this.myLexer.TokenType;
                    if (tokenType == TokenType.IDENTIFIER)
                    {
                        tempParsingResult = this.match(TokenType.IDENTIFIER);
                        result.AppendNewChild(tempParsingResult);
                    }
                    // The namespace separator parsing is moved into the prefix declaration to be it's direct child
                }
                else
                {
                    if (result.firstChild == null)
                    {
                        result = null;
                    }
                    throw new UnexpectedToken(ErrorMessages.GetErrorMessage25());
                }
            }
            catch (SyntaxError e)
            {
                if (e.ParsingResult != null && result != null)
                {
                    result.AppendNewChild(e.ParsingResult);
                }
                if (result != null)
                {
                    e.ParsingResult = result;
                }
                throw;
            }
            return result;
        }

        public override TreeElement parsePrefixDeclaration()
        {
            CompositeElement result = null;
            try
            {
                result = TreeElementFactory.CreateCompositeElement(ElementType.PREFIX_DECLARATION);
                var tokenType = this.myLexer.TokenType;
                TreeElement tempParsingResult;
                if (tokenType == TokenType.PREFIX_KEYWORD)
                {
                    tempParsingResult = this.match(TokenType.PREFIX_KEYWORD);
                    result.AppendNewChild(tempParsingResult);
                }
                else if (tokenType == TokenType.STD_PREFIX_KEYWORD)
                {
                    tempParsingResult = this.match(TokenType.STD_PREFIX_KEYWORD);
                    result.AppendNewChild(tempParsingResult);
                }
                else
                {
                    if (result.firstChild == null)
                    {
                        result = null;
                    }
                    throw new UnexpectedToken(ErrorMessages.GetErrorMessage24());
                }

                if (myLexer.TokenType == TokenType.PREFIX_IDENTIFIER)
                {
                    // PREFIX_IDENTIFIER is converted into Prefix/INDENTIFIER and NAMESPACE_SEPARATOR
                    var prefixNameElement = TreeElementFactory.CreateCompositeElement(ElementType.PREFIX_NAME);
                    var identifier = TreeElementFactory.CreateLeafElement(
                        TokenType.IDENTIFIER, this.myLexer.Buffer, this.myLexer.TokenStart, this.myLexer.TokenEnd - 1);
                    SetOffset(identifier, myLexer.TokenStart);
                    prefixNameElement.AppendNewChild(identifier);
                    result.AppendNewChild(prefixNameElement);

                    var namespaceSeparator = TreeElementFactory.CreateLeafElement(
                        TokenType.NAMESPACE_SEPARATOR, this.myLexer.Buffer, this.myLexer.TokenEnd - 1, this.myLexer.TokenEnd);
                    SetOffset(namespaceSeparator, this.myLexer.TokenEnd - 1);
                    this.myLexer.Advance();
                    result.AppendNewChild(namespaceSeparator);
                }
                else
                {
                    tempParsingResult = this.parsePrefixName();
                    result.AppendNewChild(tempParsingResult);
                    // Namespace separator parsing moved here from parsePrefixName
                    tempParsingResult = this.match(TokenType.NAMESPACE_SEPARATOR);
                    result.AppendNewChild(tempParsingResult);
                }


                tempParsingResult = this.match(TokenType.URI_BEGIN);
                result.AppendNewChild(tempParsingResult);
                tempParsingResult = this.match(TokenType.URI_STRING);
                result.AppendNewChild(tempParsingResult);
                tempParsingResult = this.match(TokenType.URI_END);
                result.AppendNewChild(tempParsingResult);
            }
            catch (SyntaxError e)
            {
                if (e.ParsingResult != null && result != null)
                {
                    result.AppendNewChild(e.ParsingResult);
                }
                if (result != null)
                {
                    e.ParsingResult = result;
                }
                throw;
            }
            return result;
        }

        public override TreeElement parseUriIdentifier()
        {
            CompositeElement result = null;
            try
            {
                result = TreeElementFactory.CreateCompositeElement(ElementType.URI_IDENTIFIER);
                var tokenType = this.myLexer.TokenType;
                TreeElement tempParsingResult;
                if (tokenType == TokenType.PREFIX_IDENTIFIER)
                {
                    // PREFIX_IDENTIFIER is converted into Prefix/INDENTIFIER and NAMESPACE_SEPARATOR
                    var prefixElement = TreeElementFactory.CreateCompositeElement(ElementType.PREFIX);
                    var identifier = TreeElementFactory.CreateLeafElement(
                        TokenType.IDENTIFIER, this.myLexer.Buffer, this.myLexer.TokenStart, this.myLexer.TokenEnd - 1);
                    SetOffset(identifier, myLexer.TokenStart);
                    prefixElement.AppendNewChild(identifier);
                    result.AppendNewChild(prefixElement);

                    var namespaceSeparator = TreeElementFactory.CreateLeafElement(
                        TokenType.NAMESPACE_SEPARATOR, this.myLexer.Buffer, this.myLexer.TokenEnd - 1, this.myLexer.TokenEnd);
                    SetOffset(namespaceSeparator, this.myLexer.TokenEnd - 1);
                    this.myLexer.Advance();
                    result.AppendNewChild(namespaceSeparator);
                    tempParsingResult = this.parseLocalName();
                    result.AppendNewChild(tempParsingResult);
                }
                else if (tokenType == TokenType.IDENTIFIER || tokenType == TokenType.NAMESPACE_SEPARATOR)
                {
                    tempParsingResult = this.parsePrefix();
                    result.AppendNewChild(tempParsingResult);
                    // Namespace separator parsing moved here from parsePrefix
                    tempParsingResult = this.match(TokenType.NAMESPACE_SEPARATOR);
                    result.AppendNewChild(tempParsingResult);
                    tempParsingResult = this.parseLocalName();
                    result.AppendNewChild(tempParsingResult);
                }
                else if (tokenType == TokenType.URI_BEGIN)
                {
                    tempParsingResult = this.match(TokenType.URI_BEGIN);
                    result.AppendNewChild(tempParsingResult);
                    tokenType = this.myLexer.TokenType;
                    if (tokenType == TokenType.URI_STRING)
                    {
                        tempParsingResult = this.parseUriString();
                        result.AppendNewChild(tempParsingResult);
                    }
                    tempParsingResult = this.match(TokenType.URI_END);
                    result.AppendNewChild(tempParsingResult);
                }
                else
                {
                    if (result.firstChild == null)
                    {
                        result = null;
                    }
                    throw new UnexpectedToken(ErrorMessages.GetErrorMessage31());
                }
            }
            catch (SyntaxError e)
            {
                if (e.ParsingResult != null && result != null)
                {
                    result.AppendNewChild(e.ParsingResult);
                }
                if (result != null)
                {
                    e.ParsingResult = result;
                }
                throw;
            }
            return result;
        }

        private void InsertMissingTokens(TreeElement result, bool trimMissingTokens)
        {
            SecretMissingTokensInserter.Run(result, originalLexer, this, trimMissingTokens, myCheckForInterrupt);
        }

        public override TreeElement parseSecretFile()
        {
            return this.ParseSecretFile(true);
        }

        public TreeElement ParseSecretFile(bool isFileReal)
        {
            TreeElement file = base.parseSecretFile();
            var psiFile = file as SecretFile;
            if (psiFile != null)
            {
                psiFile.SetSourceFile(SourceFile);
                psiFile.CreatePrefixesSymbolTable();
            }

            return psiFile;
        }
    }
}