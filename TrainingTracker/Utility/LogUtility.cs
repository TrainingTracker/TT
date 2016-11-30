/***************************************************************************
    Author          :   Harsh Wardhan   
	Created on      :   4th March, 2016
    Company Name    :   Mindfire Solutions
 **************************************************************************/

using System.Globalization;
using System.IO;
using System.Net;
using System;

namespace TrainingTracker.Utility
{
    /// <summary>
    /// Utility class for error logging.
    /// </summary>
    public class LogUtility
    {
        /// <summary>
        /// Constant for log folder name.
        /// </summary>
        private const string ERROR_LOG_FOLDER = "ErrorLogs";
        /// <summary>
        /// Error log file name.
        /// </summary>
        private const string ERROR_LOG_FILE = "NorthScopeLogFile_{0}.txt";
        /// <summary>
        /// Date format constant.
        /// </summary>
        private const string DATE_FORMAT = "ddMMyyyy";
        /// <summary>
        /// Log file path.
        /// </summary>
        public static string StrLogFilePath = string.Empty;
        /// <summary>
        /// Stream writer object.
        /// </summary>
        private static StreamWriter sw;

        /// <summary>
        /// Setting LogFile path. If the logfile path is 
        /// null then it will update error info into LogFile.txt under
        /// application directory.
        /// </summary>
        public string LogFilePath
        {
            set
            {
                StrLogFilePath = value;
            }
            get
            {
                return StrLogFilePath;
            }
        }

        /// <summary>
        /// Write error log entry for window event if the bLogType is true.
        /// Otherwise, write the log entry to
        /// customized text-based text file
        /// </summary>
        /// <param name="objException"></param>
        /// <returns>false if the problem persists</returns>
        public static bool ErrorRoutine(Exception objException)
        {
            string strPathName = string.Format(ERROR_LOG_FILE, DateTime.Now.ToString(DATE_FORMAT));

            if (StrLogFilePath.Equals(string.Empty))
            {
                //Get Default log file path.
                strPathName = GetLogFilePath();
            }
            else
            {
                strPathName = Path.Combine(StrLogFilePath, strPathName);
            }

            return WriteErrorLog(strPathName, objException);
        }

        /// <summary>
        /// Write Source,method,date,time,computer,error 
        /// and stack trace information to the text file
        /// <param name="strPathName"></param>
        /// <param name="objException"></param>
        /// <RETURNS>false if the problem persists</RETURNS>
        ///</summary>
        private static bool WriteErrorLog(string strPathName,
                                        Exception objException)
        {
            try
            {
                sw = new StreamWriter(strPathName, true);
                sw.WriteLine("Source        : " +
                        objException.Source.Trim());
                sw.WriteLine("Method        : " +
                        objException.TargetSite.Name);
                sw.WriteLine("Date        : " +
                        DateTime.Now.ToLongTimeString());
                sw.WriteLine("Time        : " +
                        DateTime.Now.ToShortDateString());
                sw.WriteLine("Computer    : " +
                        Dns.GetHostName().ToString(CultureInfo.InvariantCulture));
                sw.WriteLine("Error        : " +
                        objException.Message.ToString(CultureInfo.InvariantCulture).Trim());
                sw.WriteLine("Stack Trace    : " +
                        objException.StackTrace.Trim());
                sw.WriteLine("^^-----------------------------------------------------^^");
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Check the log file in applcation directory.
        /// If it is not available, creae it
        /// <RETURNS>Log file path</RETURNS>
        ///</summary>
        private static string GetLogFilePath()
        {
            try
            {
                // get the base directory
                var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ERROR_LOG_FOLDER);

                if (!Directory.Exists(baseDir))
                    Directory.CreateDirectory(baseDir);

                // search the file below the current directory
                var retFilePath = Path.Combine(baseDir, string.Format(ERROR_LOG_FILE, DateTime.Now.ToString(DATE_FORMAT)));

                // if exists, return the path
                if (File.Exists(retFilePath))
                    return retFilePath;

                //create a text file
                var fs = new FileStream(retFilePath,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Close();

                return retFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
