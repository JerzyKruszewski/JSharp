using JSharp.Binding.Enums;
using JSharp.Binding.Interfaces;
using JSharp.Binding.Objects;
using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
{
    public class Evaluator
    {
        private readonly IBoundExpression _root;

        public Evaluator(IBoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(IBoundExpression expression)
        {
            if (expression is BoundLiteralExpression number)
            {
                return number.Value;
            }

            if (expression is BoundUnaryExpression unary)
            {
                return unary.OperatorType switch
                {
                    BoundUnaryOperatorType.Identity => EvaluateExpression(unary.Operand),
                    BoundUnaryOperatorType.Negation => -Convert.ToDouble(EvaluateExpression(unary.Operand)),
                    BoundUnaryOperatorType.LogicalNegation => !((bool)EvaluateExpression(unary.Operand)),
                    _ => throw new Exception($"Unexpected unary operator type <{unary.OperatorType}>")
                };
            }

            if (expression is BoundBinaryExpression binary)
            {
                object left = EvaluateExpression(binary.Left);
                object right = EvaluateExpression(binary.Right);

                return PerformOperation(left, binary.OperatorType, right);
            }

            //if (expression is ParenthesizedExpressionSyntax parenthesized)
            //{
            //    return EvaluateExpression(parenthesized.Expression);
            //}

            throw new Exception($"Unexpected expression {expression.BoundNode}");
        }

        private object PerformOperation(object left, BoundBinaryOperatorType operatorToken, object right)
        {
            return operatorToken switch
            {
                BoundBinaryOperatorType.Addition => Convert.ToDouble(left) + Convert.ToDouble(right),
                BoundBinaryOperatorType.Subtraction => Convert.ToDouble(left) - Convert.ToDouble(right),
                BoundBinaryOperatorType.Multiplication => Convert.ToDouble(left) * Convert.ToDouble(right),
                BoundBinaryOperatorType.Division => Convert.ToDouble(left) / Convert.ToDouble(right),
                BoundBinaryOperatorType.LogicalAnd => (bool)left && (bool)right,
                BoundBinaryOperatorType.LogicalOr => (bool)left || (bool)right,
                _ => throw new Exception($"Unexpected binary operator: {operatorToken}")
            };
        }
    }
}
