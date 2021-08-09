using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
{
    public class Variable
    {
        public Variable(object value, Type type)
        {
            Value = value;
            Type = type;
        }

        public object Value { get; }
        public Type Type { get; }
    }
}
