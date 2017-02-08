using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.DAL.EntityFramework;
using Notification = TrainingTracker.Common.Entity.Notification;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.BLL
{
    public class MailerBl:BussinessBase
    {
       
       public bool AddNewFeedbackMail(Notification notification,User addedFor,int feedbackId)
       {
           // This need to be changed... but How!! 
           var addedByUser = UserDataAccesor.GetUserById(notification.AddedBy);

           Dictionary<string,string> replacements = new Dictionary<string,string>
           {
              {"[[[DomainName]]]", Constants.AppDomainUrl},
              {"[[[NotificationTitle]]]", notification.Title + "for " + addedFor.FirstName},
              {"[[[NotificationBy]]]",addedByUser.FirstName},
              {"[[[NotificationByImagePath]]]", Constants.AppDomainUrl + "/Uploads/ProfilePicture/"  + addedByUser.ProfilePictureName},
              {"[[[NotificationRedirectURL]]]",Constants.AppDomainUrl + "/"  + notification.Link}
           };
         
          string body = Common.Utility.UtilityFunctions.SubstituteTemplateWithReplacements( Common.Utility.UtilityFunctions.FetchEmailTemplateFromPath(EmailTemplatesPath.FeedbackTemplate)
                                                                                           , replacements);

           EmailContent emailContent  = new EmailContent
           {
               BodyText = body,
               Attempts = 0,
               IsRichBody = true,
               SubjectText = notification.Title,
               TaskSchedulerJobId = 2,
               IsSent = false
           };

           emailContent.EmailRecipients.Add( new EmailRecipient
           {
               EmailAddress = addedFor.Email,
               EmailRecipientType = (int)  Common.Enumeration.EmailRecipientType.To
           });

           List<EmailAlertSubscription> listSubscription = new List<EmailAlertSubscription>();

           listSubscription.AddRange(UnitOfWork.EmailAlertSubscriptionRepository.GetAllSubscribedMentors(addedFor.UserId)
                                                                           .Where(user => user.SubscribedByUserId != addedByUser.UserId));

           foreach (var user in listSubscription)
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

       public bool AddNewFeedbackThreadMail(Notification notification, int feedbackId)
       {
           // This need to be changed... but How!! 
          
           var feedback = UnitOfWork.FeedbackRepository.Get(feedbackId);

           Dictionary<string, string> replacements = new Dictionary<string, string>
           {
              {"[[[DomainName]]]", Constants.AppDomainUrl},
              {"[[[NotificationTitle]]]", notification.Title },
              {"[[[NotificationBy]]]",feedback.User1.FirstName},
              {"[[[NotificationByImagePath]]]", Constants.AppDomainUrl + "/Uploads/ProfilePicture/"  + feedback.User1.ProfilePictureName},
              {"[[[NotificationRedirectURL]]]",Constants.AppDomainUrl + "/"  + notification.Link}
           };

           string body = Common.Utility.UtilityFunctions.SubstituteTemplateWithReplacements(Common.Utility.UtilityFunctions.FetchEmailTemplateFromPath(EmailTemplatesPath.FeedbackTemplate)
                                                                                            , replacements);

            EmailContent emailContent = new EmailContent
           {
               BodyText = body,
               Attempts = 0,
               IsRichBody = true,
               SubjectText = notification.Title,
               TaskSchedulerJobId = 2,
               IsSent = false
           };

             if (notification.AddedBy == feedback.User.UserId)
               {
                   emailContent.EmailRecipients.Add(new EmailRecipient
                   {
                       EmailAddress = feedback.User1.Email,
                       EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                   });
               }
               
               else if (notification.AddedBy == feedback.User1.UserId)
               {
                    emailContent.EmailRecipients.Add(new EmailRecipient
                   {
                       EmailAddress = feedback.User.Email,
                       EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
                   });
               }
           

           
           
          

           emailContent.EmailRecipients.Add(new EmailRecipient
           {
               EmailAddress = addedFor.Email,
               EmailRecipientType = (int)Common.Enumeration.EmailRecipientType.To
           });

           List<EmailAlertSubscription> listSubscription = new List<EmailAlertSubscription>();

           //listSubscription.AddRange(UnitOfWork.EmailAlertSubscriptionRepository.GetAllSubscribedMentors(addedFor.UserId)
           //                                                                .Where(user => user.SubscribedByUserId != addedByUser.UserId));

           foreach (var user in listSubscription)
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
    }
}
