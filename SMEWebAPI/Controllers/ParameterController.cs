using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [RoutePrefix("api/Parameter")]
    public class ParameterController : ApiController
    {
        private LOSSME db = new LOSSME();

        [Route("Propinsi")]
        public IHttpActionResult GetPropinsi()
        {
            return Ok(db.RefPropinsis);
        }

        [Route("KotaKabupaten/{PropinsiId}")]
        public IHttpActionResult GetKotaKabByPropinsiId(string PropinsiId)
        {
            var result = db.RefKotaKabs.Where(p => p.PropId == PropinsiId).Select(p => new { p.Id, p.Name }).ToList();
            return Ok(result);
        }

        [Route("Kecamatan/{KotaKabId}")]
        public IHttpActionResult GetKecamatanByKotaKabId(string KotaKabId)
        {
            var result = db.RefKecamatans.Where(p => p.KotaId == KotaKabId).Select(p => new { p.Id, p.Name }).ToList();
            return Ok(result);
        }

        [Route("Kelurahan/{KecamatanId}")]
        public IHttpActionResult GetKelurahanByKecamatanId(string KecamatanId)
        {
            var result = db.RefKelurahans.Where(p => p.KecId == KecamatanId).Select(p => new { p.Id, p.Name }).ToList();
            return Ok(result);
        }

        [Route("Sex")]
        public IHttpActionResult GetSex()
        {
            return Ok(db.RfSexs);
        }

        [Route("Education")]
        public IHttpActionResult GetEducation()
        {
            return Ok(db.RfEducations);
        }

        [Route("Marital")]
        public IHttpActionResult GetMarital()
        {
            return Ok(db.RfMaritals);
        }

        [Route("Citizenship")]
        public IHttpActionResult GetCitizenship()
        {
            return Ok(db.RfCitizenships);
        }

        [Route("HomeStatus")]
        public IHttpActionResult GetHomeStatus()
        {
            return Ok(db.RfHomeStatuses);
        }

        [Route("JobTitle")]
        public IHttpActionResult GetJobTitle()
        {
            return Ok(db.RfJobTitles);
        }

        [Route("Relationship")]
        public IHttpActionResult GetRelationship()
        {
            return Ok(db.RfRelationships);
        }
    }
}
