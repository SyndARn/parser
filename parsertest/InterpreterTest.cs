using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using parser;

namespace parsertest
{

    [TestClass]
    public class InterpreterTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SimpleInterpreterTest()
        {
            string data =
                @"Actor bla
                    Behavior(""bla"")
                    {
                        Send Console (""bla"")
                    }
                 ";
            var parser = DataFactory.ParserFactory(data);
            var node = parser.ProduceTree();
            Assert.IsTrue(node.GetType() == typeof(ActorNode));
            int ident = 0;
            TestContext.WriteLine(data);
            TestContext.WriteLine(node.PrintNode(ident));
            var interpreter = new Interpreter();
            StringBuilder sb = new StringBuilder();
            TestContext.WriteLine(interpreter.Interpret(node, sb));
        }
    }
}
