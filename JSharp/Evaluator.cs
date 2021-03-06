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
        private readonly IDictionary<string, Variable> _variables;

        public Evaluator(IBoundExpression root, IDictionary<string, Variable> variables)
        {
            _root = root;
            _variables = variables;
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

            if (expression is BoundVariableDefinitionExpression definition)
            {
                object defaultValue = (definition.Type == typeof(int)) ?
                                      (object)default(int) :
                                      (definition.Type == typeof(bool)) ?
                                      (object)default(bool) :
                                      default(object);

                _variables.Add(definition.Name, new Variable(defaultValue, definition.Type));
                return null;
            }

            if (expression is BoundVariableExperssion variable)
            {
                return _variables[variable.Name].Value;
            }

            if (expression is BoundAssingmentExpression assingment)
            {
                object value = EvaluateExpression(assingment.Expression);

                if (!_variables.ContainsKey(assingment.Name))
                {
                    return null;
                }

                if (_variables[assingment.Name].Type != value.GetType())
                {
                    return null;
                }

                _variables[assingment.Name] = new Variable(value, _variables[assingment.Name].Type); ;
                return value;
            }

            if (expression is BoundUnaryExpression unary)
            {
                return unary.BoundOperator.OperatorType switch
                {
                    BoundUnaryOperatorType.Identity => EvaluateExpression(unary.Operand),
                    BoundUnaryOperatorType.Negation => -Convert.ToDouble(EvaluateExpression(unary.Operand)),
                    BoundUnaryOperatorType.LogicalNegation => !((bool)EvaluateExpression(unary.Operand)),
                    _ => throw new Exception($"Unexpected unary operator type <{unary.BoundOperator}>")
                };
            }

            if (expression is BoundBinaryExpression binary)
            {
                object left = EvaluateExpression(binary.Left);
                object right = EvaluateExpression(binary.Right);

                return PerformOperation(left, binary.BoundOperator.OperatorType, right);
            }

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
                BoundBinaryOperatorType.LogicalEquals => left.Equals(right),
                BoundBinaryOperatorType.LogicalNotEquals => !left.Equals(right),
                _ => throw new Exception($"Unexpected binary operator: {operatorToken}")
            };
        }
    }
}
