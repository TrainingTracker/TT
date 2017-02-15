using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using TrainingTracker.TaskScheduler.DataAccess;

namespace TrainingTracker.TaskScheduler
{
    public static class Mailer
    {
        

        /// <summary>
        /// Method that use parallelism to trigger mail 
        /// </summary>
        /// <param name="data"></param>
        internal static void StartEmailRun(IEnumerable<EmailContent> data)
        {
            try
            {
                ParallelOptions parallelOptions = new ParallelOptions();

                // Manage parallelism instead of CLR doing so.
                parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount * 2;

                Parallel.ForEach(data, parallelOptions, dataObject =>
                {
                    MailMessage msg = new MailMessage
                        {
                            Subject = dataObject.SubjectText,
                            Body = dataObject.BodyText,
                            IsBodyHtml = dataObject.IsRichBody,
                            Priority = MailPriority.Normal,

                            // If both goes null, let it crash
                            From = new MailAddress(dataObject.FromAddress ?? Constants.MyDllConfigAppSettings.Settings["FromAddress"].Value
                                                  , Constants.MyDllConfigAppSettings.Settings["FromName"].Value)
                        };

                    try
                    {

                        AddMessageRecipient(dataObject.EmailRecipients, ref msg);
                        SendEmail(msg);

                        dataObject.IsSent = true;    
                    }
                    catch (Exception ex)
                    {
                        Constants.WriteEventLog(ex.ToString());

                        // This Mail has failed update attempts
                        dataObject.IsSent = false;                       
                    }
                    finally
                    {
                        msg.Dispose();
                        new SchedulerDataAccess().UpdateEmailStatus(dataObject);
                    }
                });
            }
            catch (Exception ex)
            {
                Constants.WriteEventLog(ex.ToString());
                // This job has failed. But we can't propagate the exception above
                // Need to supress this
            }
        }

        /// <summary>
        ///  method to add recipients to the mail
        /// </summary>
        /// <param name="emailRecipients">Listof email recipients</param>
        /// <param name="mail">Instance of mail message</param>
        private static void AddMessageRecipient(IEnumerable<EmailRecipient> emailRecipients, ref MailMessage mail)
        {

            foreach (var recipients in emailRecipients)
            {
                switch (recipients.EmailRecipientType)
                {
                    case (int)EmailRecipientType.To:
                        mail.To.Add(new MailAddress(recipients.EmailAddress));
                        break;

                    case (int)EmailRecipientType.CarbonCopy:
                        mail.CC.Add(new MailAddress(recipients.EmailAddress));
                        break;

                    case (int)EmailRecipientType.BlindCarbonCopy:
                        mail.Bcc.Add(new MailAddress(recipients.EmailAddress));
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Private to class method to Send Emails
        /// </summary>
        /// <param name="mail">mailMessage instance loaded with  all data to be fired in mail</param>
        /// <exception >This method dont habdle exception, Let it propagate upward.</exception>
        private static void SendEmail(MailMessage mail)
        {
            try
            {
                // Declares an object for SMTP client for sending the mail

                string serviceLocation = Constants.MyDllConfigAppSettings.Settings["ServiceLocation"].Value ??
                                         "TestServer";

                SmtpClient smtp = new SmtpClient("email.mindfiresolutions.com");

                if (!serviceLocation.Equals("Live", StringComparison.InvariantCultureIgnoreCase))
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation =
                        Constants.MyDllConfigAppSettings.Settings["PickupDirectoryLocation"].Value ?? "D:\\00TTEmails\\";
                }
                else
                {
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("trainingtracker@mindfiresolutions.com","mfmail@2016#");
                }
               
                smtp.Send(mail);
            }           
            finally
            {
                // Do this to make sure CPU usages doesn't spike up to 100%
                Thread.Sleep(10);
                mail.Dispose();
            }
        }
    }
}
