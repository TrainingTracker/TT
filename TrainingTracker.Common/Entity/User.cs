using System;

namespace TrainingTracker.Common.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string ProfilePictureName { get; set; }
        public bool IsFemale { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsTrainer { get; set; }
        public bool IsTrainee { get; set; }
        public bool IsManager { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateAddedToSystem { get; set; }
        public bool IsValid { get; set; }
        public int UserRating { get; set; }

        public int? TeamId { get; set; }
    }

}