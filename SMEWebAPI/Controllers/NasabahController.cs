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
using Microsoft.VisualBasic.ApplicationServices;
using SMEWebAPI;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Nasabah")]
    public class NasabahController : ApiController
    {
        private LOSSME db = new LOSSME();

        // GET: api/Nasabah
        /*
        public IQueryable<Nasabah> GetNasabahs()
        {
            return db.Nasabahs;
        }
        */

        // GET: api/Nasabah/5
        [ResponseType(typeof(Nasabah))]
        public IHttpActionResult GetNasabah(int id)
        {
            Nasabah nasabah = db.Nasabahs.Find(id);
            if (nasabah == null)
            {
                return NotFound();
            }

            if (nasabah.UserName != User.Identity.GetUserName())
            {
                return Unauthorized();
            }

            return Ok(nasabah);
        }

        // PUT: api/Nasabah/5
        /*
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNasabah(int id, Nasabah nasabah)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nasabah.Id)
            {
                return BadRequest();
            }

            db.Entry(nasabah).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NasabahExists(id))
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
        */

        // POST: api/Nasabah
        [ResponseType(typeof(Nasabah))]
        public IHttpActionResult PostNasabah(Nasabah nasabah)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            nasabah.UserName = User.Identity.GetUserName();

            if (db.Nasabahs.Any(p => p.UserName == nasabah.UserName))
            {
                return BadRequest("Nasabah record is already exists");
            }

            // check if identity number is already exist
            if (db.Nasabahs.Any(p => p.NoIdentitas == nasabah.NoIdentitas))
            {
                return BadRequest("Nasabah identity number is already exists");
            }

            db.Nasabahs.Add(nasabah);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = nasabah.Id }, nasabah);
        }

        // DELETE: api/Nasabah/5
        /*
        [ResponseType(typeof(Nasabah))]
        public IHttpActionResult DeleteNasabah(int id)
        {
            Nasabah nasabah = db.Nasabahs.Find(id);
            if (nasabah == null)
            {
                return NotFound();
            }

            db.Nasabahs.Remove(nasabah);
            db.SaveChanges();

            return Ok(nasabah);
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

        private bool NasabahExists(int id)
        {
            return db.Nasabahs.Count(e => e.Id == id) > 0;
        }



        [Route("GetId")]
        public IHttpActionResult GetIdNasabahByUser()
        {
            string username = User.Identity.GetUserName();

            Nasabah nasabah = db.Nasabahs.Where(p => p.UserName == username).FirstOrDefault();
            if (nasabah == null)
            {
                return NotFound();
            }

            return Ok(nasabah.Id);
        }

        [Route("PutBasic/{Id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBasic(int id, [FromBody] Nasabah nasabah)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nasabah.Id)
            {
                return BadRequest();
            }

            // check if identity number is already exist
            if (db.Nasabahs.Any(p => p.NoIdentitas == nasabah.NoIdentitas && p.Id != nasabah.Id))
            {
                return BadRequest("Nasabah identity number is already exists");
            }

            db.Entry(nasabah).State = EntityState.Modified;

            //db.Entry(nasabah).Property("Id").IsModified = false;
            db.Entry(nasabah).Property("UserName").IsModified = false;
            db.Entry(nasabah).Property("GelarSebelum").IsModified = true;
            db.Entry(nasabah).Property("NamaLengkap").IsModified = true;
            db.Entry(nasabah).Property("GelarSesudah").IsModified = true;
            db.Entry(nasabah).Property("JenisKelamin").IsModified = true;
            db.Entry(nasabah).Property("TempatLahir").IsModified = true;
            db.Entry(nasabah).Property("TanggalLahir").IsModified = true;
            db.Entry(nasabah).Property("NoIdentitas").IsModified = true;
            db.Entry(nasabah).Property("AlamatRumah").IsModified = true;
            db.Entry(nasabah).Property("PropinsiRumah").IsModified = true;
            db.Entry(nasabah).Property("KotaKabRumah").IsModified = true;
            db.Entry(nasabah).Property("KecamatanRumah").IsModified = true;
            db.Entry(nasabah).Property("KelurahanRumah").IsModified = true;
            db.Entry(nasabah).Property("KodePosRumah").IsModified = true;
            db.Entry(nasabah).Property("TeleponRumah").IsModified = true;
            db.Entry(nasabah).Property("TeleponGenggam").IsModified = true;
            db.Entry(nasabah).Property("NamaIbuKandung").IsModified = true;
            db.Entry(nasabah).Property("Pendidikan").IsModified = true;
            db.Entry(nasabah).Property("StatusPerkawinan").IsModified = true;
            db.Entry(nasabah).Property("Kewarganegaraan").IsModified = true;
            db.Entry(nasabah).Property("StatusRumah").IsModified = true;
            db.Entry(nasabah).Property("JenisPekerjaan").IsModified = false;
            db.Entry(nasabah).Property("Pendapatan").IsModified = false;
            db.Entry(nasabah).Property("AlamatKantor").IsModified = false;
            db.Entry(nasabah).Property("TeleponKantor").IsModified = false;
            db.Entry(nasabah).Property("PropinsiKantor").IsModified = false;
            db.Entry(nasabah).Property("KotaKabKantor").IsModified = false;
            db.Entry(nasabah).Property("KodePosKantor").IsModified = false;
            db.Entry(nasabah).Property("NamaSaudara").IsModified = false;
            db.Entry(nasabah).Property("AlamatSaudara").IsModified = false;
            db.Entry(nasabah).Property("PropinsiSaudara").IsModified = false;
            db.Entry(nasabah).Property("KotaKabSaudara").IsModified = false;
            db.Entry(nasabah).Property("KodePosSaudara").IsModified = false;
            db.Entry(nasabah).Property("HubunganSaudara").IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NasabahExists(id))
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

        [Route("PutPekerjaan/{Id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPekerjaan(int id, [FromBody] NasabahPekerjaan nasabahPekerjaan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nasabahPekerjaan.Id)
            {
                return BadRequest();
            }

            Nasabah nasabah = db.Nasabahs.Find(id);

            if (nasabah == null)
            {
                return NotFound();
            }

            try
            {
                nasabah.JenisPekerjaan = nasabahPekerjaan.JenisPekerjaan;
                nasabah.Pendapatan = nasabahPekerjaan.Pendapatan;
                nasabah.AlamatKantor = nasabahPekerjaan.AlamatKantor;
                nasabah.PropinsiKantor = nasabahPekerjaan.PropinsiKantor;
                nasabah.KotaKabKantor = nasabahPekerjaan.KotaKabKantor;
                nasabah.KodePosKantor = nasabahPekerjaan.KodePosKantor;
                nasabah.TeleponKantor = nasabahPekerjaan.TeleponKantor;

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NasabahExists(id))
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

        [Route("PutSaudara/{Id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSaudara(int id, [FromBody] NasabahSaudara nasabahSaudara)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nasabahSaudara.Id)
            {
                return BadRequest();
            }

            Nasabah nasabah = db.Nasabahs.Find(id);

            if (nasabah == null)
            {
                return NotFound();
            }

            try
            {
                nasabah.NamaSaudara = nasabahSaudara.NamaSaudara;
                nasabah.AlamatSaudara = nasabahSaudara.AlamatSaudara;
                nasabah.PropinsiSaudara = nasabahSaudara.PropinsiSaudara;
                nasabah.KotaKabSaudara = nasabahSaudara.KotaKabSaudara;
                nasabah.KodePosSaudara = nasabahSaudara.KodePosSaudara;
                nasabah.HubunganSaudara = nasabahSaudara.HubunganSaudara;

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NasabahExists(id))
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
    }
}