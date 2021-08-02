using JSharp.Binding.Enums;
using JSharp.Binding.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundBinaryExpression : IBoundExpression
    {
        public BoundBinaryExpression(IBoundExpression left, BoundBinaryOperatorType operatorType, IBoundExpression right)
        {
            Left = left;
            OperatorType = operatorType;
            Right = right;
        }

        public BoundNodeType BoundNode => BoundNodeType.BinaryExpression;
        public Type Type => Left.Type;

        public IBoundExpression Left { get; }
        public BoundBinaryOperatorType OperatorType { get; }
        public IBoundExpression Right { get; }
    }
}
