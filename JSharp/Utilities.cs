using JSharp.Interfaces;
using JSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
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

        public static void LogErrors(IEnumerable<string> errors)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            foreach (string error in errors)
            {
                Console.WriteLine(error);
            }

            Console.ResetColor();
        }
    }
}
