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
            object visitLiteralExpr(Literal expr);
            object visitCallExpr(Call expr);
            object visitVariableExpr(Variable expr);
        }

        public abstract object accept(Visitor visitor);

        
        public class Literal : Expr
        {

            public Literal(string body)
            {
                this.body = body;
            }

            public override object accept(Visitor visitor)
            {
                return visitor.visitLiteralExpr(this);
            }

            public readonly string body;
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
