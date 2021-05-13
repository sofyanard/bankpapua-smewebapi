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

        [Route("Product")]
        public IHttpActionResult GetProduct()
        {
            var result = db.RfProducts
                .Where(p => p.IsSubAppProd == "1" && p.Active == "1")
                .Select(p => new { p.ProductId, p.ProductDesc })
                .ToList();
            return Ok(result);
        }

        [Route("LoanPurpose")]
        public IHttpActionResult GetLoanPurpose()
        {
            var result = db.RfLoanPurposes
                .Where(p => p.Active == "1")
                .Select(p => new { p.LoanPurpId, p.LoanPurpDesc })
                .ToList();
            return Ok(result);
        }

        [Route("CollateralType")]
        public IHttpActionResult GetCollateralType()
        {
            var result = db.RfCollateralTypes
                .Where(p => p.Active == "1")
                .Select(p => new { p.ColTypeSeq, p.ColTypeDesc })
                .ToList();
            return Ok(result);
        }

        [Route("CertificateTypeAll")]
        public IHttpActionResult GetAllCertificateType()
        {
            var result = db.RfCertTypes
                .Where(p => p.Active == "1")
                .Select(p => new { p.CertTypeId, p.CertTypeDesc })
                .ToList();
            return Ok(result);
        }

        [Route("CertificateTypeByColType/{ColTypeSeq}")]
        public IHttpActionResult GetCertificateTypeByColType(int colTypeSeq)
        {
            RfCollateralType rfCollateralType = db.RfCollateralTypes.Find(colTypeSeq);

            var result = db.RfCertTypes
                .Where(p => p.ColFlag == rfCollateralType.ColLinkTable && p.Active == "1")
                .Select(p => new { p.CertTypeId, p.CertTypeDesc })
                .ToList();
            return Ok(result);
        }

        [Route("Area")]
        public IHttpActionResult GetArea()
        {
            var result = db.RfAreas
                .Where(p => p.Active == "1")
                .Select(p => new { p.AreaId, p.AreaName })
                .ToList();
            return Ok(result);
        }

        [Route("CityAll")]
        public IHttpActionResult GetAllCity()
        {
            var result = db.RfCities
                .Where(p => p.Active == "1")
                .Select(p => new { p.CityId, p.CityName })
                .ToList();
            return Ok(result);
        }

        [Route("CityByArea/{AreaId}")]
        public IHttpActionResult GetCityByArea(string areaId)
        {
            var result = db.RfCities
                .Where(p => p.AreaId == areaId && p.Active == "1")
                .Select(p => new { p.CityId, p.CityName })
                .ToList();
            return Ok(result);
        }

        [Route("BranchAll")]
        public IHttpActionResult GetBranchByCity()
        {
            var result = db.RfBranches
                .Where(p => p.Active == "1" && p.IsBranch == "1")
                .Select(p => new { p.BranchCode, p.BranchName })
                .ToList();
            return Ok(result);
        }

        [Route("BranchByCity/{CityId}")]
        public IHttpActionResult GetAllBranch(string cityId)
        {
            var result = db.RfBranches
                .Where(p => p.CityId == cityId && p.Active == "1" && p.IsBranch == "1")
                .Select(p => new { p.BranchCode, p.BranchName })
                .ToList();
            return Ok(result);
        }

        [Route("ApprMarketability")]
        public IHttpActionResult GetApprMarketability()
        {
            return Ok(db.RfApprMarketabilities);
        }

        [Route("ApprIkatSempurna")]
        public IHttpActionResult GetApprIkatSempurna()
        {
            return Ok(db.RfApprIkatSempurnas);
        }

        [Route("ApprKuasa")]
        public IHttpActionResult GetApprKuasa()
        {
            return Ok(db.RfApprKuasas);
        }

        [Route("ApprMasalah")]
        public IHttpActionResult GetApprMasalah()
        {
            return Ok(db.RfApprMasalahs);
        }
    }
}
