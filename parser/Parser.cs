using System;
using System.Linq;

namespace parser
{
    public class Parser
    {
        public Lexer Lexer { get; set; }
        public Parser(Lexer lexer)
        {
            Lexer = lexer;
        }

        public BaseTreeNode ProduceTree()
        {
            var token = Lexer.ProduceToken().First();
            if (token.TokenType == TokenType.keyword)
            {
                if (token.TokenValue == "Actor")
                {
                    var actorNode = new ActorNode(token);
                    actorNode.Parse(this);
                    return actorNode;
                }
            }
            throw new Exception("Incorrect tree");
        }

        public IToken Peek()
        {
            return Lexer.Peek();
        }

        public IToken ReadNext()
        {
            return Lexer.ReadNext();
        }

    }


}
