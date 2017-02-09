
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
                Mailer.LogEvent(  ex
                                , "TrainingTracker_ TaskScheduler"
                                , String.Format("TT_TaskScheduler_Job_{0}","DataAcess_GetAllActiveScheduledJob"));

                return  new List<TaskSchedulerJob>();
            }
        }

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
                Mailer.LogEvent(ex
                               , "TrainingTracker_ TaskScheduler"
                               , String.Format("TT_TaskScheduler_Job_{0}", "DataAcess_UpdateJobExecutionStatus"));

                return false;                
            }
            
        }

        #endregion

        #region Email

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
                Mailer.LogEvent(ex
                             , "TrainingTracker_ TaskScheduler"
                             , String.Format("TT_TaskScheduler_Job_{0}", "DataAcess_GetAllPendingEmailContentAndCorrespondingRecipientForJob"));

                return new List<EmailContent>();
            }
        }

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
                Mailer.LogEvent(ex
                             , "TrainingTracker_ TaskScheduler"
                             , String.Format("TT_TaskScheduler_Job_{0}", "DataAcess_UpdateEmailStatus"));
            }
        }

        #endregion
    }
}
