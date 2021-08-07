﻿using JSharp.Binding.Enums;
using JSharp.Binding.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Binding.Objects
{
    public class BoundVariableExperssion : IBoundExpression
    {
        public BoundVariableExperssion(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public Type Type { get; }

        public BoundNodeType BoundNode => BoundNodeType.VariableExpression;
    }
}
