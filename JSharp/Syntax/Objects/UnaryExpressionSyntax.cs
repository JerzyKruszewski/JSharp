using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class UnaryExpressionSyntax : IExpressionSyntax
    {
        public UnaryExpressionSyntax(SyntaxToken operatorToken, IExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public TokenType TokenType => TokenType.UnaryExpression;

        public SyntaxToken OperatorToken { get; }
        public IExpressionSyntax Operand { get; }

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}
