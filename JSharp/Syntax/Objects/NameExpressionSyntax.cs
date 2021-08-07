using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class NameExpressionSyntax : IExpressionSyntax
    {
        public NameExpressionSyntax(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public SyntaxToken IdentifierToken { get; }

        public TokenType TokenType => TokenType.NameExpression;

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}
