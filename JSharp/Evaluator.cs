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
                return (unary.OperatorType == BoundUnaryOperatorType.Negation) ?
                       -Convert.ToDouble(EvaluateExpression(unary.Operand)) :
                       EvaluateExpression(unary.Operand);
            }

            if (expression is BoundBinaryExpression binary)
            {
                double left = Convert.ToDouble(EvaluateExpression(binary.Left));
                double right = Convert.ToDouble(EvaluateExpression(binary.Right));

                return PerformOperation(left, binary.OperatorType, right);
            }

            //if (expression is ParenthesizedExpressionSyntax parenthesized)
            //{
            //    return EvaluateExpression(parenthesized.Expression);
            //}

            throw new Exception($"Unexpected expression {expression.BoundNode}");
        }

        private double PerformOperation(double left, BoundBinaryOperatorType operatorToken, double right)
        {
            return operatorToken switch
            {
                BoundBinaryOperatorType.Addition => left + right,
                BoundBinaryOperatorType.Subtraction => left - right,
                BoundBinaryOperatorType.Multiplication => left * right,
                BoundBinaryOperatorType.Division => left / right,
                _ => throw new Exception($"Unexpected binary operator: {operatorToken}")
            };
        }
    }
}
