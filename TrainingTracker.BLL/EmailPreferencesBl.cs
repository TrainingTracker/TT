using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.ModelMapper;
using EfEntity = TrainingTracker.DAL.EntityFramework;
using CommonEntity = TrainingTracker.Common.Entity;


namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness class for Email Alert Preferences
    /// </summary>
    public class EmailPreferencesBl : BussinessBase
    {

        public bool SetEmailPreferences(List<EmailAlertSubscription> emailSubscriptions, User currentUser)
        {

            foreach (var emailSubscriptionEntity in emailSubscriptions.Where(item => item.IsModifiedOrAdded))
            {
                if (emailSubscriptionEntity.Id == 0)
                {
                    emailSubscriptionEntity.Id = UnitOfWork.EmailAlertSubscriptionRepository
                        .GetId(emailSubscriptionEntity.SubscribedByUserId, emailSubscriptionEntity.SubscribedForUserId);
                }

                UnitOfWork.EmailAlertSubscriptionRepository.AddOrUpdate(
                        EmailAlertSubscriptionConverter.ConvertToCore(emailSubscriptionEntity));  
            }

            return UnitOfWork.Commit() > 0;
        }

        public List<EmailAlertSubscription> GetUserSubscriptionsById(int subscribedByUserId)
        {
            return UnitOfWork.EmailAlertSubscriptionRepository.Find(x => x.SubscribedByUserId == subscribedByUserId && !x.IsDeleted)
                    .Select(entity => EmailAlertSubscriptionConverter.ConvertFromCore(entity)).ToList();
        }
    }
}