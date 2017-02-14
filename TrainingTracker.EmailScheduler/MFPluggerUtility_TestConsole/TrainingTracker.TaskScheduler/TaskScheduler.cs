using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MFPluggerService;
using TrainingTracker.TaskScheduler.DataAccess;

namespace TrainingTracker.TaskScheduler
{
    public class TaskScheduler : IMFServicePlugin
    {
        /// <summary>
        /// Entry Class for the Plugin, Implements IMFServicePlugin
        /// </summary>
        public TaskScheduler()
        {
            try
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Assembly.GetExecutingAssembly().Location + ".config"
                };

                Configuration libConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                Constants.MyDllConfigAppSettings = (libConfig.GetSection("appSettings") as AppSettingsSection);
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// This is Entry Point to Training Tracker Task scheduler
        /// </summary>
        /// <exception> No exception should propagate to top, else it may cause Service and its other dependencies to failure</exception>
        public void ExecutePlugin()
        {
            try
            {
                SchedulerDataAccess dataAccess = new SchedulerDataAccess();


                foreach (TaskSchedulerJob job in dataAccess.GetAllActiveScheduledJob())
                {
                    try
                    {
                        // Execute individual job
                        ExecuteTrainingTrackerJob(job);

                        // Update the Execution time fotr the Job
                        dataAccess.UpdateJobExecutionStatus(job.Id);
                    }
                    catch (Exception ex)
                    {
                        Constants.WriteEventLog(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());
            }
        }

        /// <summary>
        /// Private method to complete operation for each job
        /// </summary>
        /// <param name="job">Instance of Job</param>
        /// <exception>All Exception should be Managed by the called mailer , If any Exception in between let it propagate to top</exception>
        private void ExecuteTrainingTrackerJob(TaskSchedulerJob job)
        {

            int allowedFailedAttempts;

            Int32.TryParse(Constants.MyDllConfigAppSettings.Settings["AllowedFailedAttempts"].Value ?? 0.ToString(), out allowedFailedAttempts);

            Mailer.StartEmailRun(new SchedulerDataAccess().GetAllPendingEmailContentAndCorrespondingRecipientForJob(job, allowedFailedAttempts));
        }
    }



    public static class Constants
    {
        internal static AppSettingsSection MyDllConfigAppSettings;

        /// <summary>
        /// This function writes the mail sending details to the log file
        /// </summary>
        public static void WriteEventLog(String logText)
        {
            try
            {

                //set the path of the log file that is to be written 
                string pluginsPath = ServiceStartPath.Substring(0, ServiceStartPath.LastIndexOf(@"\", System.StringComparison.Ordinal)) + "\\"
                                     + ConfigurationManager.AppSettings["PluginsFolderName"];

                //create a log file inside the pluginsPath folder
                FileStream oFileStream = new FileStream(pluginsPath + "\\TrainingTrackerTaskScheduler.log", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writeLog = new StreamWriter(oFileStream);
                writeLog.BaseStream.Seek(0, SeekOrigin.End);

                //write the date value into the log file
                writeLog.WriteLine(DateTime.Now + " - ");
                writeLog.WriteLine(logText);
                writeLog.WriteLine();

                writeLog.Flush(); //Clear the memory used by StreamWriter.
                writeLog.Close(); //Close the StreamWriter object.
            }
            catch (Exception ex)
            {
                // Write the exception message to event logs.
                EventLog log = new EventLog("Application") {Source = "MFPluggerServiceV2"};
                log.WriteEntry("RecurrentGoalAdditionServicePlugin: " + ex.Message);
            }
        }

        /// <summary>
        /// This property gives the windows service startup path
        /// </summary>
        private static string ServiceStartPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            }
        }
    }
}
