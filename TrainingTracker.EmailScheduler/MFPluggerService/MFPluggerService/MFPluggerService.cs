/******************************************************************************** 
 * File Name: MFPluggerService.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 16SEP2009 
 * Description: This contains initialization of service componants, service timer
 * and methods to execute the valid plugins.
 * ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using System.Xml;

namespace MFPluggerService
{
    public partial class MFPluggerService : ServiceBase
    {
        /// <summary>
        /// Description:
        ///     This is a constructor of class, which initializes all the componants of service. 
        /// </summary>
        public MFPluggerService()
        {
            // Initialize componants placed on design part of this service.            
            InitializeComponent();
            // Instantiate the System.Timers.Timer Class.
            System.Timers.Timer pluginTimer = new System.Timers.Timer();

            double tmrInterval = new double();

            // Check TimerInterval for null.
            if (ConfigurationManager.AppSettings["TimerInterval"] != null)
            {
                double.TryParse(ConfigurationManager.AppSettings["TimerInterval"], out tmrInterval);
            }

            // Multiply by 1000, because timer interval is in milisecond.
            pluginTimer.Interval = tmrInterval * 1000;
            pluginTimer.Enabled = true;
            pluginTimer.AutoReset = true;
            pluginTimer.Elapsed += new ElapsedEventHandler(pluginTimer_Elapsed);
        }

        /// <summary>
        /// Description:
        ///     This property gives the windows service startup path
        /// </summary>
        private static string ServiceStartPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            }
        }

        /// <summary>
        /// Description:
        ///     This is a event handler for timer's elapse event. If loadtime of plugin is between
        ///     the time interval the it will execute the plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pluginTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            foreach (PluginInfo pluginInfo in GetRegisteredPluginInfos())
            {
                DateTime loadTime = pluginInfo.loadTime;                

                // If specified file is not exists in Plugins folder then make IsActive node
                // in the PluginInfos.xml to False.

                string pluginsPath = ServiceStartPath + "\\" + ConfigurationManager.AppSettings["PluginsFolderName"];

                if (!File.Exists(pluginsPath + "\\" + pluginInfo.fileName))
                {
                    XmlDocument oPluginInfo = new XmlDocument();

                    try
                    {
                        
                        oPluginInfo.Load(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

                        // write the <IsActive> to PluginInfos.xml file
                        XmlNode nodeIsActive = oPluginInfo.SelectSingleNode("Plugins/Plugin/IsActive[../@FileName='" + pluginInfo.fileName + "']");
                        nodeIsActive.InnerText = "False";
                    }
                    catch (Exception ex)
                    {
                        // Write the exception message to event logs.
                        eventLog.WriteEntry(ex.Message);
                    }
                    finally
                    {
                        oPluginInfo.Save(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

                        // Nullify the XmlDocument's object.
                        if (oPluginInfo != null)
                        {
                            oPluginInfo = null;
                        }
                    }

                    eventLog.WriteEntry("File '" + pluginInfo.fileName + "' is not found in Plugins Folder.");
                }

                // It will check the plugin's active state(by isActive parameter) and load time,
                // if they meet the requirement then it will run the plugin.
                else if (loadTime <= DateTime.Now && pluginInfo.isActive)
                {
                    // Execute the specified plugin as a separate thread.
                    new Thread(delegate() { CreateInstanceAndRun(pluginInfo.fileName); }).Start();

                    XmlDocument oPluginInfo = new XmlDocument();

                    // This block of code will write the last loaded time and active state
                    // of plugin to PluginInfos.xml file.
                    try
                    {
                        oPluginInfo.Load(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

                        // write the <LastLoadedTime> to PluginInfos.xml file
                        XmlNode nodeLastLoadedTime = oPluginInfo.SelectSingleNode("Plugins/Plugin/LastLoadedTime[../@FileName='" + pluginInfo.fileName + "']");
                        nodeLastLoadedTime.InnerText = loadTime.ToString();

                        // write the <NextLoadTime> to PluginInfos.xml file
                        XmlNode nodeNextLoadTime = oPluginInfo.SelectSingleNode("Plugins/Plugin/NextLoadTime[../@FileName='" + pluginInfo.fileName + "']");
                        nodeNextLoadTime.InnerText = (loadTime + pluginInfo.executionInterval).ToString();

                    }
                    catch (Exception ex)
                    {
                        // Write the exception message to event logs.
                        eventLog.WriteEntry(ex.Message);
                    }
                    finally
                    {
                        oPluginInfo.Save(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

                        // Nullify the XmlDocument's object.
                        if (oPluginInfo != null)
                        {
                            oPluginInfo = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Description:
        ///     Gets the registered plugins from the PluginsInfo.xml file.
        /// </summary>
        /// <returns>Registered plugin's information.</returns>
        private List<PluginInfo> GetRegisteredPluginInfos()
        {
            // List of PluginInfo structure to get the plugins information.
            List<PluginInfo> pluginInfos = new List<PluginInfo>();

            XmlDocument oPluginInfosXml = new XmlDocument();

            try
            {
                // Load the PluginInfos.xml file through XmlDocument.
                oPluginInfosXml.Load(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

                PluginInfo pluginInfo = new PluginInfo();

                // This block will retrieve the name, load time, load date, and active state
                // of plugin from PluginInfos.xml file.
                foreach (XmlNode node in oPluginInfosXml.SelectNodes("Plugins/Plugin"))
                {
                    pluginInfo.fileName = node.Attributes["FileName"].Value;

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "NextLoadTime")
                        {
                            DateTime.TryParse(childNode.InnerText, out pluginInfo.loadTime);
                        }

                        if (childNode.Name == "ExecutionInterval")
                        {
                            int execInterval;
                            int.TryParse(childNode.InnerText, out execInterval);
                            pluginInfo.executionInterval = new TimeSpan(0, execInterval, 0);
                        }

                        if (childNode.Name == "IsActive")
                        {
                            bool.TryParse(childNode.InnerText.ToLower(), out pluginInfo.isActive);
                        }
                    }

                    pluginInfos.Add(pluginInfo);
                }
            }
            catch (Exception ex)
            {
                // Write the exception message to event logs.
                eventLog.WriteEntry(ex.Message);
            }
            finally
            {
                // Nullify the XmlDocument's object.
                if (oPluginInfosXml != null)
                {
                    oPluginInfosXml = null;
                }
            }

            return pluginInfos;
        }

        /// <summary>
        /// Description:
        ///     Create Instance and execute the code of specified assembly.
        /// </summary>
        /// <param name="fileName">Name of file to be executed.</param>
        private void CreateInstanceAndRun(string fileName)
        {
            // Create application domain setup for new application domain.
            AppDomainSetup oDomainSetup = new AppDomainSetup();
            oDomainSetup.PrivateBinPath = "Plugins";

            // Create a new application domain.
            AppDomain oDomain = AppDomain.CreateDomain(fileName, null, oDomainSetup);

            try
            {
                // Instantiate the AssemblyLoader Class in a new application domain.
                AssemblyLoader oAssemblyLoader = (AssemblyLoader)oDomain.CreateInstanceAndUnwrap("MFPluggerService", "MFPluggerService.AssemblyLoader");

                // Load and execute the plugin in a new application domain through remoting.
                oAssemblyLoader.LoadAndExecute(fileName.Substring(0, fileName.LastIndexOf('.')));
            }
            catch (Exception ex)
            {
                // Write the exception message to event logs.
                eventLog.WriteEntry(ex.Message);
            }
            finally
            {
                // Unload the application domain.
                AppDomain.Unload(oDomain);
            }
        }
    }
}
