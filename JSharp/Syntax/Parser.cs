using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax
{
    public class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Parser(string text)
        {
            Lexer lexer = new Lexer(text);
            _tokens = lexer.GetFilteredTokens(TokenType.WhiteSpaceToken, TokenType.BadToken)
                           .ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken Current => Peek(0);
        public DiagnosticBag Diagnostics => _diagnostics;

        public SyntaxTree Parse()
        {
            IExpressionSyntax expression = ParseExpression();
            SyntaxToken endOfFile = MatchToken(TokenType.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFile);
        }

        private IExpressionSyntax ParseExpression()
        {
            return ParseDefinitionExpression();
        }

        private IExpressionSyntax ParseDefinitionExpression()
        {
            if (Peek(1).TokenType == TokenType.IdentifierToken)
            {
                SyntaxToken variableToken;
                SyntaxToken identifierToken;

                switch (Current.TokenType)
                {
                    case TokenType.IntVariableKeyword:
                        variableToken = NextToken();
                        identifierToken = NextToken();
                        return new DefinitionExpressionSyntax(variableToken, identifierToken, typeof(int));
                    case TokenType.BoolVariableKeyword:
                        variableToken = NextToken();
                        identifierToken = NextToken();
                        return new DefinitionExpressionSyntax(variableToken, identifierToken, typeof(bool));
                    case TokenType.ObjectVariableKeyword:
                        variableToken = NextToken();
                        identifierToken = NextToken();
                        return new DefinitionExpressionSyntax(variableToken, identifierToken, typeof(object));
                    default:
                        break;
                }
            }

            return ParseAssignmentExpression();
        }

        private IExpressionSyntax ParseAssignmentExpression()
        {
            if (Current.TokenType == TokenType.IdentifierToken &&
                Peek(1).TokenType == TokenType.EqualsToken)
            {
                SyntaxToken identifierToken = NextToken();
                SyntaxToken operatorToken = NextToken();
                IExpressionSyntax right = ParseAssignmentExpression();

                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            }

            return ParseBinaryExpression();
        }

        private IExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            IExpressionSyntax left = HandleUnaryExpression(parentPrecedence);

            while (true)
            {
                int precedence = Current.TokenType.GetBinaryOperatorPrecedence();

                if (precedence == 0 || precedence <= parentPrecedence)
                {
                    return left;
                }

                SyntaxToken operatorToken = NextToken();
                IExpressionSyntax right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
        }

        private IExpressionSyntax HandleUnaryExpression(int parentPrecedence)
        {
            int unaryPrecedence = Current.TokenType.GetUnaryOperatorPrecedence();

            if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence)
            {
                SyntaxToken oparatorToken = NextToken();
                IExpressionSyntax operand = ParseBinaryExpression(unaryPrecedence);
                return new UnaryExpressionSyntax(oparatorToken, operand);
            }

            return ParsePrimaryExpression();
        }

        private IExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.TokenType)
            {
                case TokenType.OpenParenthesesToken:
                {
                    SyntaxToken openToken = NextToken();
                    IExpressionSyntax expression = ParseExpression();
                    SyntaxToken closeToken = MatchToken(TokenType.CloseParenthesesToken);

                    return new ParenthesizedExpressionSyntax(openParenthesesToken: openToken,
                                                             expression: expression,
                                                             closeParenthesesToken: closeToken);
                }
                case TokenType.TrueKeyword:
                case TokenType.FalseKeyword:
                {
                    SyntaxToken keywordToken = NextToken();
                    bool value = keywordToken.TokenType == TokenType.TrueKeyword;

                    return new LiteralExpressionSyntax(keywordToken, value);
                }
                case TokenType.IdentifierToken:
                {
                    SyntaxToken identifierToken = NextToken();
                    return new NameExpressionSyntax(identifierToken);
                }
                default:
                    return new LiteralExpressionSyntax(MatchToken(TokenType.NumberToken));
            }
        }

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;

            if (index >= _tokens.Length)
            {
                return _tokens[^1];
            }

            return _tokens[index];
        }

        private SyntaxToken MatchToken(TokenType token)
        {
            if (Current.TokenType == token)
            {
                return NextToken();
            }

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.TokenType, token, source: "PARSER");
            return new SyntaxToken(token, Current.Position, null, null);
        }

        // Go to next token, but return pervious token
        // Usefull in parsing oparator tokens
        private SyntaxToken NextToken()
        {
            SyntaxToken token = Current;
            _position++;
            return token;
        }
    }
}
