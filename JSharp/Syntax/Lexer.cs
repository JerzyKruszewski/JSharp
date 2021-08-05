using JSharp.Syntax.Enums;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// C# but with mandatory Object Calisthenics (well... mostly...)
namespace JSharp.Syntax
{
    public class Lexer
    {
        private readonly string _text;
        private int _position;
#if DEBUG
        private readonly IList<string> _errors = new List<string>();
#endif

        public Lexer(string text)
        {
            _text = text;
        }

#if DEBUG
        public IEnumerable<string> Errors => _errors;
#endif

        private char Current => Peek(0);

        private char LookAhead => Peek(1);

        private char Peek(int offset)
        {
            int index = _position + offset;
            return (index >= _text.Length) ? '\0' : _text[index];
        }

        public IList<SyntaxToken> GetAllTokens()
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            SyntaxToken consumedToken;

            do
            {
                consumedToken = NextToken();

#if DEBUG
                if (consumedToken.TokenType == TokenType.BadToken)
                {
                    _errors.Add($"LEXER ERROR: {consumedToken.TokenType} '{consumedToken.Text}' at {consumedToken.Position}");
                }
#endif

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

#if DEBUG
                if (consumedToken.TokenType == TokenType.BadToken)
                {
                    _errors.Add($"LEXER ERROR: {consumedToken.TokenType} '{consumedToken.Text}' at {consumedToken.Position}");
                }
#endif

                if (!excludedTokens.Contains(consumedToken.TokenType))
                {
                    tokens.Add(consumedToken);
                }
            } while (consumedToken.TokenType != TokenType.EndOfFileToken);

            return tokens;
        }

        private SyntaxToken NextToken()
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

            if (char.IsLetter(Current))
            {
                return HandleLetter();
            }

            if (Current == '&' && LookAhead == '&')
            {
                return new SyntaxToken(TokenType.AmpersandAmpersandToken, _position += 2, "&&", null);
            }

            if (Current == '|' && LookAhead == '|')
            {
                return new SyntaxToken(TokenType.PipePipeToken, _position += 2, "||", null);
            }

            if (Current == '=' && LookAhead == '=')
            {
                return new SyntaxToken(TokenType.EqualsEqualsToken, _position += 2, "==", null);
            }

            if (Current == '!' && LookAhead == '=')
            {
                return new SyntaxToken(TokenType.BangEqualsToken, _position += 2, "!=", null);
            }

            return Current switch
            {
                '+' => new SyntaxToken(TokenType.PlusToken, _position++, "+", null),
                '-' => new SyntaxToken(TokenType.MinusToken, _position++, "-", null),
                '*' => new SyntaxToken(TokenType.StarToken, _position++, "*", null),
                '/' => new SyntaxToken(TokenType.SlashToken, _position++, "/", null),
                '(' => new SyntaxToken(TokenType.OpenParenthesesToken, _position++, "(", null),
                ')' => new SyntaxToken(TokenType.CloseParenthesesToken, _position++, ")", null),
                '!' => new SyntaxToken(TokenType.BangToken, _position++, "!", null),
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

            if (!int.TryParse(text, out int value))
            {
                _errors.Add($"LEXER ERROR: Text {text} is not parseable to Int32");
            }

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

        private SyntaxToken HandleLetter()
        {
            int startPos = _position;

            while (char.IsLetter(Current))
            {
                Next();
            }

            int length = _position - startPos;

            string text = _text.Substring(startPos, length);

            return new SyntaxToken(SyntaxFacts.GetKeyword(text), startPos, text, null);
        }

        private void Next()
        {
            _position++;
        }
    }
}
