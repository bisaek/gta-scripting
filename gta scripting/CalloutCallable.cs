using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{
    public interface CalloutCallable
    {
        object OnBefore(Interpreter interpreter, CalloutManager calloutManager, List<object> arguments);
        object OnAccepted(Interpreter interpreter, CalloutManager calloutManager);
        object Process(Interpreter interpreter, CalloutManager calloutManager);
        object End(Interpreter interpreter, CalloutManager calloutManager);
    }
}
