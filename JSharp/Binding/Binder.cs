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
            BoundUnaryOperator boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.TokenType, boundOperand.Type);

            if (boundOperator is null)
            {
                _errors.Add($"BINDER ERROR: Unary oparator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private IBoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            IBoundExpression boundLeft = BindExpression(syntax.Left);
            IBoundExpression boundRight = BindExpression(syntax.Right);
            BoundBinaryOperator boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.TokenType, boundLeft.Type, boundRight.Type);

            if (boundOperator is null)
            {
                _errors.Add($"BINDER ERROR: Unary oparator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} or {boundRight.Type}");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}
