﻿using JSharp;
using JSharp.Syntax;
using JSharp.Syntax.Enums;
using JSharp.Syntax.Interfaces;
using JSharp.Syntax.Objects;
using System;
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
            Utilities.PrintTree(tree.Root);
            
            if (tree.Errors.Any())
            {
                Utilities.LogErrors(tree.Errors);
                return;
            }

            Evaluator evaluator = new Evaluator(tree.Root);

            Console.WriteLine(evaluator.Evaluate()
                                       .ToString(CultureInfo.InvariantCulture));
        }
    }
}
