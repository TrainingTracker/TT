using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MFPluggerService;
using TrainingTracker.TaskScheduler.DataAccess;

namespace TrainingTracker.TaskScheduler
{
    public class TaskScheduler : IMFServicePlugin
    {
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
                Mailer.LogEvent(ex
                               , "TrainingTracker_ TaskScheduler"
                               , "TT_TaskScheduler");
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
                       Mailer.LogEvent(  ex
                                       , "TrainingTracker_ TaskScheduler"
                                       , String.Format("TT_TaskScheduler_Job_{0}",job.Description));

                    }
                }
            }
            catch (Exception ex)
            {
               Mailer. LogEvent(  ex
                                , "TrainingTracker_ TaskScheduler"
                                , "TT_TaskScheduler");
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

            Mailer.StartEmailRun(new SchedulerDataAccess().GetAllPendingEmailContentAndCorrespondingRecipientForJob(job,allowedFailedAttempts));
        }
    }



    public static class Constants
    {
        internal static AppSettingsSection MyDllConfigAppSettings;       
    }
}
