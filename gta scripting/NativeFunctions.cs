using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;



namespace GtaScript
{
    public class NativeFunctions
    {
        public class print : Callable
        {


            public int arity()
            {
                return 1;
            }

            public object call(Interpreter interpreter, List<object> arguments)
            {
               if(arguments.Count == 1)
                {
                    Game.LogTrivial((string)arguments[0]);
                }
                
                return null;
            }
        }
        public class isYPressed : Callable
        {
            public int arity()
            {
                return 0;
            }
            public object call(Interpreter interpreter, List<object> arguments)
            {
                if (Game.IsKeyDown(Keys.Y))
                {
                    return true;
                }
                return false;
            }
        }
        public class spawnSuspect : CalloutCallable
        {
            private Blip suspectBlip;
            public object OnBefore(Interpreter interpreter, CalloutManager calloutManager, List<object> arguments)
            {
                return null;
            }

            public object OnAccepted(Interpreter interpreter, CalloutManager calloutManager, List<object> arguments)
            {
                calloutManager.suspect = new Ped(calloutManager.CalloutPosition);
                suspectBlip = calloutManager.suspect.AttachBlip();
                suspectBlip.Color = Color.Red;
                suspectBlip.IsRouteEnabled = true;
                return null;
            }

            public object Process(Interpreter interpreter, CalloutManager calloutManager, List<object> arguments)
            {
                if (calloutManager.suspect.IsDead)
                {
                    calloutManager.End();
                }
                return null;
            }

            public object End(Interpreter interpreter, CalloutManager calloutManager, List<object> arguments)
            {
                suspectBlip.Delete();
                return null;
            }
        }
    }
}
