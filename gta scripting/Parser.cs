using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private Stmt statement()
        {
            if (match(TokenType.CALLOUT)) return calloutStatement();
            if(match(TokenType.START)) return startStatement();
            if (match(TokenType.LEFT_BRACE)) return new Stmt.Block(block());
            if (match(TokenType.WHEN)) return whenStatement();

            
            return new Stmt.Expression(expression());
            throw new Exception("syntax error");

        }

        private Stmt whenStatement()
        {

            
            advance();
            Console.WriteLine(peek().toString());
            Expr condition = expression();
            advance();
            Console.WriteLine(peek().toString());
            Stmt thenBranch = statement();
            Console.WriteLine(peek().toString());
            return new Stmt.When(condition, thenBranch);
        }

        private Stmt calloutStatement()
        {
            return new Stmt.Callout(expression(), statement());
        }

        private Stmt startStatement()
        {
            return new Stmt.Start();
        }

        private List<Stmt> block()
        {
            List<Stmt> statements = new List<Stmt>();

            while(!check(TokenType.RIGHT_BRACE) && !isAtEnd())
            {
                statements.Add(statement());
            }
            advance();
            return statements;
        }

        private Expr expression()
        {
            return call();
        }

        private Expr call()
        {
            Expr expr = primary();

            while (true)
            {
                
                if (match(TokenType.LEFT_PAREN))
                {
                    expr = finishCall(expr);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expr finishCall(Expr callee)
        {
           
            List<Expr> arguments = new List<Expr>();
            if (!check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(expression());
                } while (match(TokenType.COMMA));
            }

            Token paren = advance();

            return new Expr.Call(callee, paren, arguments);
        }

        private Expr primary()
        {

            
            if (match(TokenType.STRING))
            {
                return new Expr.Literal(previous().literal);
            }
            if (match(TokenType.IDENTIFIER))
            {
                return new Expr.Variable(previous());
            }

            throw new Exception("syntax error");

        }

        public List<Stmt> parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!isAtEnd())
            {
                statements.Add(statement());
            }

            return statements;
            
        }

        private Token advance()
        {
            if (!isAtEnd()) current++;
            return previous();
        }

        private Token previous()
        {
            return tokens[current - 1];
        }

        private Boolean match(params TokenType[] types)
        {
            foreach(TokenType type in types)
            {
                if (check(type))
                {
                    advance();
                    return true;
                }
            }

            return false;
        }

        private Boolean check(TokenType type)
        {
            if (isAtEnd()) return false;
            return peek().type == type;
        }

        private Boolean isAtEnd()
        {
            return peek().type == TokenType.EOF;
        }

        private Token peek()
        {
            return tokens[current];
        }
    }
}
