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
        [TestCase(TokenType.EndOfFileToken, "")]
        [TestCase(TokenType.EndOfFileToken, "1 + 2 + 3")]
        [TestCase(TokenType.NumberToken, "1 + 2 + 3")]
        [TestCase(TokenType.PlusToken, "1 + 2 + 3")]
        [TestCase(TokenType.WhiteSpaceToken, "1 + 2 + 3")]
        public void GetAllTokens_WhenCalledOnEntireString_ContainsCertainToken(TokenType token, string text)
        {
            Lexer lexer = new Lexer(text);

            Assert.IsTrue(lexer.GetAllTokens()
                               .Any(t => t.TokenType == token));
        }

        [Test]
        [TestCase(TokenType.StarToken, "1 + 2 + 3")]
        [TestCase(TokenType.BadToken, "1 + 2 + 3")]
        [TestCase(TokenType.CloseParenthesesToken, "1 + 2 + 3")]
        [TestCase(TokenType.MinusToken, "1 + 2 + 3")]
        [TestCase(TokenType.SlashToken, "1 + 2 + 3")]
        [TestCase(TokenType.OpenParenthesesToken, "1 + 2 + 3")]
        public void GetAllTokens_WhenCalledOnEntireString_DoesNotContainCertainToken(TokenType token, string text)
        {
            Lexer lexer = new Lexer(text);

            Assert.IsFalse(lexer.GetAllTokens()
                                .Any(t => t.TokenType == token));
        }

        [Test]
        [TestCase("1 + 2 + 3", TokenType.EndOfFileToken)]
        [TestCase("1 + 2 + 3", TokenType.NumberToken)]
        [TestCase("1 + 2 + 3", TokenType.NumberToken, TokenType.PlusToken)]
        public void GetFilteredTokens_WhenCalledOnEntireStringAndExcludedTokens_DoesNotContainAnyExcludedTokens(string text, params TokenType[] excludedTokens)
        {
            Lexer lexer = new Lexer(text);

            Assert.IsFalse(lexer.GetFilteredTokens(excludedTokens)
                                .Any(t => excludedTokens.Contains(t.TokenType)));
        }
    }
}
