using JSharp.Syntax.Enums;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Syntax.Interfaces
{
    public interface ISyntaxNode
    {
        public TokenType TokenType { get; }

        public IEnumerable<ISyntaxNode> GetChildren();
    }
}
