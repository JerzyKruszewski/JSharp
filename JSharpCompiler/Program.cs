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
            Console.Write("> ");
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            Parser parser = new Parser(line);
            SyntaxTree tree = parser.Parse();
            Binder binder = new Binder();
            IBoundExpression boundExpression = binder.BindExpression(tree.Root);
            Utilities.PrintTree(tree.Root);

            IEnumerable<string> errors = tree.Errors.Concat(binder.Errors);
            
            if (errors.Any())
            {
                Utilities.LogErrors(errors);
                return;
            }

            Evaluator evaluator = new Evaluator(boundExpression);

            Console.WriteLine(evaluator.Evaluate());
        }
    }
}
