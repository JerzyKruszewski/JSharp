using JSharp.Syntax.Objects;
using JSharp.Syntax;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSharp.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [Test]
        [TestCase(6, "2+2*2")]
        [TestCase(8, "(2+2)*2")]
        [TestCase(18, "(1+1*1+1)*(1+1*1+1)+(1+1*1+1)*(1+1*1+1)")]
        [TestCase(2, "3-1")]
        [TestCase(2.5, "5/2")]
        [TestCase(-1, "-1")]
        [TestCase(1, "-(-1)")]
        [TestCase(1, "--1")]
        [TestCase(-6, "-(2*3)")]
        [TestCase(-6, "-2*3")]
        [TestCase(1, "+--+1")]
        public void Evaluate_WhenCalledWithMathematicalExpression_ReturnResult(double expected, string text, double epsilon = 0.00001)
        {
            Parser parser = new Parser(text);
            SyntaxTree tree = parser.Parse();

            Assert.AreEqual(expected, new Evaluator(tree.Root).Evaluate(), epsilon);
        }
    }
}
