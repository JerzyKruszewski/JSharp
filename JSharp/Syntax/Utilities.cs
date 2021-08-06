using JSharp.Syntax.Interfaces;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax
{
    public class Utilities
    {
        public static void PrintTree(ISyntaxNode node, string indent = "", bool isLast = true)
        {
            string marker = (isLast) ? "└──" : "├──";

            Console.Write($"{indent}{marker}{node.TokenType}");

            if (node is SyntaxToken token && token.Value is not null)
            {
                Console.Write($" {token.Value}");
            }

            Console.WriteLine();

            indent += (isLast) ? "   " : "│  ";

            ISyntaxNode lastChild = node.GetChildren().LastOrDefault();

            foreach (ISyntaxNode child in node.GetChildren())
            {
                PrintTree(child, indent, child == lastChild);
            }
        }

        public static void LogErrors(string line, IEnumerable<Diagnostic> diagnostics)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            foreach (Diagnostic diagnostic in diagnostics)
            {
                Console.WriteLine(diagnostic.ToString());
                Console.ResetColor();

                PrintExactError(line, diagnostic);
            }

            Console.ResetColor();
        }

        private static void PrintExactError(string line, Diagnostic diagnostic)
        {
            string prefix;
            string error;
            string suffix;

            try
            {
                prefix = line.Substring(0, diagnostic.Span.Start);
                error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                suffix = line[diagnostic.Span.End..];
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            Console.Write(prefix);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(error);
            Console.ResetColor();
            Console.WriteLine(suffix);
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
    }
}
