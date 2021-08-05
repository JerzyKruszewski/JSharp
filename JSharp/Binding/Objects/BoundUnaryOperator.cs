using JSharp.Binding.Enums;
using JSharp.Syntax.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundUnaryOperator
    {
        public BoundUnaryOperator(TokenType tokenType, BoundUnaryOperatorType operatorType, Type operandType)
            : this(tokenType, operatorType, operandType, operandType)
        {

        }

        public BoundUnaryOperator(TokenType tokenType, BoundUnaryOperatorType operatorType, Type operandType, Type returnType)
        {
            TokenType = tokenType;
            OperatorType = operatorType;
            OperandType = operandType;
            ReturnType = returnType;
        }

        public TokenType TokenType { get; }
        public BoundUnaryOperatorType OperatorType { get; }
        public Type OperandType { get; }
        public Type ReturnType { get; }

        private static readonly BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(TokenType.BangToken, BoundUnaryOperatorType.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(TokenType.PlusToken, BoundUnaryOperatorType.Identity, typeof(int)),
            new BoundUnaryOperator(TokenType.MinusToken, BoundUnaryOperatorType.Negation, typeof(int))
        };

        public static BoundUnaryOperator Bind(TokenType token, Type type)
        {
            return _operators.FirstOrDefault(o => o.TokenType == token && o.OperandType == type);
        }
    }
}
