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
        public BoundUnaryExpression(BoundUnaryOperatorType operatorType, IBoundExpression operand)
        {
            OperatorType = operatorType;
            Operand = operand;
        }

        public BoundNodeType BoundNode => BoundNodeType.UnaryExpression;
        public Type Type => Operand.Type;
        public BoundUnaryOperatorType OperatorType { get; }
        public IBoundExpression Operand { get; }
    }
}
