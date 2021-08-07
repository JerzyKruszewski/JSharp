using JSharp.Binding.Enums;
using JSharp.Binding.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundAssingmentExpression : IBoundExpression
    {
        public BoundAssingmentExpression(string name, IBoundExpression expression)
        {
            Name = name;
            Expression = expression;
        }

        public string Name { get; }
        public IBoundExpression Expression { get; }
        public Type Type => Expression.Type;

        public BoundNodeType BoundNode => BoundNodeType.AssignmentExpression;
    }
}
