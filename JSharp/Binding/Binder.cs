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
            double value = syntax.NumberToken.Value as double? ?? 0.0;
            return new BoundLiteralExpression(value);
        }

        private IBoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            BoundUnaryOperatorType operatorType = BindUnaryOperator(syntax.OperatorToken.TokenType);
            IBoundExpression boundOperand = BindExpression(syntax.Operand);

            return new BoundUnaryExpression(operatorType, boundOperand);
        }

        private IBoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            IBoundExpression boundLeft = BindExpression(syntax.Left);
            BoundBinaryOperatorType operatorType = BindBinaryOperator(syntax.OperatorToken.TokenType);
            IBoundExpression boundRight = BindExpression(syntax.Right);

            return new BoundBinaryExpression(boundLeft, operatorType, boundRight);
        }

        private BoundUnaryOperatorType BindUnaryOperator(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.PlusToken => BoundUnaryOperatorType.Identity,
                TokenType.MinusToken => BoundUnaryOperatorType.Negation,
                _ => throw new ArgumentException($"Unexpected token type {tokenType} which is not parsable {nameof(BoundUnaryOperatorType)}")
            };
        }

        private BoundBinaryOperatorType BindBinaryOperator(TokenType tokenType)
        {
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
