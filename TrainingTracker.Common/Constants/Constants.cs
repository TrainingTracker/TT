
namespace TrainingTracker.Common.Constants
{
    /// <summary>
    /// Class contains application level constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// SalesTracker Common Encryption Key
        /// </summary>
        public const string SalesTrackerEncryptionKey = "!#$a54?3";

        public const string DefaultCourseIcon = "DefaultCourse.jpg";
    }
    /// <summary>
    /// Contains all user roles.
    /// </summary>
    public static class UserRoles
    {
        /// <summary>
        /// User Administrator role.
        /// </summary>
        public const string Administrator = "Administrator";
        /// <summary>
        /// User Manager role.
        /// </summary>
        public const string Manager = "Manager";
        /// <summary>
        /// User Trainee role.
        /// </summary>
        public const string Trainee = "Trainee";
        /// <summary>
        /// User Trainer role.
        /// </summary>
        public const string Trainer = "Trainer";
    }

    /// <summary>
    /// Contains Session Assets Virtual paths
    /// </summary>
    public static class SessionAssets
    {
        /// <summary>
        /// Session's Virtual path For Slides
        /// </summary>
        public const string SlidePath = "~/Uploads/SessionSlide/";

        /// <summary>
        /// Session's Virtual path For Videos
        /// </summary>
        public const string VideoPath = "~/Uploads/SessionVideo/";
    }

    /// <summary>
    /// Contains App used Extensions
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Extension for Mp4
        /// </summary>
        public const string Mp4 = ".mp4";

        /// <summary>
        /// Extensions for Ppt
        /// </summary>
        public const string Ppt = ".ppt";
    }

    /// <summary>
    /// Contains Learning Map/Path Assets Virtual paths
    /// </summary>
    public static class LearningAssetsPath
    {
        /// <summary>
        /// Virtual path for Course Icons
        /// </summary>
        public const string CourseIcon = "~/Uploads/CourseIcon/";

        /// <summary>
        /// Virtual path for Assignment documents
        /// </summary>
        public const string Assignment = "~/Uploads/Assignment/";
    }
}
