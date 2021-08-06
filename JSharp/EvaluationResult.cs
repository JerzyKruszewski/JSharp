using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
{
    public class EvaluationResult
    {
        public EvaluationResult(object value, IEnumerable<Diagnostic> diagnostics)
        {
            Value = value;
            Diagnostics = diagnostics.ToList();
        }

        public object Value { get; }
        public IReadOnlyList<Diagnostic> Diagnostics { get; }
    }
}
