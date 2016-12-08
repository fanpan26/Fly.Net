using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fly.Util.FileUpload
{
    public sealed class FileUploadHelper
    {
        public static object Upload(HttpPostedFileBase file, string fullPath)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    string fileName = Guid.NewGuid().ToString();
                    string fullFileName = string.Format("{0}{1}", fileName, fileExtension);
                    string oldFileName = Path.GetFileName(file.FileName);

                    string fileSavePath = string.Format("{0}{1}", fullPath, fullFileName);
                    string url = "/upload/" + fullFileName;
                    file.SaveAs(fileSavePath);

                    object fileObj = new { src = url, name = oldFileName };
                    return fileObj;
                }
                return null;
            }
            catch (Exception ex)
            {
                //记录日志
                return null;
            }
        }
    }
}
