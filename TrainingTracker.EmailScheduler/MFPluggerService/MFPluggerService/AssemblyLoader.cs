/******************************************************************************** 
 * File Name: AssemblyLoader.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 08OCT2009 
 * Description: This contains the method to execute plugin by loading assemblies
 * through remoting by inheriting MarshalByRefObject Class.
 * ******************************************************************************/

using System;
using System.Reflection;

namespace MFPluggerService
{
    public class AssemblyLoader : MarshalByRefObject
    {
        /// <summary>
        /// Description:
        ///     This method will load and execute the assembly.
        /// </summary>
        /// <param name="assemblyName">Name of plugins assembly.</param>
        public void LoadAndExecute(string assemblyName)
        {
            // Load the MFServicePlugin.exe to the current application domain.
            AppDomain.CurrentDomain.Load("MFServicePlugin");

            // Load the plugin's assembly to the current application doamin.
            Assembly oAassembly = AppDomain.CurrentDomain.Load(assemblyName);

            // This block of code will execute the plugin's assembly code.
            foreach (Type oType in oAassembly.GetTypes())
            {
                if (oType.GetInterface("IMFServicePlugin") != null)
                {
                    object oPlugin = Activator.CreateInstance(oType, null, null);
                    ((IMFServicePlugin)oPlugin).ExecutePlugin();
                }
            }
        }
    }
}
