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
        private readonly IDictionary<string, Variable> _variables;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Binder(IDictionary<string, Variable> variables)
        {
            _variables = variables;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        public IBoundExpression BindExpression(IExpressionSyntax syntax)
        {
            return syntax.TokenType switch
            {
                TokenType.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
                TokenType.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
                TokenType.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
                TokenType.ParenthesizedExpression => BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression),
                TokenType.DefinitionExpression => BindDefinitionExpression((DefinitionExpressionSyntax)syntax),
                TokenType.NameExpression => BindNameExpression((NameExpressionSyntax)syntax),
                TokenType.AssignmentExpression => BindAssignmentExpression((AssignmentExpressionSyntax)syntax),
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
                _diagnostics.ReportUndefinedOperator(syntax.OperatorToken.Span,
                                                     syntax.OperatorToken.Text,
                                                     boundOperand.Type,
                                                     source: "BINDER");
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
                _diagnostics.ReportUndefinedOperator(syntax.OperatorToken.Span,
                                                     syntax.OperatorToken.Text,
                                                     boundLeft.Type,
                                                     boundRight.Type,
                                                     source: "BINDER");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }

        private IBoundExpression BindDefinitionExpression(DefinitionExpressionSyntax syntax)
        {
            string name = syntax.IdentifierToken.Text;

            if (_variables.ContainsKey(name))
            {
                _diagnostics.ReportVariableAlreadyDefined(syntax.IdentifierToken.Span, name, source: "BINDER");
            }

            Type type = syntax.VariableType;

            return new BoundVariableDefinitionExpression(name, type);
        }

        private IBoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            string name = syntax.IdentifierToken.Text;

            if (!_variables.TryGetValue(name, out Variable value))
            {
                _diagnostics.ReportUndefinedVariableName(syntax.IdentifierToken.Span, name, source: "BINDER");
            }

            Type type = value.Type ?? typeof(object);

            return new BoundVariableExperssion(name, type);
        }

        private IBoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            string name = syntax.IdentifierToken.Text;
            IBoundExpression boundExpression = BindExpression(syntax.Expression);
            return new BoundAssingmentExpression(name, boundExpression);
        }
    }
}
