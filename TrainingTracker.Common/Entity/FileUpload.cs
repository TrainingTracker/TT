using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TrainingTracker.Common.Entity
{
   public class FileUpload
    {
       public int Id { get; set; }

       public HttpPostedFile FormData { get; set; }
    }
}
