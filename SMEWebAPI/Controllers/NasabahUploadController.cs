using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using SMEWebAPI;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/NasabahUpload")]
    public class NasabahUploadController : ApiController
    {
        private LOSSME db = new LOSSME();

        // GET: api/NasabahUpload
        public IHttpActionResult GetNasabahUploadsByBasabahId()
        {
            int nasabahId = this.GetNasabahId();

            List<NasabahUpload> nasabahUploads = db.NasabahUploads.Where(p => p.NasabahId == nasabahId).ToList();

            if ((nasabahUploads == null) || (nasabahUploads.Count == 0))
            {
                return NotFound();
            }

            string baseURL = Url.Content("~/");
            string downloaddir = ConfigurationManager.AppSettings["DownloadDir"];

            List<NasabahUploadView> nasabahUploadViews = new List<NasabahUploadView>();

            foreach (NasabahUpload nasabahUpload in nasabahUploads)
            {
                NasabahUploadView nasabahUploadView = new NasabahUploadView(nasabahUpload);
                nasabahUploadView.DownloadUrl = baseURL + downloaddir + nasabahUploadView.FileName;
                nasabahUploadViews.Add(nasabahUploadView);
            }

            return Ok(nasabahUploadViews);
        }

        // POST: api/NasabahUpload
        public async Task<HttpResponseMessage> PostFormData()
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
            string caption = "";

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

                // Loop for Other Form Datas
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key == "caption")
                        {
                            caption = val;
                        }
                    }
                }

                // Insert NasabahUpload Entity
                NasabahUpload nasabahUpload = new NasabahUpload();
                nasabahUpload.NasabahId = this.GetNasabahId();
                nasabahUpload.FileName = filename;
                nasabahUpload.Caption = caption;
                db.NasabahUploads.Add(nasabahUpload);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, nasabahUpload);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE: api/NasabahUpload/5
        [ResponseType(typeof(NasabahUpload))]
        public IHttpActionResult DeleteNasabahUpload(int id)
        {
            NasabahUpload nasabahUpload = db.NasabahUploads.Find(id);
            if (nasabahUpload == null)
            {
                return NotFound();
            }

            db.NasabahUploads.Remove(nasabahUpload);
            db.SaveChanges();

            // Delete File
            string root = ConfigurationManager.AppSettings["UploadPath"];
            string filepath = Path.Combine(root, nasabahUpload.FileName);
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

            return Ok(nasabahUpload);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NasabahUploadExists(int id)
        {
            return db.NasabahUploads.Count(e => e.Id == id) > 0;
        }



        private int GetNasabahId()
        {
            string username = User.Identity.GetUserName();

            Nasabah nasabah = db.Nasabahs.Where(p => p.UserName == username).FirstOrDefault();

            if (nasabah == null)
            {
                return 0;
            }

            return nasabah.Id;
        }
    }
}