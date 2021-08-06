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
        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, IExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IEnumerable<Diagnostic> Diagnostics { get; }
        public IExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
    }
}
