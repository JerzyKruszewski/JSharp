using JSharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Objects
{
    public class SyntaxToken
    {
        public SyntaxToken(TokenType tokenType, int position, string text, object value)
        {
            TokenType = tokenType;
            Position = position;
            Text = text;
            Value = value;
        }

        public TokenType TokenType { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }
}
