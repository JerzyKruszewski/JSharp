using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class ParenthesizedExpressionSyntax : IExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesesToken,
                                             IExpressionSyntax expression,
                                             SyntaxToken closeParenthesesToken)
        {
            OpenParenthesesToken = openParenthesesToken;
            Expression = expression;
            CloseParenthesesToken = closeParenthesesToken;
        }

        public TokenType TokenType => TokenType.ParenthesizedExpression;

        public SyntaxToken OpenParenthesesToken { get; }
        public IExpressionSyntax Expression { get; }
        public SyntaxToken CloseParenthesesToken { get; }

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return OpenParenthesesToken;
            yield return Expression;
            yield return CloseParenthesesToken;
        }
    }
}
