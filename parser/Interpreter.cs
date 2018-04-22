using System;
using System.Collections.Generic;
using System.Text;

namespace parser
{
    public class Interpreter
    {
        public Interpreter()
        {
        }
        public string Interpret(BaseTreeNode node, StringBuilder sb)
        {
            if (node == null) return "";
            if (sb == null)
            {
                sb = new StringBuilder();
            }
            switch(node.NodeType)
            {
                case "ActorNode":
                    {
                        sb.Append("public class ");
                        var actorName = Interpret(node.Left, sb);
                        sb.AppendLine(" : BaseActor");
                        sb.AppendLine("{");
                        sb.AppendLine($"public {node.Left.Token.TokenValue}()");
                        sb.AppendLine("{");
                        Interpret(node.Right, sb);
                        sb.AppendLine("}");
                        sb.AppendLine("}");
                        break;
                    }
                case "BehaviorNode":
                    {
                        sb.AppendLine("AddBehavior( new Behavior(");
                        Interpret(node.Left, sb);
                        sb.AppendLine(",");
                        Interpret(node.Right, sb);
                        sb.AppendLine(")) ;");
                        break;
                    }
                case "MessageFilterNode":
                    {
                        sb.AppendLine("Pattern = () => {");
                        Interpret(node.Left, sb);
                        Interpret(node.Right, sb);
                        sb.AppendLine("}");
                        break;
                    }
                case "ExpressionNode":
                    {
                        Interpret(node.Left, sb);
                        Interpret(node.Right, sb);
                        break;
                    }
                case "BehaviorsNode":
                    {
                        Interpret(node.Left, sb);
                        Interpret(node.Right, sb);
                        break;
                    }
                case "IdentifierNode":
                    {
                        sb.Append($" {node.Token.TokenValue} ");
                        break;
                    }
                case "StringNode":
                    {
                        sb.Append(node.Token.TokenValue);
                        break;
                    }
                case "SequenceNode":
                    {
                        sb.AppendLine("{");
                        Interpret(node.Left, sb);
                        Interpret(node.Right, sb);
                        sb.AppendLine("}");
                        break;
                    }
                case "ApplyNode":
                    {
                        sb.AppendLine("Apply = () => ");
                        Interpret(node.Left, sb);
                        Interpret(node.Right, sb);
                        break;
                    }
                case "SendNode":
                    {
                        sb.Append("SendMessage(");
                        Interpret(node.Left, sb);
                        sb.Append(",");
                        Interpret(node.Right, sb);
                        sb.Append(");");
                        break;
                    }
                default:
                    {
                        sb.AppendLine(node.ToString());
                        if (node.Left != null) Interpret(node.Left, sb);
                        if (node.Right != null) Interpret(node.Right, sb);
                        break; 
                    }
            }
            return sb.ToString();
        }
    }
}
