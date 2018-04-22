using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using parser;

namespace parsertest
{
    [TestClass]
    public class InputStreamerTest
    {
        [TestMethod]
        public void TestInputStreamer()
        {
            string data = "0 1 2 3 4 5 6 7 8 9 \"123bbbcc\"";
            string expected = "0 1 2 3 4 5 6 7 8 9 \"123bbbcc\"";
            StringBuilder sb = new StringBuilder();
            Stream stream = DataFactory.StreamFactory(data);
            using (var reader = new StreamReader(stream))
            {
                var streamer = new InputStreamer(reader);
                while (!streamer.Eos())
                {
                    sb.Append(streamer.Next());
                }
            }
            Assert.AreEqual(expected, sb.ToString());
        }
    }
}
