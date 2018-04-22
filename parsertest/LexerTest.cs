using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using parser;

namespace parsertest
{
    [TestClass]
    public class LexerTest
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
         [DataRow("0 1 2 3 4 5 6 7 8 9 \"123bbbcc\"", 11)]
        [DataRow("0 1 2", 3)]
        [DataRow(
            @"0 
              1 
              2", 3)]
        public void BasicLexingTest(string data, int qttToken)
        {
            var stream = DataFactory.StreamFactory(data);
            using (var reader = new StreamReader(stream))
            {
                var streamer = new InputStreamer(reader);
                var lexer = new Lexer(streamer);
                List<IToken> tokens = new List<IToken>();
                foreach (var item in lexer.ProduceToken())
                {
                    tokens.Add(item);
                    TestContext.WriteLine(item.ToString());
                }
                Assert.AreEqual(qttToken, tokens.Count);
            }
        }

        [TestMethod]
        [DataRow(@" Identifier=(3+2) ", "Identifier,=,(,3,+,2,)")]
        public void ParenthesisLexingTest(string data, string parseResult)
        {
            var stream = DataFactory.StreamFactory(data);
            using (var reader = new StreamReader(stream))
            {
                var streamer = new InputStreamer(reader);
                var lexer = new Lexer(streamer);
                List<IToken> tokens = new List<IToken>();
                foreach (var item in lexer.ProduceToken())
                {
                    tokens.Add(item);
                    TestContext.WriteLine(item.ToString());
                }

                var parse = parseResult.Split(',');
                Assert.AreEqual(parse.Length, tokens.Count);
                int i = 0;
                foreach (var s in parse)
                {
                    Assert.AreEqual(s, tokens[i].TokenValue);
                    i++;
                }
            }
        }

        [TestMethod]
        [DataRow(@" Identifier=3 ", "Identifier", "=", "3")]
        public void OperatorLexingTest(string data, string ident, string oper, string value)
        {
            var stream = DataFactory.StreamFactory(data);
            using (var reader = new StreamReader(stream))
            {
                var streamer = new InputStreamer(reader);
                var lexer = new Lexer(streamer);
                List<IToken> tokens = new List<IToken>();
                foreach (var item in lexer.ProduceToken())
                {
                    tokens.Add(item);
                    TestContext.WriteLine(item.ToString());
                }
                Assert.AreEqual(3, tokens.Count);
                Assert.AreEqual(ident, tokens[0].TokenValue);
                Assert.AreEqual(oper, tokens[1].TokenValue);
                Assert.AreEqual(value, tokens[2].TokenValue);
            }
        }

        [TestMethod]
        public void StartEndSequenceLexing()
        {
            string data = "{}";
            var lexer = DataFactory.LexerFactory(data);
            var startToken = lexer.ReadNext();
            var endToken = lexer.ReadNext();
            Assert.IsTrue(startToken.TokenType == TokenType.StartSequence);
            Assert.IsTrue(endToken.TokenType == TokenType.EndSequence);
        }


        [TestMethod]
        [DataRow(@" Identifier ", TokenType.Identifier, "Identifier")]
        public void IdentifierLexingTest(string data, TokenType token, string value)
        {
            var stream = DataFactory.StreamFactory(data);
            using (var reader = new StreamReader(stream))
            {
                var streamer = new InputStreamer(reader);
                var lexer = new Lexer(streamer);
                List<IToken> tokens = new List<IToken>();
                foreach (var item in lexer.ProduceToken())
                {
                    tokens.Add(item);
                    TestContext.WriteLine(item.ToString());
                }
                Assert.AreEqual(1, tokens.Count);
                Assert.AreEqual(token, tokens[0].TokenType);
                Assert.AreEqual(value, tokens[0].TokenValue);
            }
        }
    }
}
