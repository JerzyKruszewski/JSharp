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
        private SyntaxToken Current => Peek(0);

        public Parser(string text)
        {
            Lexer lexer = new Lexer(text);
            _tokens = lexer.GetFilteredTokens(TokenType.WhiteSpaceToken, TokenType.BadToken)
                           .ToArray();
        }

        public IExpressionSyntax ParseExpression()
        {
            IExpressionSyntax left = ParsePrimaryExpression();

            while (Current.TokenType == TokenType.PlusToken ||
                   Current.TokenType == TokenType.MinusToken ||
                   Current.TokenType == TokenType.StarToken ||
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
