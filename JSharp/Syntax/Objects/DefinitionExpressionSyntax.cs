using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class DefinitionExpressionSyntax : IExpressionSyntax
    {
        public DefinitionExpressionSyntax(SyntaxToken variableToken, SyntaxToken identifierToken, Type variableType)
        {
            VariableToken = variableToken;
            IdentifierToken = identifierToken;
            VariableType = variableType;
        }

        public SyntaxToken VariableToken { get; }
        public SyntaxToken IdentifierToken { get; }
        public Type VariableType { get; }

        public TokenType TokenType => TokenType.DefinitionExpression;

        public IEnumerable<ISyntaxNode> GetChildren()
        {
            yield return VariableToken;
            yield return IdentifierToken;
        }
    }
}
