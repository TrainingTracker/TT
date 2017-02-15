/******************************************************************************** 
 * File Name: MFDemoPlugin.cs
 * Company Name: mindfire solutions 
 * Author: abhishekb
 * Created On: 17SEP2009 
 * Description: This contains the implementation of IMFServicePlugin Interface and
 * additional functionality for host application
 * ******************************************************************************/

using System;
using System.IO;
using MFPluggerService;

namespace MFDemoPlugin
{
    public class MFDemoPlugin : IMFServicePlugin
    {
        #region IMFServicePlugin Members

        /// <summary>
        /// Description:
        ///     This method acts as a entry point for this Demo Plugin.
        /// </summary>
        void IMFServicePlugin.ExecutePlugin()
        {
            WriteFile(@"D:\MFDemoPlugin.txt", DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss tt") );
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Description:
        ///     This method will write the specified file.
        /// </summary>
        /// <param name="fileName">File name with path of the file.</param>
        /// <param name="text">Text to write the file.</param>
        private void WriteFile(string fileName, string text)
        {
            FileStream oFileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter oStreamWriter = new StreamWriter(oFileStream);
            oStreamWriter.BaseStream.Seek(0, SeekOrigin.End);
            oStreamWriter.WriteLine(text);
            oStreamWriter.Flush(); //Clear the memory used by StreamWriter.
            oStreamWriter.Close(); //Close the StreamWriter object.
        }

        #endregion
    }
}
