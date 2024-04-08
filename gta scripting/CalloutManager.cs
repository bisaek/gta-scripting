using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using Rage;



namespace GtaScript
{
    [CalloutInfo("test", CalloutProbability.High)]
    public class CalloutManager : LSPD_First_Response.Mod.Callouts.Callout
    {
        public static readonly List<Callout> callouts = new List<Callout>();
        private readonly Interpreter interpreter;
        public static int currentCallout = 0;

        public Ped suspect;

        public CalloutManager()
        {
            interpreter = new Interpreter(this, "OnBefore");
        }

        public void createNewCallout(string CalloutMessage, Stmt stmt)
        {
            callouts.Add(new Callout(CalloutMessage, stmt));
        }

        public void createWhen(Stmt stmt)
        {

        }






        public override bool OnBeforeCalloutDisplayed()
        {
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
            interpreter.mode = "OnAccepted";
            interpreter.intrepret(new List<Stmt>() { callouts[currentCallout].stmt });
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {

            interpreter.mode = "Process";
            interpreter.intrepret(new List<Stmt>() { callouts[currentCallout].stmt });
            base.Process();
            //timer++;
            //wait();
            





        }

        public override void End()
        {
            interpreter.mode = "End";
            interpreter.intrepret(new List<Stmt>() { callouts[currentCallout].stmt });
            base.End();

        }

    }
}
