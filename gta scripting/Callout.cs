using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{
    public class Callout
    {
        public Stmt stmt;
        public string CalloutMessage;
        //public readonly List<CalloutCallable> calloutCallables = new List<CalloutCallable>();
        //public readonly List<Stmt.When> whens = new List<Stmt.When>();

        public Callout(string CalloutMessage, Stmt stmt)
        {
            this.CalloutMessage = CalloutMessage;
            this.stmt = stmt;
        }
    }
}
