using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Enums
{
    public enum TokenType
    {
        // Special
        EndOfFileToken,
        BadToken,

        //Tokens
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesesToken,
        CloseParenthesesToken,
        IdentifierToken,
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        EqualsToken,

        //Keywords
        TrueKeyword,
        FalseKeyword,
        IntVariableKeyword,
        BoolVariableKeyword,
        ObjectVariableKeyword,

        //Expressions
        LiteralExpression,
        NameExpression,
        AssignmentExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}
