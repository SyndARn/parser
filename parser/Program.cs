using System;
using System.Text;

namespace parser
{
    class Program
    {
        static void Main(string[] args)
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
            int ident = 0;
            Console.WriteLine(data);
            Console.WriteLine(node.PrintNode(ident));
            var interpreter = new Interpreter();
            StringBuilder sb = new StringBuilder();
            var result = interpreter.Interpret(node, sb);
            Console.WriteLine(result);
            // Console.ReadLine();
        }
    }
}
