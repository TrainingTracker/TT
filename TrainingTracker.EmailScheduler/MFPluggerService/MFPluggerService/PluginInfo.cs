/******************************************************************************** 
 * File Name: PluginInfo.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 06OCT2009 
 * Description: This contains the information of plugin such as filename, loadtime,
 * loaddate of plugin.
 * ******************************************************************************/

using System;

namespace MFPluggerService
{
    struct PluginInfo
    {
        // Name of plugin's dll file.
        public string fileName;

        // Load time of plugin.
        public DateTime loadTime;

        // Execution interval of plugin.
        public TimeSpan executionInterval;

        // Get the plugin's active state.
        public bool isActive;
    }
}
