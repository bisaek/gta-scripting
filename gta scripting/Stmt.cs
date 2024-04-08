using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{
    public abstract class Stmt
    {

        public abstract void Accept(Visitor visitor);

        public interface Visitor
        {
            void visitCalloutStmt(Callout stmt);
            void visitStartStmt();
            void visitBlockStmt(Block stmt);
            void visitExpressionStmt(Expression stmt);
            void visitWhenStmt(When stmt);
        }

        public class Expression : Stmt
        {
            public Expression(Expr expression)
            {
                this.expression = expression;
            }
            public override void Accept(Visitor visitor)
            {
                visitor.visitExpressionStmt(this);
            }
            public readonly Expr expression;
        }

        public class Start : Stmt
        {
            public override void Accept(Visitor visitor)
            {
                visitor.visitStartStmt();
            }
        }

        public class Callout : Stmt
        {

            public Callout(Expr name, Stmt body)
            {
                this.name = name;
                this.body = body;
            }

            public override void Accept(Visitor visitor)
            {
                visitor.visitCalloutStmt(this);
            }

            public readonly Stmt body;
            public readonly Expr name;
        }

        public class Block : Stmt
        {

            public Block(List<Stmt> statements)
            {
                this.statements = statements;
            }

            public override void Accept(Visitor visitor)
            {
                visitor.visitBlockStmt(this);
            }

            public readonly List<Stmt> statements;
        }

        public class When : Stmt
        {
            public When(Expr condition, Stmt thenBranch)
            {
                this.condition = condition;
                this.thenBranch = thenBranch;
            }

            public override void Accept(Visitor visitor)
            {
                visitor.visitWhenStmt(this);
            }

            public readonly Expr condition;
            public readonly Stmt thenBranch;
        }
    }
}
