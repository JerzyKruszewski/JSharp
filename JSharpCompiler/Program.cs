using JSharp;
using JSharp.Binding;
using JSharp.Binding.Interfaces;
using JSharp.Syntax;
using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JSharpCompiler
{
    internal class Program
    {
        private static void Main()
        {
            Dictionary<string, object> variables = new Dictionary<string, object>();

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                Parser parser = new Parser(line);
                SyntaxTree tree = parser.Parse();
                Compilation compilation = new Compilation(tree);
                EvaluationResult result = compilation.Evaluate(variables);

                Utilities.PrintTree(tree.Root);

                if (result.Diagnostics.Any())
                {
                    Utilities.LogErrors(line, result.Diagnostics);
                    return;
                }

                Console.WriteLine(result.Value);

                Console.WriteLine();
            }
        }
    }
}
