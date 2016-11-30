using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Model class for login page
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Get and Sets UserName
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the Password 
        /// </summary>
        [Required]
        public string Password { get; set; }

    }
}
