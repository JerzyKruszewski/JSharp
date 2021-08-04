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

namespace JSharp.Binding
{
    public class Binder
    {
        private readonly IList<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;

        public IBoundExpression BindExpression(IExpressionSyntax syntax)
        {
            return syntax.TokenType switch
            {
                TokenType.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
                TokenType.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
                TokenType.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
                _ => throw new ArgumentException($"Unexpected token type {syntax.TokenType} of object {nameof(syntax)}")
            };
        }

        private IBoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            object value = syntax.Value ?? 0.0;

            return new BoundLiteralExpression(value);
        }

        private IBoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            IBoundExpression boundOperand = BindExpression(syntax.Operand);
            BoundUnaryOperatorType? operatorType = BindUnaryOperator(syntax.OperatorToken.TokenType, boundOperand.Type);

            if (operatorType is null)
            {
                _errors.Add($"BINDER ERROR: Unary oparator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }

            return new BoundUnaryExpression(operatorType.Value, boundOperand);
        }

        private IBoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            IBoundExpression boundLeft = BindExpression(syntax.Left);
            IBoundExpression boundRight = BindExpression(syntax.Right);
            BoundBinaryOperatorType? operatorType = BindBinaryOperator(syntax.OperatorToken.TokenType, boundLeft.Type, boundRight.Type);

            if (operatorType is null)
            {
                _errors.Add($"BINDER ERROR: Unary oparator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} or {boundRight.Type}");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, operatorType.Value, boundRight);
        }
        
        private BoundUnaryOperatorType? BindUnaryOperator(TokenType tokenType, Type type)
        {
            if (type == typeof(bool))
            {
                return tokenType switch
                {
                    TokenType.BangToken => BoundUnaryOperatorType.LogicalNegation,
                    _ => throw new ArgumentException($"Unexpected boolean token type {tokenType} for type {type}")
                };
            }

            if (type != typeof(double) && type != typeof(int))
            {
                _errors.Add($"BINDER ERROR: Unknown {nameof(type)} ({type})");
                return null;
            }

            return tokenType switch
            {
                TokenType.PlusToken => BoundUnaryOperatorType.Identity,
                TokenType.MinusToken => BoundUnaryOperatorType.Negation,
                _ => throw new ArgumentException($"Unexpected token type {tokenType} which is not parsable {nameof(BoundUnaryOperatorType)}")
            };
        }

        private BoundBinaryOperatorType? BindBinaryOperator(TokenType tokenType, Type leftType, Type rightType)
        {
            if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                return tokenType switch
                {
                    TokenType.AmpersandAmpersandToken => BoundBinaryOperatorType.LogicalAnd,
                    TokenType.PipePipeToken => BoundBinaryOperatorType.LogicalOr,
                    _ => throw new ArgumentException($"Unexpected boolean token type {tokenType} for type {leftType}")
                };
            }

            if ((leftType != typeof(double) && leftType != typeof(int)) ||
                (rightType != typeof(double) && rightType != typeof(int)))
            {
                _errors.Add($"BINDER ERROR: Unknown {nameof(leftType)} ({leftType}) or {nameof(rightType)} ({rightType})");
                return null;
            }

            return tokenType switch
            {
                TokenType.PlusToken => BoundBinaryOperatorType.Addition,
                TokenType.MinusToken => BoundBinaryOperatorType.Subtraction,
                TokenType.StarToken => BoundBinaryOperatorType.Multiplication,
                TokenType.SlashToken => BoundBinaryOperatorType.Division,
                _ => throw new ArgumentException($"Unexpected token type {tokenType} which is not parsable {nameof(BoundBinaryOperatorType)}")
            };
        }
    }
}
