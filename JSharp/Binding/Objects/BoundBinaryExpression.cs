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
        public BoundBinaryExpression(IBoundExpression left, BoundBinaryOperator boundOperator, IBoundExpression right)
        {
            Left = left;
            BoundOperator = boundOperator;
            Right = right;
        }

        public BoundNodeType BoundNode => BoundNodeType.BinaryExpression;
        public Type Type => BoundOperator.ReturnType;

        public IBoundExpression Left { get; }
        public BoundBinaryOperator BoundOperator { get; }
        public IBoundExpression Right { get; }
    }
}
