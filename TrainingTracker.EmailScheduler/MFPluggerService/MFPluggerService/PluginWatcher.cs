/******************************************************************************** 
 * File Name: PluginWatcher.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 17SEP2009 
 * Description: This contains the FileSystemWatcher to Plugins folder to manage 
 * the Plugin loading and unloading, when plugin libraries are added or removed.
 * ******************************************************************************/

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace MFPluggerService
{
    public class PluginWatcher
    {
        /// <summary>
        /// Description:
        ///     This constructor will instantiate a FileSystemWatcher, set its properties
        ///     and declare its event handler.
        /// </summary>
        public PluginWatcher()
        {
            //it is watching dlls only.
            string pluginsPath = ServiceStartPath + "\\" + ConfigurationManager.AppSettings["PluginsFolderName"];

            FileSystemWatcher pluginWatcher = new FileSystemWatcher(pluginsPath, "*.dll");

            pluginWatcher.EnableRaisingEvents = true;
            pluginWatcher.Created += new FileSystemEventHandler(pluginWatcher_Created);
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
        ///     This is a event handler for pluginWatcher, which loads the .dll file to
        ///     the memory when they pasted to the Plugins folder according to its specifications.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pluginWatcher_Created(object sender, FileSystemEventArgs e)
        {
            // This block will save the information to PluginInfo.xml of valid pasted dll
            // into repository.
            foreach (string assemblyName in GetAssemblies())
            {
                if (assemblyName == e.Name)
                {
                    SavePluginInfo(assemblyName);
                }
            }
        }

        /// <summary>
        /// Description:
        ///     This will check the plug-in repository and get the dll's which have the 
        ///     implementation of IMFServicePlugin.
        /// </summary>
        /// <returns>Returns the valid assemblies.</returns>
        private static string[] GetAssemblies()
        {
            string[] validAssemblies = null;

            AppDomainSetup oDomainSetup = new AppDomainSetup();
            oDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            oDomainSetup.ConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + @"\MFPluggerService.exe.config";

            // This block will create an application domain, execute the MFAssemblyValidator assembly
            // and get the name of valid assemblies from repository.
            if (AppDomain.CurrentDomain.FriendlyName != "AssemblyValidatorDomain")
            {
                AppDomain oAppDomain = AppDomain.CreateDomain("AssemblyValidatorDomain", null, oDomainSetup);

                string pluginsPath = ServiceStartPath + "\\" + ConfigurationManager.AppSettings["PluginsFolderName"];

                // Execute the MFAssemblyValidator.exe file.
                oAppDomain.ExecuteAssembly(AppDomain.CurrentDomain.BaseDirectory + @"\MFAssemblyValidator.exe", null,
                    Directory.GetFiles(pluginsPath, "*.dll"));
                
                validAssemblies = (string[])oAppDomain.GetData("AssemblyNames");

                AppDomain.Unload(oAppDomain);
            }

            return validAssemblies;
        }

        /// <summary>
        /// Description:
        ///     This will save all the information of plugin to pluginInfo.xml file.
        /// </summary>
        /// <param name="fileName">Assembly name of plugin.</param>
        private static void SavePluginInfo(string fileName)
        {
            string name = string.Empty;
            string description = string.Empty;
            DateTime loadTime = DateTime.Now;
            string executionInterval = string.Empty;

            // get the information of specified plugin from plugin's config file.
            GetPluginInfo(fileName, ref name, ref description, ref loadTime, ref executionInterval);

            XmlDocument oPluginInfosXml = new XmlDocument();

            if (!File.Exists(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]))
            {
                XmlTextWriter oXmlWriter = new XmlTextWriter(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"], System.Text.Encoding.UTF8);
                oXmlWriter.Formatting = Formatting.Indented;
                oXmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                oXmlWriter.WriteStartElement("Plugins");
                oXmlWriter.Close();
            }

            oPluginInfosXml.Load(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

            if (!IsPluginRegistered(fileName)) // Check whether plugin is registered or not.
            {
                // Create <Plugins> element.
                XmlNode oPlugins = oPluginInfosXml.DocumentElement;

                // Create <Plugin FileName="PluginName.dll"> element.
                XmlElement oPlugin = oPluginInfosXml.CreateElement("Plugin");
                oPlugins.AppendChild(oPlugin);
                oPlugin.SetAttribute("FileName", fileName);

                // Create <Name> element.
                XmlElement oName = oPluginInfosXml.CreateElement("Name");
                oPlugin.AppendChild(oName);
                oName.AppendChild(oPluginInfosXml.CreateTextNode(name));

                // Create <Description> element.
                XmlElement oDescription = oPluginInfosXml.CreateElement("Description");
                oPlugin.AppendChild(oDescription);
                oDescription.AppendChild(oPluginInfosXml.CreateTextNode(description));

                // Create <NextLoadTime> element.
                XmlElement oNextLoadTime = oPluginInfosXml.CreateElement("NextLoadTime");
                oPlugin.AppendChild(oNextLoadTime);
                oNextLoadTime.AppendChild(oPluginInfosXml.CreateTextNode(loadTime.ToString()));

                // Create <ExecutionInterval> element.
                XmlElement oExecutionInterval = oPluginInfosXml.CreateElement("ExecutionInterval");
                oPlugin.AppendChild(oExecutionInterval);
                oExecutionInterval.AppendChild(oPluginInfosXml.CreateTextNode(executionInterval));

                // Create <IsActive> element.
                XmlElement oIsActive = oPluginInfosXml.CreateElement("IsActive");
                oPlugin.AppendChild(oIsActive);
                oIsActive.AppendChild(oPluginInfosXml.CreateTextNode("True"));

                // Create <LastLoadedTime> element.
                XmlElement oLastLoadedTime = oPluginInfosXml.CreateElement("LastLoadedTime");
                oPlugin.AppendChild(oLastLoadedTime);
                oLastLoadedTime.AppendChild(oPluginInfosXml.CreateTextNode(""));
            }

            oPluginInfosXml.Save(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

            // Nullify the XmlDocument's object.
            if (oPluginInfosXml != null)
            {
                oPluginInfosXml = null;
            }
        }

        /// <summary>
        /// Description:
        ///     This will get all the information of the specified plugin 
        ///     from it's config file.
        /// </summary>
        /// <param name="fileName">Assembly name of plugin.</param>
        /// <param name="name">Name of plugin.</param>
        /// <param name="description">Description of plugin.</param>
        /// <param name="loadTime">Load time of plugin.</param>
        /// <param name="loadDate">Load date of plugin.</param>
        private static void GetPluginInfo(string fileName, ref string name, ref string description, ref DateTime loadTime, ref string executionInterval)
        {
            // Initialize the plugin's config filename string.

            string pluginsPath = ServiceStartPath + "\\" + ConfigurationManager.AppSettings["PluginsFolderName"];
            string pluginConfigFile = pluginsPath + "\\" + fileName + ".config";
            
            XmlTextReader oReader = null;

            try
            {
                oReader = new XmlTextReader(pluginConfigFile);

                // This block will read the plugin's config file through a XmlTextReader
                // and save information to the respective local variables.
                while (oReader.Read())
                {
                    if (oReader.Name == "add")
                    {
                        //ToDo: use name instead of index in GetAttribute
                        if (oReader.GetAttribute(0).ToLower() == "name")
                        {
                            name = oReader.GetAttribute(1);
                        }

                        if (oReader.GetAttribute(0).ToLower() == "description")
                        {
                            description = oReader.GetAttribute(1);
                        }

                        if (oReader.GetAttribute(0).ToLower() == "initialloadtime")
                        {
                            DateTime.TryParse(oReader.GetAttribute(1), out loadTime);
                        }

                        if (oReader.GetAttribute(0).ToLower() == "executioninterval")
                        {
                            executionInterval = oReader.GetAttribute(1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Write the exception message to event logs.
                EventLog log = new EventLog("Application");
                log.Source = "MFPluggerService";
                log.WriteEntry(ex.Message);
            }
            finally
            {
                // Close the XmlTextReader.
                if (oReader != null)
                {
                    oReader.Close();
                }
            }
        }

        /// <summary>
        /// Description:
        ///     This will check whether the plugin is registered of not.
        /// </summary>
        /// <param name="fileName">Assembly name of plugin.</param>
        /// <returns>Plugin is registered or not.</returns>
        private static bool IsPluginRegistered(string fileName)
        {
            bool isRegistered = false;

            XmlTextReader oReader = null;

            // This block will check whether the specified file registered to PluginsInfo.xml or not.
            try
            {
                oReader = new XmlTextReader(ServiceStartPath  + "\\" + ConfigurationManager.AppSettings["PluginInfoFileName"]);

                while (oReader.Read())
                {
                    if (oReader.Name == "Plugin")
                    {
                        if (oReader.GetAttribute("FileName") == fileName)
                        {
                            isRegistered = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Write the exception message to event logs.
                EventLog log = new EventLog("Application");
                log.Source = "MFPluggerService";
                log.WriteEntry(ex.Message);
            }
            finally
            {
                // Close the XmlTextReader.
                if (oReader != null)
                {
                    oReader.Close();
                }
            }

            // If not registered then return false.
            return isRegistered;
        }
    }
}
