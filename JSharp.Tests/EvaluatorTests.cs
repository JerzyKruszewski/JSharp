using JSharp.Syntax.Objects;
using JSharp.Syntax;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSharp.Binding;
using JSharp.Binding.Interfaces;

namespace JSharp.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [Test]
        [TestCase(6, "2+2*2")]
        [TestCase(8, "(2+2)*2", Ignore = "Broken support for ()")]
        [TestCase(18, "(1+1*1+1)*(1+1*1+1)+(1+1*1+1)*(1+1*1+1)", Ignore = "Broken support for ()")]
        [TestCase(2, "3-1")]
        [TestCase(2.5, "5/2")]
        [TestCase(-1, "-1")]
        [TestCase(1, "-(-1)", Ignore = "Broken support for ()")]
        [TestCase(1, "--1")]
        [TestCase(-6, "-(2*3)", Ignore = "Broken support for ()")]
        [TestCase(-6, "-2*3")]
        [TestCase(1, "+--+1")]
        public void Evaluate_WhenCalledWithMathematicalExpression_ReturnResult(double expected, string text, double epsilon = 0.00001)
        {
            Parser parser = new Parser(text);
            SyntaxTree tree = parser.Parse();
            Binder binder = new Binder();
            IBoundExpression boundExpression = binder.BindExpression(tree.Root);

            double actual = Convert.ToDouble(new Evaluator(boundExpression).Evaluate());

            Assert.AreEqual(expected, actual, epsilon);
        }

        [Test]
        [TestCase(true, "true && true")]
        [TestCase(false, "true && false")]
        [TestCase(false, "false && true")]
        [TestCase(false, "false && false")]
        [TestCase(true, "true || true")]
        [TestCase(true, "true || false")]
        [TestCase(true, "false || true")]
        [TestCase(false, "false || false")]
        public void Evaluate_WhenCalledWithLogicalExpression_ReturnResult(bool expected, string text)
        {
            Parser parser = new Parser(text);
            SyntaxTree tree = parser.Parse();
            Binder binder = new Binder();
            IBoundExpression boundExpression = binder.BindExpression(tree.Root);

            bool actual = (bool)(new Evaluator(boundExpression).Evaluate());

            Assert.AreEqual(expected, actual);
        }
    }
}
