using JSharp.Binding;
using JSharp.Binding.Interfaces;
using JSharp.Syntax.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
{
    public class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate(IDictionary<string, object> variables)
        {
            Binder binder = new Binder(variables);
            IBoundExpression boundExpression = binder.BindExpression(Syntax.Root);

            IEnumerable<Diagnostic> diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics);

            if (diagnostics.Any())
            {
                return new EvaluationResult(null, diagnostics);
            }

            Evaluator evaluator = new Evaluator(boundExpression, variables);
            object value = evaluator.Evaluate();

            return new EvaluationResult(value, diagnostics);
        }
    }
}
