using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class BinaryExpressionSyntax : IExpressionSyntax
    {
        public BinaryExpressionSyntax(IExpressionSyntax left,
                                      SyntaxToken operatorToken,
                                      IExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public TokenType TokenType => TokenType.BinaryExpression;

        public IExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public IExpressionSyntax Right { get; }

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}