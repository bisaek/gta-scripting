using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;



namespace GtaScript
{
    public class Interpreter : Stmt.Visitor, Expr.Visitor
    {
        public readonly CalloutManager calloutManager;
        private readonly Environment globals = new Environment();
        private Environment environment;


        public Interpreter(CalloutManager c)
        {
            calloutManager = c;
            environment = globals;

            globals.define("print", new NativeFunctions.print());
            globals.define("spawnSuspect", new NativeFunctions.spawnSuspect());
            globals.define("isYPressed", new NativeFunctions.isYPressed());
            globals.define("getSuspectDistanceToPlayer", new NativeFunctions.getSuspectDistanceToPlayer());
        }

        public void intrepret(List<Stmt> statements)
        {
            foreach(Stmt statement in statements)
            {
                execute(statement);
            }
        }

        public void execute(Stmt stmt)
        {
            stmt.Accept(this);
        }
        public object evaluate(Expr expr)
        {
            return expr.accept(this);
        }



        public void visitBlockStmt(Stmt.Block stmt)
        {
            Console.WriteLine("block start");
            foreach(Stmt statement in stmt.statements)
            {
                execute(statement);
            }
            Console.WriteLine("block end");
        }

        public void visitCalloutStmt(Stmt.Callout stmt)
        {

            Console.WriteLine("hello world i am cool");
            Game.LogTrivial("hello");
            
            calloutManager.createNewCallout((string)evaluate(stmt.name), stmt.body);
            execute(stmt.body);
        }

        public object visitCallExpr(Expr.Call expr)
        {
            object callee = evaluate(expr.callee);

            List<object> arguments = new List<object>();
            foreach(Expr argument in expr.arguments)
            {
                arguments.Add(evaluate(argument));
            }
            if(callee is CalloutCallable)
            {
                CalloutCallable function = (CalloutCallable)callee;
                calloutManager.createCalloutCallable(function);
                return function.OnBefore(this, calloutManager, arguments);
            
                
            }
            else
            {
                Callable function = (Callable)callee;
                return function.call(this, arguments);
            }
            
        }

        public void visitStartStmt()
        {
            

        }

        public object visitBinaryExpr(Expr.Binary expr)
        {
            object left = evaluate(expr.left);
            object right = evaluate(expr.right);

            switch (expr.operator_.type)
            {
                case TokenType.GREATER:
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL:
                    return !isEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return isEqual(left, right);
                case TokenType.MINUS:
                    return (double)left - (double)right;
                case TokenType.SLASH:
                    return (double)left / (double)right;
                case TokenType.STAR:
                    return (double)left * (double)right;
                case TokenType.PLUS:
                    if(left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if(left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }

                    break;
            }

            return null;
        }

        public object visitLiteralExpr(Expr.Literal expr)
        {
            return expr.body;
        }

        public object visitGroupingExpr(Expr.Grouping expr)
        {
            return evaluate(expr.expression);
        }

        public object visitUnaryExpr(Expr.Unary expr)
        {
            object right = evaluate(expr.right);

            switch (expr.operator_.type)
            {
                case TokenType.BANG:
                    return !isTruthy(right);
                case TokenType.MINUS:
                    return -(double)right;
            }

            return null;
        }
        private bool isTruthy(object o)
        {
            if (o == null) return false;
            if (o is bool) return (bool)o;
            return true;
        }

        private bool isEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        public object visitVariableExpr(Expr.Variable expr)
        {
            return globals.get(expr.name);
        }

        

        public void visitExpressionStmt(Stmt.Expression stmt)
        {
            evaluate(stmt.expression);
        }

        public void visitWhenStmt(Stmt.When stmt)
        {
            calloutManager.createWhen(stmt);
            //if ((bool)evaluate(stmt.condition))
            //{
            //    Game.LogTrivial("burde kun være bed y");
            //    execute(stmt.thenBranch);
            //}
        }
    }
}
