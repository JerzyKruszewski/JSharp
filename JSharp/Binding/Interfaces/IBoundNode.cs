using JSharp.Binding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Interfaces
{
    public interface IBoundNode
    {
        public BoundNodeType BoundNode { get; }
    }
}
