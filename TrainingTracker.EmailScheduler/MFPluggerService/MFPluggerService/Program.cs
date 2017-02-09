/******************************************************************************** 
 * File Name: Program.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 16SEP2009 
 * Description: This contains the definitions of Main() function, which is the
 * entry point of application.
 * ******************************************************************************/

using System.ServiceProcess;

namespace MFPluggerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new MFPluggerService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
