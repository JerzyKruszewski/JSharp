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
            IExpressionSyntax expression = ParseTerm();
            SyntaxToken endOfFile = Match(TokenType.EndOfFileToken);
            return new SyntaxTree(_errors, expression, endOfFile);
        }

        private IExpressionSyntax ParseTerm()
        {
            IExpressionSyntax left = ParseFactor();

            while (Current.TokenType == TokenType.PlusToken ||
                   Current.TokenType == TokenType.MinusToken)
            {
                SyntaxToken operatorToken = NextToken();
                IExpressionSyntax right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private IExpressionSyntax ParseFactor()
        {
            IExpressionSyntax left = ParsePrimaryExpression();

            while (Current.TokenType == TokenType.StarToken ||
                   Current.TokenType == TokenType.SlashToken)
            {
                SyntaxToken operatorToken = NextToken();
                IExpressionSyntax right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private IExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.TokenType == TokenType.OpenParenthesesToken)
            {
                SyntaxToken openToken = NextToken();
                IExpressionSyntax expression = ParseTerm();
                SyntaxToken closeToken = Match(TokenType.CloseParenthesesToken);

                return new ParenthesizedExpressionSyntax(openParenthesesToken: openToken,
                                                         expression: expression,
                                                         closeParenthesesToken: closeToken);
            }

            return new NumberExpressionSyntax(Match(TokenType.NumberToken));
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

        private SyntaxToken Match(TokenType token)
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
