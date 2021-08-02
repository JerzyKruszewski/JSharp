using JSharp.Syntax.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Objects
{
    public class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> errors, IExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Errors = errors;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IEnumerable<string> Errors { get; }
        public IExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    }
}
