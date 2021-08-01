using JSharp.Enums;
using JSharp.Interfaces;
using JSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
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
            IExpressionSyntax left = ParsePrimaryExpression();

            while (true)
            {
                int precedence = GetBinaryOperatorPrecedence(Current.TokenType);

                if (precedence == 0 || precedence <= parentPrecedence)
                {
                    return left;
                }

                SyntaxToken operatorToken = NextToken();
                IExpressionSyntax right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
        }

        private static int GetBinaryOperatorPrecedence(TokenType token)
        {
            return token switch
            {
                TokenType.PlusToken or TokenType.MinusToken => 1,
                TokenType.StarToken or TokenType.SlashToken => 2,
                _ => 0
            };
        }

        private IExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.TokenType == TokenType.OpenParenthesesToken)
            {
                SyntaxToken openToken = NextToken();
                IExpressionSyntax expression = ParseExpression();
                SyntaxToken closeToken = MatchToken(TokenType.CloseParenthesesToken);

                return new ParenthesizedExpressionSyntax(openParenthesesToken: openToken,
                                                         expression: expression,
                                                         closeParenthesesToken: closeToken);
            }

            return new LiteralExpressionSyntax(MatchToken(TokenType.NumberToken));
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
