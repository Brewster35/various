using System;
using System.Collections.Generic;

namespace Parser
{
    class Tokeniser
    {
        public int Cursor;
        private string tokens;

        private readonly Dictionary<string, NodeType?> spec;

        public Tokeniser()
        {
            spec = new Dictionary<string, NodeType?>
            {
                // whitespace
                { @"^\s+", null },

                // single line comments
                { @"^\/\/.*", null },

                // multi line comments
                { @"^\/\*[\s\S]*?\*\/", null },

                // symbols and delimeters
                { @"^;", NodeType.SemiColon },
                { @"^\{", NodeType.OpeningBrace },
                { @"^\}", NodeType.ClosingBrace },

                // operators
                { @"^[+\-]", NodeType.AdditiveOperator },

                // integer
                { @"^\d+", NodeType.Number },

                // string in double quotes
                { @"""[^""]*""", NodeType.String },

                // string in single quotes
                { @"'[^']*'", NodeType.String }
            };
        }

        public string String
        {
            set
            {
                tokens = value;
            }
        }

        public bool HasMoreTokens()
        {
            return Cursor < tokens.Length;
        }

        public Node GetNextToken()
        {
            if (!HasMoreTokens())
            {
                return null;
            }

            string str = tokens.Substring(Cursor);

            foreach (var keyValue in spec)
            {
                string tokenValue = str.RegExMatch(keyValue.Key);

                Cursor += (tokenValue?.Length ?? 0);

                if (tokenValue == null)
                {
                    continue;
                }

                if (keyValue.Value == null)
                {
                    return GetNextToken();
                }

                return new Node(keyValue.Value).Value(tokenValue);
            }

            throw new Exception("Unexpected token... :(");
        }
    }

}
