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
#if DEBUG
        private readonly List<string> _errors = new List<string>();
#endif

        public Parser(string text)
        {
            Lexer lexer = new Lexer(text);
            _tokens = lexer.GetFilteredTokens(TokenType.WhiteSpaceToken, TokenType.BadToken)
                           .ToArray();
            _errors.AddRange(lexer.Errors);
        }

        private SyntaxToken Current => Peek(0);
#if DEBUG
        public IEnumerable<string> Errors => _errors;
#endif

        public SyntaxTree Parse()
        {
            IExpressionSyntax expression = ParseExpression();
            SyntaxToken endOfFile = MatchToken(TokenType.EndOfFileToken);
            return new SyntaxTree(_errors, expression, endOfFile);
        }

        private IExpressionSyntax ParseExpression(int parentPrecedence = 0)
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
                IExpressionSyntax right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
        }

        private IExpressionSyntax HandleUnaryExpression(int parentPrecedence)
        {
            int unaryPrecedence = Current.TokenType.GetUnaryOperatorPrecedence();

            if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence)
            {
                SyntaxToken oparatorToken = NextToken();
                IExpressionSyntax operand = ParseExpression(unaryPrecedence);
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

            _errors.Add($"PARSER ERROR: Unexpected token {Current.TokenType} at {Current.Position} (Expected: {token})");
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
