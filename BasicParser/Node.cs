using System;
using System.Collections.Generic;

namespace Parser
{
    enum Operator
    {
        Null,
        Add,
        Minus
    }

    enum NodeType
    {
        Program,

        SemiColon,
        OpeningBrace,
        ClosingBrace,

        ExpressionStatement,
        BlockStatement,

        Number,
        String,

        BinaryExpression,
        AdditiveOperator
    }

    class Statement
    {
        private NodeType type;
        private Node expression;
        private Stack<Statement> body;

        public Statement(NodeType? nodeType)
        {
            type = (NodeType)nodeType;
        }

        public Statement Value(Node expression)
        {
            this.expression = expression;

            return this;
        }

        public Statement Value(Stack<Statement> expression)
        {
            this.body = expression;

            return this;
        }
    }

    class Node
    {
        private NodeType type;
        //public string Value;
        private Node node;

        private int numberValue;
        private string stringValue;

        public Node Right;
        public Node Left;

        public Operator Operator;

        public NodeType Type
        {
            get
            {
                return type;
            }
        }

        public string String
        {
            get
            {
                return stringValue;
            }
        }

        public int Number
        {
            get
            {
                return numberValue;
            }
        }

        public Node(NodeType? nodeType)
        {
            type = (NodeType)nodeType;
        }

        public Node Value(int data)
        {
            numberValue = data;

            return this;
        }

        public Node Value(string data)
        {
            switch (type)
            {
                case NodeType.Number:
                    numberValue = int.Parse(data);
                    break;

                case NodeType.String:
                    stringValue = data;
                    break;

                case NodeType.SemiColon:
                case NodeType.OpeningBrace:
                case NodeType.ClosingBrace:
                    break;

                case NodeType.AdditiveOperator:
                    if (data == "+")
                    {
                        Operator = Operator.Add;
                    }

                    if (data == "-")
                    {
                        Operator = Operator.Minus;
                    }
                    break;

                default:
                    throw new Exception("Unknown type! :(");
            }
            
            return this;
        }

        public Node Value(Node node)
        {
            this.node = node;

            return this;
        }
    }

    class Program
    {
        public Stack<Statement> Body;
    }
}
