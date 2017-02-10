/******************************************************************************** 
 * File Name: MFAssemblyValidator.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 19SEP2009 
 * Description: This class will load all the dll's from repository and check whether
 * they are inheriting from IMFServicePlugin interface and returns the valid dlls.
 * ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MFPluggerService;

namespace MFAssemblyValidator
{
    public class MFAssemblyValidator
    {
        static void Main(string[] args)
        {
            List<string> assemblyNames = new List<string>();
            Assembly[] oAssemblies = new Assembly[args.Length];

            for (int assemblyCount = 0; assemblyCount < args.Length; assemblyCount++)
            {
                oAssemblies[assemblyCount] = Assembly.LoadFile(args[assemblyCount]);
            
                try
                {
                    foreach (Type oType in oAssemblies[assemblyCount].GetTypes())
                    {
                        // Check whether class is inheriting from IMFServicePlugin.
                        if (oType.GetInterface("IMFServicePlugin") == typeof(IMFServicePlugin))
                        {
                            assemblyNames.Add(args[assemblyCount].Substring(args[assemblyCount].LastIndexOf("\\") + 1));
                        }
                    }
                }
                catch (Exception ex) 
                {
                    EventLog log = new EventLog("Application");
                    log.Source = "MFPluggerService";
                    log.WriteEntry(ex.Message);
                }
            }

            // Passing data one application domain to another.
            AppDomain.CurrentDomain.SetData("AssemblyNames", assemblyNames.ToArray());
        }
    }
}
