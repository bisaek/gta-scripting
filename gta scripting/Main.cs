using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Reflection;
using System.IO;



namespace GtaScript
{
    public class Main : Plugin
    {
        private static readonly Interpreter interpreter = new Interpreter(new CalloutManager(), "main");


        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            //Game.DisplayNotification("danish callouts");
            Game.LogTrivial("gta scripting");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);

        }
        public override void Finally()
        {
            Game.LogTrivial("wow");
        }
        private static void OnOnDutyStateChangedHandler(bool onDuty)
        {
            if (onDuty)
            {

                string textFile = @"C:\Users\Bruger\source\repos\gta scripting\gta scripting\test.gta";

                string file = File.ReadAllText(textFile);


                Scanner scanner = new Scanner(file);
                List<Token> tokens = scanner.scanTokens();
               
                Parser parser = new Parser(tokens);
                List<Stmt> code = parser.parse();

                interpreter.intrepret(code);

                Game.DisplayNotification("gta scripting is loaded");
                Game.LogTrivial("Gta scripting");
                RegisterCallouts();
            };

            //Game.DisplayNotification("danish callouts by me");
        }

        private static void RegisterCallouts()
        {

            Functions.RegisterCallout(typeof(CalloutManager));
        }

        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }
        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0) return true;
                }
            }
            return false;
        }
    }
}

