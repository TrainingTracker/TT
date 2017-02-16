using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MFPluggerService;

namespace MFPluggerUtility_TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            // This Program is just a helper to test your Dll..
            // It wont go Anywhere.. See MFServicePlugger for how and why!!         

            // Change the Assembly name here "TrainingTracker.TaskScheduler"  if creating another plugin
            Assembly assembly = AppDomain.CurrentDomain.Load("TrainingTracker.TaskScheduler");

            // This block of code will execute the plugin's assembly code.
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface("IMFServicePlugin") != null)
                {
                    object oPlugin = Activator.CreateInstance(type, null, null);
                    ((IMFServicePlugin)oPlugin).ExecutePlugin();
                }
            }

            Console.Read();
        }
    }

}
