using JSharp.Binding.Enums;
using JSharp.Binding.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundLiteralExpression : IBoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;
        }

        public BoundNodeType BoundNode => BoundNodeType.LiteralExpression;
        public Type Type => Value.GetType();
        public object Value { get; }
    }
}
