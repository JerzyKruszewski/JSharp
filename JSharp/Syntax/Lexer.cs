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
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Lexer(string text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

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

                if (consumedToken.TokenType == TokenType.BadToken)
                {
                    _diagnostics.ReportBadToken(consumedToken.Position, consumedToken.Text, source: "LEXER");
                }

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

                if (consumedToken.TokenType == TokenType.BadToken)
                {
                    _diagnostics.ReportBadToken(consumedToken.Position, consumedToken.Text, source: "LEXER");
                }

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

            int startPos = _position;

            if (Current == '&' && LookAhead == '&')
            {
                _position += 2;
                return new SyntaxToken(TokenType.AmpersandAmpersandToken, startPos, "&&", null);
            }

            if (Current == '|' && LookAhead == '|')
            {
                _position += 2;
                return new SyntaxToken(TokenType.PipePipeToken, startPos, "||", null);
            }

            if (Current == '=' && LookAhead == '=')
            {
                _position += 2;
                return new SyntaxToken(TokenType.EqualsEqualsToken, startPos, "==", null);
            }

            if (Current == '!' && LookAhead == '=')
            {
                _position += 2;
                return new SyntaxToken(TokenType.BangEqualsToken, startPos, "!=", null);
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
                _diagnostics.ReportInvalidType(new TextSpan(startPos, length), text, typeof(int), source: "LEXER");
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
