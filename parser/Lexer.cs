using System;
using System.Collections.Generic;
using System.Text;

namespace parser
{
    public interface IToken
    {
        TokenType TokenType { get; }
        string TokenValue { get; }
    }

    public enum TokenType
    {
        String,
        Number,
        keyword,
        Identifier,
        Null,
        Eos,
        Parenthesis,
        EndParenthesis,
        Operator,
        Fail,
        StartSequence,
        EndSequence
    }

    public class BaseToken : IToken
    {
        public override string ToString()
        {
            return $"{TokenType} {TokenValue}";
        }

        public TokenType TokenType { get; private set; }
        public string TokenValue { get; private set; }

        public BaseToken(TokenType type, string value)
        {
            TokenType = type;
            TokenValue = value;
        }
    }

    public class StartSequenceToken : BaseToken
    {
        public StartSequenceToken() : base(TokenType.StartSequence, "{")
        {
        }
    }

    public class EndSequenceToken : BaseToken
    {
        public EndSequenceToken() : base(TokenType.EndSequence, "}")
        {
        }
    }

    public class StringToken : BaseToken
    {
        public StringToken(string value) : base(TokenType.String, value) { }
    }

    public class NumberToken : BaseToken
    {
        public NumberToken(string value) : base(TokenType.Number, value) { }
    }

    public class NullToken : BaseToken
    {
        public NullToken() : base(TokenType.Null,"")
        {

        }
    }

    public class EosToken : BaseToken
    {
        public EosToken() : base(TokenType.Eos, "")
        {

        }
    }


    public class FailToken : BaseToken
    {
        public FailToken() : base(TokenType.Fail, "")
        {

        }
    }

    public class ParenthesisToken : BaseToken
    {
        public ParenthesisToken() : base(TokenType.Parenthesis, "(") { }
    }

    public class EndParenthesisToken : BaseToken
    {
        public EndParenthesisToken() : base(TokenType.EndParenthesis, ")") { }
    }

    public class IdentifierToken : BaseToken
    {
        public IdentifierToken(string identifierName) : base(TokenType.Identifier, identifierName) { }
    }

    public class KeywordToken : BaseToken
    {
        public KeywordToken(string identifierName) : base(TokenType.keyword, identifierName) { }
    }

    public class OperatorToken : BaseToken
    {
        public OperatorToken(string value) : base(TokenType.Operator, value) { }
    }

    public class Lexer
    {
        private InputStreamer Streamer;
        public Lexer(InputStreamer streamer)
        {
            Streamer = streamer;
        }

        public IEnumerable<IToken> ProduceToken()
        {
            IToken token = ReadNext();
            while (token.GetType() != typeof(EosToken))
            {
                yield return token;
                token = ReadNext();
            }
        }

        public void SkipWhiteSpace()
        {
            while(!Streamer.Eos())
            {
                char ch = Streamer.Peek();
                if (! char.IsWhiteSpace(ch))
                {
                    break;
                }
                Streamer.Next();
            }
        }

        public IToken ReadString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            Streamer.Next();
            while(! Streamer.Eos())
            {
                char ch = Streamer.Next();
                if (ch == '"')
                {
                    sb.Append("\"");
                    return new StringToken(sb.ToString());
                }
                sb.Append(ch);
            }
            return new FailToken();
        }

        public IToken ReadNumber()
        {
            StringBuilder sb = new StringBuilder();
            char ch0 = Streamer.Next();
            sb.Append(ch0);
            while (!Streamer.Eos())
            {
                char ch = Streamer.Peek();
                if (char.IsDigit(ch))
                {
                    Streamer.Next();
                    sb.Append(ch);
                }
                else
                {
                    return new NumberToken(sb.ToString());
                }
            }
            return new FailToken();
        }

        public IToken ReadIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            char ch0 = Streamer.Next();
            sb.Append(ch0);
            while(!Streamer.Eos())
            {
                char ch = Streamer.Peek();
                if (char.IsLetterOrDigit(ch))
                {
                    Streamer.Next();
                    sb.Append(ch);
                }
                else
                {
                    break;
                }
            }
            var s = sb.ToString();
            if (IsKeyword(s))
            {
                return new KeywordToken(s);
            }
            return new IdentifierToken(s);
        }

        public bool IsKeyword(string s)
        {
            return (s == "Actor") || (s == "Behavior") || (s == "MessageFilter") || (s == "Apply") || (s == "Send");
        }

        public bool IsOperator(char ch)
        {
            return @"=+-/*".IndexOf(ch) >= 0;
        }

        public int ParenthesisLevel { get; set; }

        private IToken Current;

        public IToken Peek()
        {
            if (Current != null)
            {
                return Current;
            }
            Current = DoReadNext();
            return Current;
        }

        public IToken ReadNext()
        {
            if (Current != null)
            {
                var current = Current ;
                Current = null;
                return current;
            }

            return DoReadNext();
        }

        private IToken DoReadNext()
        {
            if (Streamer.Eos())
            {
                return new EosToken();
            }

            SkipWhiteSpace();

            char ch = Streamer.Peek();

            if (char.IsWhiteSpace(ch))
            {
                return new EosToken();
            }

            if (ch == '{')
            {
                Streamer.Next();
                return new StartSequenceToken();
            }

            if (ch == '}')
            {
                Streamer.Next();
                return new EndSequenceToken();
            }

            if (ch == '"')
            {
                return ReadString();
            }

            if (char.IsDigit(ch))
            {
                return ReadNumber();
            }

            if (ch == '(')
            {
                Streamer.Next();
                ParenthesisLevel++;
                return new ParenthesisToken();
            }

            if (ch == ')')

            {
                if (ParenthesisLevel>0)
                {
                    Streamer.Next();
                    ParenthesisLevel--;
                    return new EndParenthesisToken();
                }
                return new FailToken();
            }

            if (char.IsLetter(ch))
            {
                return ReadIdentifier();
            }

            if (IsOperator(ch))
            {
                Streamer.Next();
                return new OperatorToken(ch.ToString());
            }

            return new FailToken();
            
        }

    }
}
