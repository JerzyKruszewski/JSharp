using JSharp.Binding.Enums;
using JSharp.Syntax.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundBinaryOperator
    {
        public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorType operatorType, Type leftType, Type rightType)
            : this(tokenType, operatorType, leftType, rightType, leftType)
        {

        }

        public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorType operatorType, Type leftType, Type rightType, Type returnType)
        {
            TokenType = tokenType;
            OperatorType = operatorType;
            LeftType = leftType;
            RightType = rightType;
            ReturnType = returnType;
        }

        public TokenType TokenType { get; }
        public BoundBinaryOperatorType OperatorType { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ReturnType { get; }

        private static readonly BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(TokenType.AmpersandAmpersandToken, BoundBinaryOperatorType.LogicalAnd, typeof(bool), typeof(bool)),
            new BoundBinaryOperator(TokenType.PipePipeToken, BoundBinaryOperatorType.LogicalOr, typeof(bool), typeof(bool)),
            new BoundBinaryOperator(TokenType.PlusToken, BoundBinaryOperatorType.Addition, typeof(int), typeof(int)),
            new BoundBinaryOperator(TokenType.MinusToken, BoundBinaryOperatorType.Subtraction, typeof(int), typeof(int)),
            new BoundBinaryOperator(TokenType.StarToken, BoundBinaryOperatorType.Multiplication, typeof(int), typeof(int)),
            new BoundBinaryOperator(TokenType.SlashToken, BoundBinaryOperatorType.Division, typeof(int), typeof(int))
        };

        public static BoundBinaryOperator Bind(TokenType token, Type leftType, Type rightType)
        {
            return _operators.FirstOrDefault(o => o.TokenType == token && o.LeftType == leftType && o.RightType == rightType);
        }
    }
}
