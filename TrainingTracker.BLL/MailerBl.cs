using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using Notification = TrainingTracker.Common.Entity.Notification;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.BLL
{
    public class MailerBl : BussinessBase
    {
        public bool AddNewFeedbackMail(Notification notification, User addedFor, int feedbackId)
        {
            var addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);
            var emailContent = GetFeedbackEmailContent(notification
                                                      ,addedByUser
                                                      ,notification.Title + " for " + addedFor.FirstName);

            emailContent.EmailRecipients.Add(new EmailRecipient
            {
                EmailAddress = addedFor.Email,
                EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
            });

           
            foreach (var user in UnitOfWork.EmailAlertSubscriptionRepository.GetAllSubscribedMentors(addedFor.UserId)
                                                                            .Where(user => user.SubscribedByUserId != addedByUser.UserId))
            {
                emailContent.EmailRecipients.Add(new EmailRecipient
                {
                    EmailAddress = user.User.Email,
                    EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
                });
            }

            UnitOfWork.EmailRepository.Add(emailContent); //TODO:EmailRepository needs to be EmailContentRepository. 
            return UnitOfWork.Commit() > 0;
        }

        public bool AddNewFeedbackThreadMail(Notification notification, int feedbackId)
        {
            // This need to be changed... but How!! 
            var addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);

            var feedback = UnitOfWork.FeedbackRepository.Get(feedbackId);

            var emailContent = GetFeedbackEmailContent(notification
                                                     , addedByUser
                                                     , notification.Title);


            if (notification.AddedBy == feedback.User1.UserId)
            {
                emailContent.EmailRecipients.Add(new EmailRecipient
                {
                    EmailAddress = feedback.User.Email,
                    EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                });
            }
            else
            {
                emailContent.EmailRecipients.Add(new EmailRecipient
                {
                    EmailAddress = feedback.User1.Email,
                    EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                });

                if (notification.AddedBy != feedback.User.UserId)
                {
                    emailContent.EmailRecipients.Add(new EmailRecipient
                    {
                        EmailAddress = feedback.User.Email,
                        EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
                    });
                }
            }

            foreach (var user in UnitOfWork.EmailAlertSubscriptionRepository.GetAllSubscribedMentors(feedback.User1.UserId)
                                                                            .Where(user => user.SubscribedByUserId != addedByUser.UserId))
            {
                if (emailContent.EmailRecipients.All(x => x.EmailAddress != user.User.Email))
                {
                    emailContent.EmailRecipients.Add(new EmailRecipient
                    {
                        EmailAddress = user.User.Email,
                        EmailRecipientType = (int) Common.Enumeration.EmailRecipientType.CarbonCopy
                    });
                }
            }


            foreach (var thread in feedback.FeedbackThreads.Where(user=>user.AddedBy != addedByUser.UserId))
            {
                if (emailContent.EmailRecipients.All(x => x.EmailAddress != thread.User.Email))
                {
                    emailContent.EmailRecipients.Add(new EmailRecipient
                    {
                        EmailAddress = thread.User.Email,
                        EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
                    });
                }
            }

            UnitOfWork.EmailRepository.Add(emailContent);
            return UnitOfWork.Commit() > 0;
        }

        private EmailContent GetFeedbackEmailContent(Notification notification, User addedBy, string title)
        {
           return new EmailContent
            {
                BodyText = GenerateFeedbackMailBody(notification, addedBy, title),
                Attempts = 0,
                IsRichBody = true,
                SubjectText = notification.Title,
                TaskSchedulerJobId = 2,
                IsSent = false
            };
        }

        private string GenerateFeedbackMailBody(Notification notification, User addedBy, string title)
        {
            var templateData = new Dictionary<string, string>
            {
               {NotificationEmailTemplateItems.DomainName, Constants.AppDomainUrl},
               {NotificationEmailTemplateItems.NotificationTitle, title},
               {NotificationEmailTemplateItems.NotificationBy, addedBy.FirstName},
               {NotificationEmailTemplateItems.NotificationByImagePath, Constants.AppDomainUrl + "/Uploads/ProfilePicture/"  + addedBy.ProfilePictureName},
               {NotificationEmailTemplateItems.NotificationRedirectUrl,Constants.AppDomainUrl + "/"  + notification.Link}
            };

            var template = new TemplateContentBuilder(UtilityFunctions.FetchEmailTemplateFromPath(EmailTemplatesPath.FeedbackTemplate).ToString());
            template.Fill(templateData);
            return template.GetText();
        }

    }
}
