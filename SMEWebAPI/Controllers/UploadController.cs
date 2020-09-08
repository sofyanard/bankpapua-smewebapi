using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

/*
* reference:
* https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/sending-html-form-data-part-2
* https://www.c-sharpcorner.com/UploadFile/2b481f/uploading-a-file-in-Asp-Net-web-api/
*/

namespace SMEWebAPI.Controllers
{
    public class UploadController : ApiController
    {
        [Route("api/upload")]
        public async Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string responsemessage = "";
            string filename = "";
            string localfilepath = "";
            string filepath = "";
            string uploadpath = ConfigurationManager.AppSettings["UploadPath"];
            string filenamewithoutextension = "";
            string fileextension = "";
            int fileindex = 0;
            string indexedfilename = "";

            // string root = HttpContext.Current.Server.MapPath("~/App_Data");
            string root = uploadpath;
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    filename = file.Headers.ContentDisposition.FileName;
                    filename = filename.Trim('"'); // remove double quotes
                    // kalau lewat Edge, dapetnya full path C:\\Users\\sofyanard\\Documents\\SOFYAN.txt
                    // kalau lewat Chrome dapetnya file name 

                    localfilepath = file.LocalFileName;
                    // Server file path: D:\\SOURCE\\BANKPAPUA\\SMEWebAPI\\SMEWebAPI\\App_Data\\BodyPart_c9b044f4-03b0-42b1-b8a6-42e814fe86c6

                    filepath = Path.Combine(root, filename);

                    // Rename if file exists
                    fileindex = 0;
                    while (File.Exists(filepath))
                    {
                        filenamewithoutextension = Path.GetFileNameWithoutExtension(filename);
                        fileextension = Path.GetExtension(filepath);
                        fileindex += 1;
                        indexedfilename = filenamewithoutextension + "(" + fileindex.ToString() + ")" + fileextension;
                        filepath = Path.Combine(root, indexedfilename);
                    }

                    if (fileindex != 0)
                    {
                        filename = Path.GetFileName(filepath);
                    }
                    
                    File.Move(localfilepath, filepath);

                    responsemessage += filepath + " - ";
                }

                // Show all the key-value pairs.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        responsemessage += string.Format("{0}: {1}", key, val) + " - ";
                        // caption: sofyan test file
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, responsemessage);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
