using JSharp.Syntax.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax
{
    public static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this TokenType token)
        {
            return token switch
            {
                TokenType.StarToken or TokenType.SlashToken => 5,
                TokenType.PlusToken or TokenType.MinusToken => 4,
                TokenType.EqualsEqualsToken or TokenType.BangEqualsToken => 3,
                TokenType.AmpersandAmpersandToken => 2,
                TokenType.PipePipeToken => 1,
                _ => 0
            };
        }

        public static int GetUnaryOperatorPrecedence(this TokenType token)
        {
            return token switch
            {
                TokenType.PlusToken or 
                TokenType.MinusToken or
                TokenType.BangToken => 6,
                _ => 0
            };
        }

        public static TokenType GetKeyword(string text)
        {
            return text switch
            {
                "true" => TokenType.TrueKeyword,
                "false" => TokenType.FalseKeyword,
                _ => throw new ArgumentException($"Unknown keyword: {text}")
            };
        }
    }
}
