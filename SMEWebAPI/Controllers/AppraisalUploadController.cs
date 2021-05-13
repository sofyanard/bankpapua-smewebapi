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
    [RoutePrefix("api/AppraisalUpload")]
    public class AppraisalUploadController : ApiController
    {
        private LOSSME db = new LOSSME();

        // GET: api/AppraisalUpload/{apRegno}/{cuRef}/{clSeq}
        [Route("{apRegno}/{cuRef}/{clSeq}")]
        public IHttpActionResult GetAll(string apRegno, string cuRef, int clSeq)
        {
            List<AppraisalNewFileUpload> listAppraisalNewFileUpload = db.AppraisalNewFileUploads
                .Where(p => p.ApRegno == apRegno && p.CuRef == cuRef && p.ClSeq == clSeq)
                .ToList();

            if ((listAppraisalNewFileUpload == null) || (listAppraisalNewFileUpload.Count == 0))
            {
                return NotFound();
            }

            // string baseURL = Url.Content("~/");
            string downloaddir = Url.Content(ConfigurationManager.AppSettings["DownloadDir"]);

            List<AppraisalNewFileUploadView> listAppraisalNewFileUploadView = new List<AppraisalNewFileUploadView>();

            foreach (AppraisalNewFileUpload appraisalNewFileUpload in listAppraisalNewFileUpload)
            {
                AppraisalNewFileUploadView appraisalNewFileUploadView = new AppraisalNewFileUploadView(appraisalNewFileUpload);
                appraisalNewFileUploadView.DownloadUrl = downloaddir + appraisalNewFileUpload.FuFileName;
                listAppraisalNewFileUploadView.Add(appraisalNewFileUploadView);
            }

            return Ok(listAppraisalNewFileUploadView);
        }

        // POST: api/AppraisalUpload/{apRegno}/{cuRef}/{clSeq}
        [Route("{apRegno}/{cuRef}/{clSeq}")]
        public async Task<HttpResponseMessage> PostFormData(string apRegno, string cuRef, int clSeq)
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

                // Get Max Seq from Table AppraisalNewFileUpload
                AppraisalNewFileUpload appraisalNewFileUpload = db.AppraisalNewFileUploads
                    .Where(p => p.ApRegno == apRegno && p.CuRef == cuRef && p.ClSeq == clSeq)
                    .OrderByDescending(p => p.FuSeq)
                    .FirstOrDefault();
                int nextFuSeq = 1;
                if (appraisalNewFileUpload != null)
                {
                    nextFuSeq = appraisalNewFileUpload.FuSeq + 1;
                }

                // Get User from Table ScUser
                string email = User.Identity.GetUserName();
                ScUser scUser = db.ScUsers.Where(p => p.Email == email).FirstOrDefault();
                string userId = "";
                if (scUser != null)
                {
                    userId = scUser.UserId;
                }

                // Insert to Table AppraisalNewFileUpload
                AppraisalNewFileUpload newAppraisalNewFileUpload = new AppraisalNewFileUpload();
                newAppraisalNewFileUpload.ApRegno = apRegno;
                newAppraisalNewFileUpload.CuRef = cuRef;
                newAppraisalNewFileUpload.ClSeq = clSeq;
                newAppraisalNewFileUpload.FuSeq = nextFuSeq;
                newAppraisalNewFileUpload.FuFileName = filename;
                newAppraisalNewFileUpload.FuDate = DateTime.Now;
                newAppraisalNewFileUpload.FuUserId = userId;

                db.AppraisalNewFileUploads.Add(newAppraisalNewFileUpload);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newAppraisalNewFileUpload);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE: api/AppraisalUpload/{apRegno}/{cuRef}/{clSeq}/{fuSeq}
        [Route("{apRegno}/{cuRef}/{clSeq}/{fuSeq}")]
        public IHttpActionResult Delete(string apRegno, string cuRef, int clSeq, int fuSeq)
        {
            AppraisalNewFileUpload appraisalNewFileUpload = db.AppraisalNewFileUploads
                .Where(p => p.ApRegno == apRegno && p.CuRef == cuRef && p.ClSeq == clSeq && p.FuSeq == fuSeq).FirstOrDefault();
            
            if (appraisalNewFileUpload == null)
            {
                return NotFound();
            }

            db.AppraisalNewFileUploads.Remove(appraisalNewFileUpload);
            db.SaveChanges();

            // Delete File
            string root = ConfigurationManager.AppSettings["UploadPath"];
            string filepath = Path.Combine(root, appraisalNewFileUpload.FuFileName);
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

            return Ok(appraisalNewFileUpload);
        }
    }
}
