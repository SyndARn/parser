using System;
using Xunit;
using parser;
using parsertest;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.Linq;

namespace xUnitTest
{
    public class xOperatorTest
    {
        private readonly ITestOutputHelper Output;

        public xOperatorTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public void OperatorLexerTest()
        {
            string data = @"Send a ""a""";
            var lexer = DataFactory.LexerFactory(data);
            List<IToken> tokens = new List<IToken>();
            foreach (var item in lexer.ProduceToken())
            {
                tokens.Add(item);
                Output.WriteLine(item.ToString());
            }
            Assert.Equal(3,tokens.Count);
            Assert.True(tokens.Exists(t => t.TokenType == TokenType.keyword));
        }
    }
}

