using System;
using System.Collections.Generic;

namespace Parser
{
    class Parser
    {
        Node lookAhead;
        Tokeniser tokeniser;

        public Program Parse(string input)
        {
            tokeniser = new Tokeniser
            {
                String = input
            };

            lookAhead = tokeniser.GetNextToken();

            return Program();
        }

        private Node Eat(NodeType tokenType)
        {
            Node token = lookAhead;

            if (token == null)
            {
                throw new Exception("Unexpected end of input :(");
            }

            if (token.Type != tokenType)
            {
                throw new Exception("Unexpected token :(");
            }

            lookAhead = tokeniser.GetNextToken();

            return token;
        }

        private Node NumericLiteral()
        {
            Node token = Eat(NodeType.Number);

            return new Node(NodeType.Number).Value(token.Number);
        }

        private Node StringLiteral()
        {
            Node token = Eat(NodeType.String);

            return new Node(NodeType.String).Value(token.String.Trim(new char[] { '\"' }));
        }

        private Node Literal()
        {
            switch (lookAhead.Type)
            {
                case NodeType.Number:
                    return NumericLiteral();

                case NodeType.String:
                    return StringLiteral();
            }

            throw new Exception("Unknown type :(");
        }

        private Stack<Statement> StatementList(NodeType? stopLookAhead = null)
        {
            Stack<Statement> statementList = new Stack<Statement>();

            statementList.Push(Statement());

            while (lookAhead != null &&
                    lookAhead.Type != stopLookAhead)
            {
                statementList.Push(Statement());
            }

            return statementList;
        }

        private Statement Statement()
        {
            switch (lookAhead.Type)
            {
                case NodeType.SemiColon:
                    return EmptyStatement();

                case NodeType.OpeningBrace:
                    return BlockStatement();

                default:
                    return ExpressionStatement();
            }
            
        }

        private Statement EmptyStatement()
        {
            Eat(NodeType.SemiColon);

            return new Statement(NodeType.SemiColon);
        }

        private Statement BlockStatement()
        {
            Stack<Statement> body;

            Eat(NodeType.OpeningBrace);

            body = lookAhead.Type != NodeType.ClosingBrace ? 
                StatementList(NodeType.ClosingBrace) : 
                new Stack<Statement>();

            Eat(NodeType.ClosingBrace);

            return new Statement(NodeType.BlockStatement).Value(body);
        }

        private Statement ExpressionStatement()
        {
            Node expression = Expression();

            Eat(NodeType.SemiColon);

            return new Statement(NodeType.ExpressionStatement).Value(expression);
        }

        private Node Expression()
        {
            return AdditiveExpression();
        }

        private Node AdditiveExpression()
        {
            Node left = Literal();

            while (lookAhead.Type == NodeType.AdditiveOperator)
            {
                Operator op = Eat(NodeType.AdditiveOperator).Operator;

                Node right = Literal();

                Node newLeft = new Node(NodeType.BinaryExpression)
                {
                    Operator = op,
                    Left = left,
                    Right = right
                };

                left = newLeft;
            }

            return left;
        }

        public Program Program()
        {
            return new Program
            {
                Body = StatementList()
            };
        }
    }
}
