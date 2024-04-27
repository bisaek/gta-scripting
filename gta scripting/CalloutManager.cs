using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Windows.Forms;



namespace GtaScript
{
    [CalloutInfo("test", CalloutProbability.High)]
    public class CalloutManager : LSPD_First_Response.Mod.Callouts.Callout
    {
        public static readonly List<Callout> callouts = new List<Callout>();
        private readonly Interpreter interpreter;
        public static int currentCallout = 0;

        private readonly List<CalloutCallable> calloutCallables = new List<CalloutCallable>();
        private List<Tuple<Stmt.When, bool>> whens = new List<Tuple<Stmt.When, bool>>();

        public Ped suspect;

        public CalloutManager()
        {
            interpreter = new Interpreter(this);
        }

        public void createNewCallout(string CalloutMessage, Stmt stmt)
        {
            callouts.Add(new Callout(CalloutMessage, stmt));
        }

        public void createWhen(Stmt.When when)
        {
            whens.Add(new Tuple<Stmt.When, bool>(when, false));
        }

        public void createCalloutCallable(CalloutCallable calloutCallable)
        {
            calloutCallables.Add(calloutCallable);
        }




        public override bool OnBeforeCalloutDisplayed()
        {
            Random random = new Random();
            currentCallout = random.Next(0, callouts.Count);

            //Game.LogTrivial(callouts[currentCallout].CalloutMessage);
            Vector3 spawnpoint;

            spawnpoint = World.GetRandomPositionOnStreet();
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 30f);
            AddMinimumDistanceCheck(30f, spawnpoint);
            CalloutPosition = spawnpoint;
            CalloutMessage = callouts[currentCallout].CalloutMessage;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_RESISTING_ARREST_02 IN_OR_ON_POSITION", spawnpoint);
            interpreter.intrepret(new List<Stmt>() { callouts[currentCallout].stmt });
            
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            foreach (CalloutCallable calloutCallable in calloutCallables)
            {
                calloutCallable.OnAccepted(interpreter, this);
            }
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (Game.IsKeyDown(Keys.End))
            {
                End();
            }
            foreach(CalloutCallable calloutCallable in calloutCallables)
            {
                calloutCallable.Process(interpreter, this);
            }
            for (int i = 0; i < whens.Count; i++)
            {
                if ((bool)interpreter.evaluate(whens[i].Item1.condition) && !whens[i].Item2)
                {
                    interpreter.execute(whens[i].Item1.thenBranch);
                    whens[i] = new Tuple<Stmt.When, bool>(whens[i].Item1, true);
                }
            }
            base.Process();
            //timer++;
            //wait();
            





        }

        public override void End()
        {
            foreach (CalloutCallable calloutCallable in calloutCallables)
            {
                calloutCallable.End(interpreter, this);
            }
            base.End();

        }

    }
}
