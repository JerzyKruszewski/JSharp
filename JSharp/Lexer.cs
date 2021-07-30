using JSharp.Enums;
using JSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// C# but with mandatory Object Calisthenics
namespace JSharp
{
    public class Lexer
    {
        private readonly string _text;
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        private char Current
        {
            get
            {
                return (_position >= _text.Length || _position < 0) ? '\0' : _text[_position];
            }
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(TokenType.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                int startPos = _position;

                while (char.IsDigit(Current))
                {
                    Next();
                }

                int length = _position - startPos;

                string text = _text.Substring(startPos, length);
                _ = int.TryParse(text, out int value);

                return new SyntaxToken(TokenType.NumberToken, startPos, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                int startPos = _position;

                while (char.IsWhiteSpace(Current))
                {
                    Next();
                }

                int length = _position - startPos;

                string text = _text.Substring(startPos, length);

                return new SyntaxToken(TokenType.WhiteSpaceToken, startPos, text, null);
            }

            if (Current == '+')
            {
                return new SyntaxToken(TokenType.PlusToken, _position++, "+", null);
            }
            if (Current == '-')
            {
                return new SyntaxToken(TokenType.MinusToken, _position++, "-", null);
            }
            if (Current == '*')
            {
                return new SyntaxToken(TokenType.StarToken, _position++, "*", null);
            }
            if (Current == '/')
            {
                return new SyntaxToken(TokenType.SlashToken, _position++, "/", null);
            }
            if (Current == '(')
            {
                return new SyntaxToken(TokenType.OpenParenthesesToken, _position++, "(", null);
            }
            if (Current == ')')
            {
                return new SyntaxToken(TokenType.CloseParenthesesToken, _position++, ")", null);
            }

            return new SyntaxToken(TokenType.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }

        private void Next()
        {
            _position++;
        }
    }
}
