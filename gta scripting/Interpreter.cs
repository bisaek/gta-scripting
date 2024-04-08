﻿using System;
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
        public string mode;


        public Interpreter(CalloutManager c, string mode)
        {
            calloutManager = c;
            this.mode = mode;
            environment = globals;

            globals.define("print", new NativeFunctions.print());
            globals.define("spawnSuspect", new NativeFunctions.spawnSuspect());
            globals.define("isYPressed", new NativeFunctions.isYPressed());
        }

        public void intrepret(List<Stmt> statements)
        {
            foreach(Stmt statement in statements)
            {
                execute(statement);
            }
        }

        private void execute(Stmt stmt)
        {
            stmt.Accept(this);
        }
        private object evaluate(Expr expr)
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
                if(mode == "OnAccepted")
                {
                    return function.OnAccepted(this, calloutManager, arguments);
                }
                else if(mode == "Process")
                {
                    return function.Process(this, calloutManager, arguments);
                }
                else if(mode == "End")
                {
                    return function.End(this, calloutManager, arguments);
                }
                else
                {
                    return function.OnBefore(this, calloutManager, arguments);
                }
                
            }
            else
            {
                Callable function = (Callable)callee;
                return function.call(this, arguments);
            }
            
        }

        public void visitStartStmt()
        {
            if(mode == "OnBefore")
            {
                calloutManager.start();
            }
            

        }

        public object visitLiteralExpr(Expr.Literal expr)
        {
            return expr.body;
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
            if ((bool)evaluate(stmt.condition))
            {
                Game.LogTrivial("burde kun være bed y");
                execute(stmt.thenBranch);
            }
        }
    }
}
