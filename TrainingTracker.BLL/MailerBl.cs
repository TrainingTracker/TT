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
    public class MailerBl : BusinessBase
    {
        public bool AddNewFeedbackMail(Notification notification, User addedFor, int feedbackId)
        {
            User addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);
            EmailContent emailContent = GetFeedbackEmailContent(notification
                                                      , addedByUser
                                                      , notification.Title);

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

            Feedback feedback = UnitOfWork.FeedbackRepository.Get(feedbackId);

            EmailContent emailContent = GetFeedbackEmailContent(notification
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
                        EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
                    });
                }
            }


            foreach (var thread in feedback.FeedbackThreads.Where(user => user.AddedBy != addedByUser.UserId))
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

        public bool AddNewDiscussionMail(Notification notification)
        {
            User addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);
            List<EmailAlertSubscription> subscriptionList = UnitOfWork.EmailAlertSubscriptionRepository
                                                                      .GetAllSubscribedMentors(addedByUser.UserId)
                                                                      .Where(x=>x.SubscribedByUserId!=addedByUser.UserId)
                                                                      .ToList();

            if (!subscriptionList.Any()) return true; // escape the routine if no one is subscribed to this trainee.

            EmailContent emailContent = GetFeedbackEmailContent(notification, addedByUser, notification.Title);

            foreach (var user in subscriptionList)
            {
                emailContent.EmailRecipients.Add(new EmailRecipient
                {
                    EmailAddress = user.User.Email,
                    EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
                });
            }

            UnitOfWork.EmailRepository.Add(emailContent);
            return UnitOfWork.Commit() > 0;
        }

        public bool AddNewDiscussionThreadMail(Notification notification, int discussionPostId)
        {
            User addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);
            ForumDiscussionPost forumDiscussionPost = UnitOfWork.ForumDiscussionPostRepository.GetPostWithThreads(discussionPostId);
            List<EmailAlertSubscription> subscriptionList = UnitOfWork.EmailAlertSubscriptionRepository
                                                                      .GetAllSubscribedMentors(forumDiscussionPost.AddedBy)
                                                                      .Where(x => x.SubscribedByUserId != addedByUser.UserId)
                                                                      .ToList();

            EmailContent emailContent = GetFeedbackEmailContent(notification, addedByUser, notification.Title);

            IEnumerable<DAL.EntityFramework.User> allUsers = forumDiscussionPost.ForumDiscussionThreads.Where(x => x.AddedBy != addedByUser.UserId)
                                                                                                       .Select(x => x.User)
                                                                                                       .Union(subscriptionList.Select(x => x.User));

            if (addedByUser.UserId != forumDiscussionPost.AddedBy)
            {
                emailContent.EmailRecipients.Add(new EmailRecipient
                {
                    EmailAddress = forumDiscussionPost.User.Email,
                    EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                });

                foreach (DAL.EntityFramework.User user in allUsers)
                {
                    emailContent.EmailRecipients.Add(new EmailRecipient
                    {
                        EmailAddress = user.Email,
                        EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
                    });
                }
            }
            else
            {
                if (!allUsers.Any()) return true; // / escape the routine if no one is subscribed to this trainee.

                foreach (DAL.EntityFramework.User user in allUsers)
                {
                    emailContent.EmailRecipients.Add(new EmailRecipient
                    {
                        EmailAddress = user.Email,
                        EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                    });
                }
            }
            UnitOfWork.EmailRepository.Add(emailContent);
            return UnitOfWork.Commit() > 0;
        }

        public bool AddSessionMail(Notification notification, Common.Entity.Session session)
        {
            User addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);
            EmailContent emailContent = GetFeedbackEmailContent(notification
                                                     , addedByUser
                                                     , notification.Title);

            foreach (var user in UnitOfWork.SessionRepository.GetSessionWithAttendees(session.Id).UserSessionMappings)
            {
                emailContent.EmailRecipients.Add(new EmailRecipient
                             {
                                 EmailAddress = user.User1.Email,
                                 EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                             });
            }

            emailContent.EmailRecipients.Add(new EmailRecipient
            {
                EmailAddress = addedByUser.Email,
                EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.CarbonCopy
            });

            UnitOfWork.EmailRepository.Add(emailContent); //TODO:EmailRepository needs to be EmailContentRepository. 
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
