﻿using JSharp.Enums;
using JSharp.Interfaces;
using JSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
{
    public class Evaluator
    {
        private readonly IExpressionSyntax _root;

        public Evaluator(IExpressionSyntax root)
        {
            _root = root;
        }

        public double Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private double EvaluateExpression(IExpressionSyntax expression)
        {
            if (expression is LiteralExpressionSyntax number)
            {
                return Convert.ToDouble(number.NumberToken.Value);
            }

            if (expression is UnaryExpressionSyntax unary)
            {
                return (unary.OperatorToken.TokenType == TokenType.MinusToken) ?
                       -EvaluateExpression(unary.Operand) :
                       EvaluateExpression(unary.Operand);
            }

            if (expression is BinaryExpressionSyntax binary)
            {
                double left = EvaluateExpression(binary.Left);
                double right = EvaluateExpression(binary.Right);

                return PerformOperation(left, binary.OperatorToken.TokenType, right);
            }

            if (expression is ParenthesizedExpressionSyntax parenthesized)
            {
                return EvaluateExpression(parenthesized.Expression);
            }

            throw new Exception($"Unexpected expression {expression.TokenType}");
        }

        private double PerformOperation(double left, TokenType operatorToken, double right)
        {
            if (operatorToken == TokenType.PlusToken)
            {
                return left + right;
            }

            if (operatorToken == TokenType.MinusToken)
            {
                return left - right;
            }

            if (operatorToken == TokenType.StarToken)
            {
                return left * right;
            }

            if (operatorToken == TokenType.SlashToken)
            {
                return left / right;
            }

            throw new Exception($"Unexpected binary operator: {operatorToken}");
        }
    }
}
