using JSharp.Enums;
using JSharp.Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Tests
{
    [TestFixture]
    public class LexerTests
    {
        [Test]
        [TestCase(TokenType.EndOfFileToken, "1 + 2 + 3")]
        [TestCase(TokenType.NumberToken, "1 + 2 + 3")]
        [TestCase(TokenType.PlusToken, "1 + 2 + 3")]
        [TestCase(TokenType.WhiteSpaceToken, "1 + 2 + 3")]
        public void LexterNextToken_WhenCalledOnEntireString_ContainsCertainToken(TokenType token, string text)
        {
            Lexer lexer = new Lexer(text);
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            while (true)
            {
                SyntaxToken consumedToken = lexer.NextToken();
                tokens.Add(consumedToken);

                if (consumedToken.TokenType == TokenType.EndOfFileToken)
                {
                    break;
                }
            }

            Assert.IsTrue(tokens.Any(t => t.TokenType == token));
        }

        [Test]
        [TestCase(TokenType.StarToken, "1 + 2 + 3")]
        [TestCase(TokenType.BadToken, "1 + 2 + 3")]
        [TestCase(TokenType.CloseParenthesesToken, "1 + 2 + 3")]
        [TestCase(TokenType.MinusToken, "1 + 2 + 3")]
        [TestCase(TokenType.SlashToken, "1 + 2 + 3")]
        [TestCase(TokenType.OpenParenthesesToken, "1 + 2 + 3")]
        public void LexterNextToken_WhenCalledOnEntireString_DoesNotContainCertainToken(TokenType token, string text)
        {
            Lexer lexer = new Lexer(text);
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            while (true)
            {
                SyntaxToken consumedToken = lexer.NextToken();
                tokens.Add(consumedToken);

                if (consumedToken.TokenType == TokenType.EndOfFileToken)
                {
                    break;
                }
            }

            Assert.IsFalse(tokens.Any(t => t.TokenType == token));
        }
    }
}
