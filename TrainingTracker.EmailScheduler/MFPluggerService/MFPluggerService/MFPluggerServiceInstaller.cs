/******************************************************************************** 
 * File Name: MFPluggerServiceInstaller.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 17SEP2009 
 * Description: This contains a partial class, which initialise the installer
 * componants placed on design part.
 * ******************************************************************************/

using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace MFPluggerService
{
    [RunInstaller(true)]
    public partial class MFPluggerServiceInstaller : Installer
    {
        public MFPluggerServiceInstaller()
        {
            InitializeComponent();
        }

        private void svcInstaller_Committed(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController("MFPluggerServiceV2");
            sc.Start(); 
        }
    }
}
