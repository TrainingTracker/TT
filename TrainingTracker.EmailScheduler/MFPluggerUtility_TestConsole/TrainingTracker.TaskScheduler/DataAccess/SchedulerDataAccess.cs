
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainingTracker.TaskScheduler.DataAccess
{
    /// <summary>
    /// This Class implemnts all methods neccesary  to Fetch/Modify Jobs and pending emails
    /// </summary>
    public class SchedulerDataAccess
    {
        #region Jobs

        /// <summary>
        /// Fetches All Active schedulers jobs that are ready for Execution
        /// </summary>
        /// <returns>List of All Active schedulers</returns>
        /// <exception >On Exception returns Empty List</exception>
        internal List<TaskSchedulerJob> GetAllActiveScheduledJob()
        {
            try
            {
                using (TrainingTrackerContainer context = new TrainingTrackerContainer())
                {
                    var jobs = context.TaskSchedulerJobs.Where(x => x.IsActive)    
                                                        .ToList();

                    return jobs.Where(x=> !x.LastExecution.HasValue 
                                        || x.LastExecution.Value.AddSeconds(x.ExecutionIntervalSeconds) <= DateTime.Now)
                               .ToList();
                }
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());

                return  new List<TaskSchedulerJob>();
            }
        }

        /// <summary>
        /// Updates the Job Execution Status 
        /// </summary>
        /// <param name="jobId">Id  of the job to Be updated</param>
        /// <returns>Success Status of the method</returns>
        /// <exception> On Exception returns False</exception>
        internal bool UpdateJobExecutionStatus(int jobId)
        {
            try
            {
                using (TrainingTrackerContainer context = new TrainingTrackerContainer())
                {
                    // Intentional Use of Find.. There should be single job with the id
                    context.TaskSchedulerJobs.Find(jobId).LastExecution = DateTime.Now;
                    return  context.SaveChanges() == 1;
                }
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());

                return false;                
            }
            
        }

        #endregion

        #region Email

        /// <summary>
        /// Fetch All Emails pending for the Given Job Id
        /// </summary>
        /// <param name="job">Instance of current Executing Job</param>
        /// <param name="allowedFailedAttempts">Config Driven allowed Failed Count for Emails</param>
        /// <returns>List Of Emails Need to be Triggered</returns>
        /// <exception>Returns Empty List </exception>
        internal IEnumerable<EmailContent> GetAllPendingEmailContentAndCorrespondingRecipientForJob(TaskSchedulerJob job, int allowedFailedAttempts)
        {
            try
            {
                using (TrainingTrackerContainer context = new TrainingTrackerContainer())
                {
                    return context.EmailContents.Include("EmailRecipients")
                                                .Where(x=>x.TaskSchedulerJobId == job.Id && !x.IsSent && x.Attempts <= allowedFailedAttempts)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());
                return new List<EmailContent>();
            }
        }

        /// <summary>
        /// Method To update the Email Status , Sets 1 to Is sent on success Sets 0 on Failure.
        /// </summary>
        /// <param name="email">Instance Of Email which needs updation</param>
        internal void UpdateEmailStatus(EmailContent email)
        {
            try
            {
                using (TrainingTrackerContainer context = new TrainingTrackerContainer())
                {
                    // intentional use of single to let it fail if multiple email with same id.
                    EmailContent emailContent = context.EmailContents.Single(x => x.Id == email.Id);
                    emailContent.IsSent = email.IsSent;
                    emailContent.SentTimeStamp = DateTime.Now;
                    ++emailContent.Attempts;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());
            }
        }

        #endregion
    }
}
