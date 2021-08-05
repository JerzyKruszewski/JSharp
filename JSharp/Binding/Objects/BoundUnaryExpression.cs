using JSharp.Binding.Enums;
using JSharp.Binding.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundUnaryExpression : IBoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator boundOperator, IBoundExpression operand)
        {
            BoundOperator = boundOperator;
            Operand = operand;
        }

        public BoundNodeType BoundNode => BoundNodeType.UnaryExpression;
        public Type Type => BoundOperator.ReturnType;
        public BoundUnaryOperator BoundOperator { get; }
        public IBoundExpression Operand { get; }
    }
}
