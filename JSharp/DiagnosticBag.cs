using JSharp.Syntax.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp
{
    public class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        public void ReportInvalidType(TextSpan span, string text, Type type, string source)
        {
            string message = $"{source} ERROR: The value {text} is not valid {type}";
            Report(span, message);
        }

        public void ReportBadToken(int position, string text, string source)
        {
            string message = $"{source} ERROR: Bad character input '{text}'";
            Report(new TextSpan(position, 1), message);
        }

        public void ReportUnexpectedToken(TextSpan span, TokenType token, TokenType expectedToken, string source)
        {
            string message = $"{source} ERROR: Unexpected token {token} at {span.Start} (Expected: {expectedToken})";
            Report(span, message);
        }

        public void ReportUndefinedOperator(TextSpan span, string operatorTokenText, Type type, string source)
        {
            string message = $"{source} ERROR: Oparator '{operatorTokenText}' is not defined for type {type}";
            Report(span, message);
        }

        public void ReportUndefinedOperator(TextSpan span, string operatorTokenText, Type firstType, Type secondType, string source)
        {
            string message = $"{source} ERROR: Oparator '{operatorTokenText}' is not defined for types {firstType} or {secondType}";
            Report(span, message);
        }


        public  void ReportUndefinedVariableName(TextSpan span, string name, string source)
        {
            string message = $"{source} ERROR: Undefined variable with name {name}";
            Report(span, message);
        }

        public void ReportVariableAlreadyDefined(TextSpan span, string name, string source)
        {
            string message = $"{source} ERROR: Variable {name} is already defined";
            Report(span, message);
        }

        private void Report(TextSpan span, string message)
        {
            _diagnostics.Add(new Diagnostic(span, message));
        }
    }
}
