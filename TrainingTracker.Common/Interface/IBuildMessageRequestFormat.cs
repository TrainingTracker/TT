using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace TrainingTracker.Common.Utility
{
    public interface IBuildMessageRequestFormat
    {
        string BuildMessageRequestFormat(HttpRequestMessage requestMessage);
    }
}

