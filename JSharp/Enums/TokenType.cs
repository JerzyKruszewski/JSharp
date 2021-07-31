using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Enums
{
    public enum TokenType
    {
        EndOfFileToken = 0,
        NumberToken = 1,
        WhiteSpaceToken = 2,
        PlusToken = 3,
        MinusToken = 4,
        StarToken = 5,
        SlashToken = 6,
        OpenParenthesesToken = 7,
        CloseParenthesesToken = 8,
        BadToken = 9,
        NumberExpression = 10,
        BinaryExpression = 11
    }
}
