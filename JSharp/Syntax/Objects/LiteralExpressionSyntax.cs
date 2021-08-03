using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class LiteralExpressionSyntax : IExpressionSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken token)
            : this(token, token.Value)
        {

        }

        public LiteralExpressionSyntax(SyntaxToken token, object value)
        {
            Token = token;
            Value = value;
        }

        public TokenType TokenType => TokenType.LiteralExpression;

        public SyntaxToken Token { get; }
        public object Value { get; }

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}
