using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;

namespace TrainingTracker.Common.Utility
{
    public class HMACMessageRequestFormatBuilder : DelegatingHandler
    {
        private string ApiId = "4d53bce03ec34c0a911182d4c228ee6c";
        private string ApiKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            string requestContentBase64String = string.Empty;

            string requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = request.Method.Method;

            //timestamp
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //creating nonce
            string nonce = Guid.NewGuid().ToString("N");

            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }
            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", ApiId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);
            var secretKeyByteArray = Convert.FromBase64String(ApiKey);
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("amx", string.Format("{0}:{1}:{2}:{3}", ApiId, requestSignatureBase64String, nonce, requestTimeStamp));
            }
            response = await base.SendAsync(request, cancellationToken);
            return response;              
        }
    }
}

