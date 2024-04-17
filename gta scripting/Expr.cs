using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{

    public abstract class Expr
    {

        public interface Visitor
        {
            object visitBinaryExpr(Binary expr);
            object visitGroupingExpr(Grouping expr);
            object visitLiteralExpr(Literal expr);
            object visitUnaryExpr(Unary expr);

            object visitCallExpr(Call expr);
            object visitVariableExpr(Variable expr);
        }

        public abstract object accept(Visitor visitor);

        public class Binary : Expr
        {
            public Binary(Expr left, Token operator_, Expr right)
            {
                this.left = left;
                this.operator_ = operator_;
                this.right = right;
            }

            public override object accept(Visitor visitor)
            {
                return visitor.visitBinaryExpr(this);
            }

            public readonly Expr left;
            public readonly Token operator_;
            public readonly Expr right;
        }

        public class Grouping : Expr
        {

            public Grouping(Expr expression)
            {
                this.expression = expression;
            }

            public override object accept(Visitor visitor)
            {
                return visitor.visitGroupingExpr(this);
            }

            public readonly Expr expression;
        }


        public class Literal : Expr
        {

            public Literal(object body)
            {
                this.body = body;
            }

            public override object accept(Visitor visitor)
            {
                return visitor.visitLiteralExpr(this);
            }

            public readonly object body;
        }

        public class Unary : Expr
        {

            public Unary(Token operator_, Expr right)
            {
                this.operator_ = operator_;
                this.right = right;
            }

            public override object accept(Visitor visitor)
            {
                return visitor.visitUnaryExpr(this);
            }

            public readonly Token operator_;
            public readonly Expr right;
        }

        public class Call : Expr
        {

            public Call(Expr callee, Token paren, List<Expr> arguments)
            {
                this.callee = callee;
                this.paren = paren;
                this.arguments = arguments;
            }

            public override object accept(Visitor visitor)
            {
                return visitor.visitCallExpr(this);
            }

            public readonly Expr callee;
            public readonly Token paren;
            public readonly List<Expr> arguments;
        }

        public class Variable : Expr
        {
            public Variable(Token name)
            {
                this.name = name;
            }
            public override object accept(Visitor visitor)
            {
                return visitor.visitVariableExpr(this);
            }
            public readonly Token name;
        }

    }
}
