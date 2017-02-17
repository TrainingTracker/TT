using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [AllowHtml] 
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the Password 
        /// </summary>
        [Required]
        [AllowHtml] 
        public string Password { get; set; }

    }
}
