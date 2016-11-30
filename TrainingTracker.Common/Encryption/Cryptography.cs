using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TrainingTracker.Common.Encryption
{
    /// <summary>
    /// Static Class contains encryption and decryption method to attain cryptography
    /// </summary>
    public static class Cryptography
    {
        #region EncryptMessage
        /// <summary>
        /// Method which does the encryption using Rijndeal's algorithm
        /// </summary>
        /// <param name="stringToEncrypt">string to be encrypted</param>
        /// <returns>Encrypted Data to Encrypt</returns>
        public static string Encrypt( string stringToEncrypt )
        {
            if (string.IsNullOrWhiteSpace(stringToEncrypt))
            {
                return string.Empty;
            }

            RijndaelManaged objRijndaelCipher = new RijndaelManaged();
            MemoryStream objMemoryStream = new MemoryStream();

            // fetch the encryption key from constant
            const string strEncryptionKey = Constants.Constants.SalesTrackerEncryptionKey;

            try
            {
                byte[] bytePlainText = Encoding.Unicode.GetBytes(stringToEncrypt);
                byte[] byteCipherSalt = Encoding.ASCII.GetBytes(strEncryptionKey.Length
                                                                                .ToString(CultureInfo.InvariantCulture));

                // This class uses an extension of the PBKDF1 algorithm defined in the PKCS#5 v2.0 
                // standard to derive bytes suitable for use as key material from a password. 
                // The standard is documented in IETF RRC 2898.
                PasswordDeriveBytes objSecretKey = new PasswordDeriveBytes(strEncryptionKey , byteCipherSalt);

                // Creates a symmetric encryptor object. 
                ICryptoTransform objEncryptor = objRijndaelCipher.CreateEncryptor(objSecretKey.GetBytes(32) ,
                                                                                  objSecretKey.GetBytes(16));


                // Defines a stream that links data streams to cryptographic transformations
                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream ,
                                                                objEncryptor ,
                                                                CryptoStreamMode.Write);

                objCryptoStream.Write(bytePlainText , 0 , bytePlainText.Length);

                // Writes the final state and clears the buffer
                objCryptoStream.FlushFinalBlock();
                byte[] objCipherBytes = objMemoryStream.ToArray();

                // close the datastream to cryptographic transformations
                objCryptoStream.Close();

                string strEncryptedData = Convert.ToBase64String(objCipherBytes);
                return strEncryptedData;
            }
            catch (Exception ex)
            {
                // exception Logger
                // failed to encrypt
                return string.Empty;
            }
            finally
            {
                // Close stream backing to memory
                objMemoryStream.Close();
            }
        }
        #endregion

        #region DecryptMessage
        /// <summary>
        /// Method which does the decryption using Rijndeal's algorithm
        /// </summary>
        /// <param name="strEncryptedMessage">string to decrypt</param>
        /// <returns>returns decrypted string</returns>
        public static string Decrypt( string strEncryptedMessage )
        {
            if (string.IsNullOrWhiteSpace(strEncryptedMessage))
            {
                return string.Empty;
            }

            RijndaelManaged objRijndaelCipher = new RijndaelManaged();

            // fetch the encryption key
            const string strEncryptionKey = Constants.Constants.SalesTrackerEncryptionKey;

            try
            {
                byte[] byteEncryptedData = Convert.FromBase64String(strEncryptedMessage);
                byte[] bytePlainText = new byte[byteEncryptedData.Length];
                byte[] byteCipherSalt = Encoding.ASCII.GetBytes(strEncryptionKey.Length
                                                                                .ToString());

                // Making of the key for decryption
                PasswordDeriveBytes objSecretKey = new PasswordDeriveBytes(strEncryptionKey , byteCipherSalt);

                // Creates a symmetric Rijndael decryptor object.
                ICryptoTransform objDecryptor = objRijndaelCipher.CreateDecryptor(objSecretKey.GetBytes(32) ,
                                                                                  objSecretKey.GetBytes(16));

                MemoryStream objMemoryStream = new MemoryStream(byteEncryptedData);

                // Defines the cryptographics stream for decryption.THe stream contains decrpted data
                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream ,
                    objDecryptor ,
                    CryptoStreamMode.Read);

                int intDecryptedCount = objCryptoStream.Read(bytePlainText , 0 , bytePlainText.Length);
                objMemoryStream.Close();
                objCryptoStream.Close();

                return Encoding.Unicode.GetString(bytePlainText , 0 , intDecryptedCount);
            }
            catch (Exception ex)
            {
                // Implement logger later here

                return string.Empty;
            }
        }
        #endregion
    }
}
