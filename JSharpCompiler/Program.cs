using JSharp;
using JSharp.Enums;
using JSharp.Interfaces;
using JSharp.Objects;
using System;

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
            Utilities.PrintTree(parser.ParseExpression());
        }
    }
}
