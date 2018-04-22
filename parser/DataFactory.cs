using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using parser; 

namespace parser
{
    public static class DataFactory
    {
        public static Stream StreamFactory(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static InputStreamer InputStreamerFactory(string s)
        {
            var stream = DataFactory.StreamFactory(s);
            var reader = new StreamReader(stream);
            var streamer = new InputStreamer(reader);
            return streamer;
        }

        public static Lexer LexerFactory(string s)
        {
            var inputStreamer = DataFactory.InputStreamerFactory(s);
            var lexer = new Lexer(inputStreamer);
            return lexer;
        }

        public static Parser ParserFactory(string s)
        {
            var lexer = DataFactory.LexerFactory(s);
            var parser = new Parser(lexer);
            return parser;
        }
    }
}
