
namespace Parser
{

    class TheProgram
    {
        static void Main(string[] args)
        {
            Program ast;
            Parser parser;
            string program;

            parser = new Parser();
            program = @"    


// a number:
42 + 3 - 1;

/*
 * multi line comment
 */

";

            ast = parser.Parse(program);
        }
    }
}
