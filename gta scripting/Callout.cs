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
        public readonly Dictionary<string, CalloutCallable> calloutCallables = new Dictionary<string, CalloutCallable>();
        public readonly Dictionary<string, Stmt.When> whens = new Dictionary<string, Stmt.When>();

        public Callout(string CalloutMessage, Stmt stmt)
        {
            this.CalloutMessage = CalloutMessage;
            this.stmt = stmt;
        }
    }
}
