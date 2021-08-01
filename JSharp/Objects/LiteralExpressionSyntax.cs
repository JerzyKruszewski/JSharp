using JSharp.Enums;
using JSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Objects
{
    public class LiteralExpressionSyntax : IExpressionSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public TokenType TokenType => TokenType.LiteralExpression;

        public SyntaxToken NumberToken { get; }

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }
    }
}
