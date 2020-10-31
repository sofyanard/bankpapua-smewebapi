using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using SMEWebAPI;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    public class PengajuanController : ApiController
    {
        private LOSSME db = new LOSSME();

        // GET: api/Pengajuan
        public IHttpActionResult GetPengajuans()
        {
            int nasabahId = this.GetNasabahId();

            List<Pengajuan> pengajuans = db.Pengajuans.Where(p => p.NasabahId == nasabahId).ToList();

            if ((pengajuans == null) || (pengajuans.Count == 0))
            {
                return NotFound();
            }

            return Ok(pengajuans);
        }

        // GET: api/Pengajuan/5
        [ResponseType(typeof(Pengajuan))]
        public IHttpActionResult GetPengajuan(int id)
        {
            Pengajuan pengajuan = db.Pengajuans.Find(id);
            if (pengajuan == null)
            {
                return NotFound();
            }

            return Ok(pengajuan);
        }

        // PUT: api/Pengajuan/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPengajuan(int id, Pengajuan pengajuan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pengajuan.Id)
            {
                return BadRequest();
            }

            db.Entry(pengajuan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PengajuanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pengajuan
        [ResponseType(typeof(Pengajuan))]
        public IHttpActionResult PostPengajuan(Pengajuan pengajuan)
        {
            pengajuan.NasabahId = this.GetNasabahId();
            pengajuan.PengajuanDate = DateTime.Now;
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pengajuans.Add(pengajuan);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pengajuan.Id }, pengajuan);
        }

        // DELETE: api/Pengajuan/5
        /*
        [ResponseType(typeof(Pengajuan))]
        public IHttpActionResult DeletePengajuan(int id)
        {
            Pengajuan pengajuan = db.Pengajuans.Find(id);
            if (pengajuan == null)
            {
                return NotFound();
            }

            db.Pengajuans.Remove(pengajuan);
            db.SaveChanges();

            return Ok(pengajuan);
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PengajuanExists(int id)
        {
            return db.Pengajuans.Count(e => e.Id == id) > 0;
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