using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Interfaces
{
    public interface IBoundExpression : IBoundNode
    {
        public Type Type { get; }
    }
}
