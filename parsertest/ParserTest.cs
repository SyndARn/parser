using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using parser;

namespace parsertest
{
    [TestClass]
    public class ParserTest
    {
        public TestContext TestContext { get; set; }

        //[TestMethod]
        //public void BasicParserTest()
        //{
        //    string data = "JeSuisUnIdentifier";
        //    var parser = DataFactory.ParserFactory(data);
        //    var node = parser.ProduceTree();
        //    {
        //        nodes.Add(item);
        //        TestContext.WriteLine(item.NodeType + " " + item.NodeValue);
        //    }
        //    Assert.AreEqual(1, nodes.Count);
        //}

        [TestMethod]
        public void ParserSimpleActorTest()
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
        }

        [TestMethod]
        public void Parser2BehaviorActorTest()
        {
            string data =
                @"Actor bla
                    Behavior(""bla"")
                    {
                        Send Console (""bla"")
                    }
                    Behavior()
                    {                        
                    }
                 ";
            var parser = DataFactory.ParserFactory(data);
            var node = parser.ProduceTree();
            Assert.IsTrue(node.GetType() == typeof(ActorNode));
            int ident = 0;
            TestContext.WriteLine(data);
            TestContext.WriteLine(node.PrintNode(ident));
        }


    }

    ///
    /*
     * tiny actor language
     * actor bla
     *    behavior("bla")
     *      {
     *          send Console "bla"
     *      }
     *    behavior(Any)
     *      {
     *          send Console "not bla"
     *      }
     * actor IsBla
     *     behavior(any)
     *     {
     *          var blaActor = new bla 
     *          send blaActor message
     *     }
     *      
     */
}

