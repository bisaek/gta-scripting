using System;
using System.IO;
using GtaScript;
using System.Collections.Generic;

namespace consoleTest
{
    class Program
    {
        private static readonly Interpreter interpreter = new Interpreter(new CalloutManager());
        static void Main(string[] args)
        {
            string textFile = @"C:\Users\Bruger\source\repos\gta scripting\gta scripting\test.gta";

            string file = File.ReadAllText(textFile);


            Scanner scanner = new Scanner(file);
            List<Token> tokens = scanner.scanTokens();
            foreach(Token token in tokens)
            {
                //Console.WriteLine(token.toString());
            }
            
            Parser parser = new Parser(tokens);
            List < Stmt > code = parser.parse();

            interpreter.intrepret(code);

        }
    }
}
