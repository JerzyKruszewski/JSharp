using JSharp.Enums;
using JSharp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Interfaces
{
    public interface ISyntaxNode
    {
        public TokenType TokenType { get; }

        public IEnumerable<ISyntaxNode> GetChildren();
    }
}
