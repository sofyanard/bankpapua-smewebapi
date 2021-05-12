using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/SiteVisitUpload")]
    public class SiteVisitUploadController : ApiController
    {
        private LOSSME db = new LOSSME();

        // GET: api/SiteVisitUpload
        [Route("{apRegno}")]
        public IHttpActionResult GetNasabahUploadsByBasabahId(string apRegno)
        {
            List<DocUploadFileUpload> listDocUploadFileUpload = db.DocUploadFileUploads
                .Where(p => p.ApRegno == apRegno && p.GroupFile == "SVUPLOAD")
                .OrderBy(p => p.Seq)
                .ToList();

            if ((listDocUploadFileUpload == null) || (listDocUploadFileUpload.Count == 0))
            {
                return NotFound();
            }

            // string baseURL = Url.Content("~/");
            string downloaddir = Url.Content(ConfigurationManager.AppSettings["DownloadDir"]);

            List<DocUploadFileUploadView> listDocUploadFileUploadView = new List<DocUploadFileUploadView>();

            foreach (DocUploadFileUpload docUploadFileUpload in listDocUploadFileUpload)
            {
                DocUploadFileUploadView docUploadFileUploadView = new DocUploadFileUploadView(docUploadFileUpload);
                docUploadFileUploadView.DownloadUrl = downloaddir + docUploadFileUpload.FuFileName;
                listDocUploadFileUploadView.Add(docUploadFileUploadView);
            }

            return Ok(listDocUploadFileUploadView);
        }

        // POST: api/SiteVisitUpload
        [Route("{apRegno}")]
        public async Task<HttpResponseMessage> PostFormData(string apRegno)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = ConfigurationManager.AppSettings["UploadPath"];
            var provider = new MultipartFormDataStreamProvider(root);

            string filename = "";
            string localfilepath = "";
            string filepath = "";
            int fileindex = 0;
            string filenamewithoutextension = "";
            string fileextension = "";
            string indexedfilename = "";

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // Loop for Files
                foreach (MultipartFileData file in provider.FileData)
                {
                    filename = file.Headers.ContentDisposition.FileName;
                    filename = filename.Trim('"'); // remove double quotes

                    localfilepath = file.LocalFileName;

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
                }

                // Get Max Seq from Table DocUploadFileUpload
                DocUploadFileUpload maxDocUploadFileUpload = db.DocUploadFileUploads
                    .Where(p => p.GroupFile == "SVUPLOAD")
                    .OrderByDescending(p => p.Seq)
                    .FirstOrDefault();
                int nextSeq = 1;
                if (maxDocUploadFileUpload != null)
                {
                    nextSeq = maxDocUploadFileUpload.Seq + 1;
                }

                // Get User from Table ScUser
                string email = User.Identity.GetUserName();
                ScUser scUser = db.ScUsers.Where(p => p.Email == email).FirstOrDefault();
                string userId = "";
                if (scUser != null)
                {
                    userId = scUser.UserId;
                }

                // Insert to Table DocUploadFileUpload
                DocUploadFileUpload docUploadFileUpload = new DocUploadFileUpload();
                docUploadFileUpload.ApRegno = apRegno;
                docUploadFileUpload.GroupFile = "SVUPLOAD";
                docUploadFileUpload.Seq = nextSeq;
                docUploadFileUpload.FuFileName = filename;
                docUploadFileUpload.FuDate = DateTime.Now;
                docUploadFileUpload.FuUserId = userId;

                db.DocUploadFileUploads.Add(docUploadFileUpload);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, docUploadFileUpload);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE: api/SiteVisitUpload/5/1
        [Route("{apRegno}/{seq}")]
        public IHttpActionResult DeleteNasabahUpload(string apRegno, int seq)
        {
            DocUploadFileUpload docUploadFileUpload = db.DocUploadFileUploads
                .Where(p => p.ApRegno == apRegno && p.GroupFile == "SVUPLOAD" && p.Seq == seq).FirstOrDefault();
            if (docUploadFileUpload == null)
            {
                return NotFound();
            }

            db.DocUploadFileUploads.Remove(docUploadFileUpload);
            db.SaveChanges();

            // Delete File
            string root = ConfigurationManager.AppSettings["UploadPath"];
            string filepath = Path.Combine(root, docUploadFileUpload.FuFileName);
            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(docUploadFileUpload);
        }
    }
}
