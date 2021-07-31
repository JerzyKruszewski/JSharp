using JSharp;
using JSharp.Enums;
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

            Lexer lexer = new Lexer(line);

            while (true)
            {
                SyntaxToken token = lexer.NextToken();

                Console.Write($"{token.Position}. {token.TokenType}: '{token.Text}'");

                if (token.Value is not null)
                {
                    Console.Write($" {token.Value}");
                }

                Console.WriteLine();

                if (token.TokenType == TokenType.EndOfFileToken)
                {
                    break;
                }
            }
        }
    }
}
