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
                TokenType.PlusToken or TokenType.MinusToken => 1,
                TokenType.StarToken or TokenType.SlashToken => 2,
                _ => 0
            };
        }

        public static int GetUnaryOperatorPrecedence(this TokenType token)
        {
            return token switch
            {
                TokenType.PlusToken or TokenType.MinusToken => 3,
                _ => 0
            };
        }
    }
}
