using JSharp.Enums;
using JSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// C# but with mandatory Object Calisthenics (well... mostly...)
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

        public IList<SyntaxToken> GetAllTokens()
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            SyntaxToken consumedToken;

            do
            {
                consumedToken = NextToken();
                tokens.Add(consumedToken);
            } while (consumedToken.TokenType != TokenType.EndOfFileToken);

            return tokens;
        }

        public IList<SyntaxToken> GetFilteredTokens(params TokenType[] excludedTokens)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            SyntaxToken consumedToken;

            do
            {
                consumedToken = NextToken();

                if (!excludedTokens.Contains(consumedToken.TokenType))
                {
                    tokens.Add(consumedToken);
                }
            } while (consumedToken.TokenType != TokenType.EndOfFileToken);

            return tokens;
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(TokenType.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                return HandleDigit();
            }

            if (char.IsWhiteSpace(Current))
            {
                return HandleWhiteSpace();
            }

            return Current switch
            {
                '+' => new SyntaxToken(TokenType.PlusToken, _position++, "+", null),
                '-' => new SyntaxToken(TokenType.MinusToken, _position++, "-", null),
                '*' => new SyntaxToken(TokenType.StarToken, _position++, "*", null),
                '/' => new SyntaxToken(TokenType.SlashToken, _position++, "/", null),
                '(' => new SyntaxToken(TokenType.OpenParenthesesToken, _position++, "(", null),
                ')' => new SyntaxToken(TokenType.CloseParenthesesToken, _position++, ")", null),
                _ => new SyntaxToken(TokenType.BadToken, _position++, _text.Substring(_position - 1, 1), null)
            };
        }

        private SyntaxToken HandleDigit()
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

        private SyntaxToken HandleWhiteSpace()
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

        private void Next()
        {
            _position++;
        }
    }
}
