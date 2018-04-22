using System;
using System.Collections.Generic;
using System.Text;

namespace parser
{
    public class BaseTreeNode
    {
        public IToken Token { get; set; }
        public string NodeType { get; set; }
        public BaseTreeNode(IToken token)
        {
            Token = token;
            NodeType = GetType().Name;
        }
        public override string ToString() { return $"{NodeType} {Token.TokenValue}"; }
        public BaseTreeNode Left { get; set; }
        public BaseTreeNode Right { get; set; }
        public string PrintNode(int indent)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < indent; i++)
                sb.Append("--");
            sb.AppendLine(ToString());
            if (Left != null)
            {
                sb.Append(Left.PrintNode(indent + 1));
            }
            if (Right != null)
            {
                sb.Append(Right.PrintNode(indent + 1));
            }
            return sb.ToString();
        }
        public virtual void Parse(Parser parser)
        {
            throw new NotImplementedException(this.GetType().ToString());
        }
    }

    public class FailNode : BaseTreeNode
    {
        public FailNode(IToken token) : base(token)
        {
        }
    }

    public class IdentifierNode : BaseTreeNode
    {
        public IdentifierNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
        }
    }

    public class NumberNode : BaseTreeNode
    {
        public NumberNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
        }
    }

    public class StringNode : BaseTreeNode
    {
        public StringNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
        }
    }

    public class OperatorNode : BaseTreeNode
    {
        public OperatorNode(IToken token) : base(token)
        {
        }
        public override void Parse(Parser parser)
        {
        }
    }

    public class AssignNode : BaseTreeNode
    {
        public AssignNode(IToken token) : base(token)
        {
        }

        public string Operator { get; set; }
        public void IdentifierNode(IdentifierNode node)
        {
            Left = node;
        }
        public void ExpressionNode(ExpressionNode node)
        { Right = node; }
    }

    public class NodeList : BaseTreeNode
    {
        public NodeList(IToken token) : base(token)
        {
        }

        public void Add(BaseTreeNode node)
        {
            Left = node;
        }
        public override void Parse(Parser parser)
        {
        }
    }

    public class ExpressionNode : BaseTreeNode
    {
        public ExpressionNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            int parentLevel = 0;
            while (Token.TokenType == TokenType.Parenthesis)
            {
                parentLevel++;
                Token = parser.Lexer.ReadNext();
            }

            while ((Token.TokenType == TokenType.EndParenthesis) && (parentLevel>1))
            {
                parentLevel--;
                Token = parser.Lexer.ReadNext();
            }
            switch (Token.TokenType)
            {
                case TokenType.Number: Left = new NumberNode(Token); break;
                case TokenType.Operator: Left = new OperatorNode(Token); break;
                case TokenType.Identifier: Left = new IdentifierNode(Token); break;
                case TokenType.String: Left = new StringNode(Token); break;
                case TokenType.EndParenthesis: Token = new NullToken();  return;
            }
            var token = parser.ReadNext();
            Right = new ExpressionNode(token);
            Right.Parse(parser);
        }
    }



    public class SequenceNode : BaseTreeNode
    {
        public SequenceNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            var token = parser.Lexer.ReadNext();
            if (token.TokenType != TokenType.StartSequence)
            {
                Left = new FailNode(token);
                return;
            }
            token = parser.Lexer.ReadNext();
            if ((token.TokenType == TokenType.keyword) && (token.TokenValue == "Send"))
            {
                Left = new SendNode(token);
                Left.Parse(parser);
                token = parser.Lexer.ReadNext();
                if (token.TokenType != TokenType.EndSequence && token.TokenType != TokenType.Eos && token.TokenType != TokenType.Fail)
                {
                    Right = new SequenceNode(token);
                    Right.Parse(parser);
                }
            }
            if (token.TokenType != TokenType.EndSequence)
            {
                Right = new FailNode(token);
            }
        }
    }

    public class MessageFilterNode : BaseTreeNode
    {
        public MessageFilterNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            Token = new NullToken();
            Left = new ExpressionNode(this.Token);
            Left.Parse(parser);
        }
    }

    public class ApplyNode : BaseTreeNode
    {
        public ApplyNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            Left = new SequenceNode(Token);
            Left.Parse(parser);
        }
    }

    public class BehaviorNode : BaseTreeNode
    {
        public BehaviorNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            var token = parser.Lexer.ReadNext();
            Left = new MessageFilterNode(token);
            Left.Parse(parser);
            Right = new ApplyNode(token);
            Right.Parse(parser);
        }
    }

    public class SendNode : BaseTreeNode
    {
        public SendNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            var token = parser.Lexer.ReadNext();
            Left = new IdentifierNode(token);
            token = parser.Lexer.ReadNext();
            Right = new ExpressionNode(token);
            Right.Parse(parser);
        }
    }

    public class BehaviorsNode : BaseTreeNode
    {
        public BehaviorsNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            Left = new BehaviorNode(Token);
            Left.Parse(parser);
            var token = parser.Peek();
            if (token.TokenType == TokenType.Eos)
            {
                return ;
            }
            if (token.TokenType == TokenType.Fail)
            {
                Right = new FailNode(token);
                return;
            }
            Right = new BehaviorsNode(token);
            Right.Parse(parser);
        }
    }

    public class ActorNode : BaseTreeNode
    {
        public ActorNode(IToken token) : base(token)
        {
        }

        public override void Parse(Parser parser)
        {
            var token = parser.Lexer.ReadNext();
            if (token.TokenType == TokenType.Identifier)
            {
                Left = new IdentifierNode(token);
                token = parser.ReadNext();
                while (!(token.TokenType == TokenType.Eos) && !(token.TokenType == TokenType.Fail))
                {
                    Right = new BehaviorsNode(token);
                    Right.Parse(parser);
                    token = parser.Lexer.ReadNext();
                }
            }
        }
    }


}
