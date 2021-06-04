using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DMS.CuBESCore;
using DMS.DBConnection;
using SMEWebAPI.Models;
using System.Web.Http.Description;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Asuransi")]
    public class AsuransiController : ApiController
    {
        private Connection conn;
        private LOSSME db = new LOSSME();

        public AsuransiController()
        {
            conn = new Connection(ConfigurationManager.AppSettings["ConnSME"]);
        }

        // GET: api/Asuransi/Jiwa
        [Route("Jiwa")]
        public IHttpActionResult GetPipelineJiwa()
        {
            try
            {
                string email = User.Identity.GetUserName();
                string companyId;

                conn.QueryString = "SELECT IC_ID FROM RFINSURANCECOMPANY WHERE IC_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                if (conn.GetRowCount() > 0)
                {
                    companyId = conn.GetFieldValue("IC_ID");
                }
                else
                {
                    return NotFound();
                }

                conn.QueryString = "select " +
                    // Key
                    "i.AP_REGNO, i.SEQ, " +
                    // Application Data
                    "case c.CU_CUSTTYPEID when '02' then c1.CU_FIRSTNAME + isnull(' ' + c1.CU_MIDDLENAME, '') + isnull(' ' + c1.CU_LASTNAME, '') " +
                    "when '01' then c3.COMPTYPEDESC + ' ' + c2.CU_COMPNAME end as CU_NAME, " +
                    "u.SU_FULLNAME, " +
                    // Insurance Data
                    "t.IT_DESC as INSURANCE_TYPE " +
                    "from APPLIFEINSURANCE i " +
                    "join RFINSURANCETYPE t on i.IT_ID = t.IT_ID " +
                    "join APPLICATION a on i.AP_REGNO = a.AP_REGNO " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join SCUSER u on a.AP_RELMNGR = u.USERID " +
                    "where i.IC_ID = '" + companyId + "'";
                conn.ExecuteQuery();

                DataTable dtResult = conn.GetDataTable().Copy();

                if ((dtResult == null) || (dtResult.Rows.Count == 0))
                {
                    return NotFound();
                }

                List<AsuransiJiwaPipeline> listAsuransiJiwaPipeline = new List<AsuransiJiwaPipeline>();

                foreach (DataRow row in dtResult.Rows)
                {
                    if ((row != null) && (row.ItemArray != null))
                    {
                        AsuransiJiwaPipeline asuransiJiwaPipeline = new AsuransiJiwaPipeline();

                        asuransiJiwaPipeline.ApRegno = row["AP_REGNO"].ToString();
                        asuransiJiwaPipeline.Seq = int.Parse(row["SEQ"].ToString());
                        asuransiJiwaPipeline.CustomerName = row["CU_NAME"].ToString();
                        asuransiJiwaPipeline.OfficerName = row["SU_FULLNAME"].ToString();
                        asuransiJiwaPipeline.InsuranceType = row["INSURANCE_TYPE"].ToString();

                        listAsuransiJiwaPipeline.Add(asuransiJiwaPipeline);
                    }
                }

                return Ok(listAsuransiJiwaPipeline);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Asuransi/Kredit
        [Route("Kredit")]
        public IHttpActionResult GetPipelineKredit()
        {
            try
            {
                string email = User.Identity.GetUserName();
                string companyId;

                conn.QueryString = "SELECT IC_ID FROM RFINSURANCECOMPANY WHERE IC_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                if (conn.GetRowCount() > 0)
                {
                    companyId = conn.GetFieldValue("IC_ID");
                }
                else
                {
                    return NotFound();
                }

                conn.QueryString = "select " +
                    // Key
                    "i.AP_REGNO, i.APPTYPE, i.PRODUCTID, i.PROD_SEQ, i.SEQ, " +
                    // Application Data
                    "case c.CU_CUSTTYPEID when '02' then c1.CU_FIRSTNAME + isnull(' ' + c1.CU_MIDDLENAME, '') + isnull(' ' + c1.CU_LASTNAME, '') " +
                    "when '01' then c3.COMPTYPEDESC + ' ' + c2.CU_COMPNAME end as CU_NAME, " +
                    "u.SU_FULLNAME, " +
                    // Insurance Data
                    "t.IT_DESC as INSURANCE_TYPE " +
                    "from APPCREDASURANCE i " +
                    "join RFINSURANCETYPE t on i.IT_ID = t.IT_ID " +
                    "join APPLICATION a on i.AP_REGNO = a.AP_REGNO " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join SCUSER u on a.AP_RELMNGR = u.USERID " +
                    "where i.IC_ID = '" + companyId + "'";
                conn.ExecuteQuery();

                DataTable dtResult = conn.GetDataTable().Copy();

                if ((dtResult == null) || (dtResult.Rows.Count == 0))
                {
                    return NotFound();
                }

                List<AsuransiKreditPipeline> listAsuransiKreditPipeline = new List<AsuransiKreditPipeline>();

                foreach (DataRow row in dtResult.Rows)
                {
                    if ((row != null) && (row.ItemArray != null))
                    {
                        AsuransiKreditPipeline asuransiKreditPipeline = new AsuransiKreditPipeline();

                        asuransiKreditPipeline.ApRegno = row["AP_REGNO"].ToString();
                        asuransiKreditPipeline.AppType = row["APPTYPE"].ToString();
                        asuransiKreditPipeline.ProductId = row["PRODUCTID"].ToString();
                        asuransiKreditPipeline.ProductSeq = int.Parse(row["PROD_SEQ"].ToString());
                        asuransiKreditPipeline.Seq = int.Parse(row["SEQ"].ToString());
                        asuransiKreditPipeline.CustomerName = row["CU_NAME"].ToString();
                        asuransiKreditPipeline.OfficerName = row["SU_FULLNAME"].ToString();
                        asuransiKreditPipeline.InsuranceType = row["INSURANCE_TYPE"].ToString();

                        listAsuransiKreditPipeline.Add(asuransiKreditPipeline);
                    }
                }

                return Ok(listAsuransiKreditPipeline);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Asuransi/Agunan
        [Route("Agunan")]
        public IHttpActionResult GetPipelineAgunan()
        {
            try
            {
                string email = User.Identity.GetUserName();
                string companyId;

                conn.QueryString = "SELECT IC_ID FROM RFINSURANCECOMPANY WHERE IC_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                if (conn.GetRowCount() > 0)
                {
                    companyId = conn.GetFieldValue("IC_ID");
                }
                else
                {
                    return NotFound();
                }

                conn.QueryString = "select " +
                    // Key
                    "i.AP_REGNO, i.CU_REF, i.PRODUCTID, i.CL_SEQ, i.SEQ, " +
                    // Application Data
                    "case c.CU_CUSTTYPEID when '02' then c1.CU_FIRSTNAME + isnull(' ' + c1.CU_MIDDLENAME, '') + isnull(' ' + c1.CU_LASTNAME, '') " +
                    "when '01' then c3.COMPTYPEDESC + ' ' + c2.CU_COMPNAME end as CU_NAME, " +
                    "u.SU_FULLNAME, " +
                    // Insurance Data
                    "t.IT_DESC as INSURANCE_TYPE " +
                    "from APPCOLASURANCE i " +
                    "join RFINSURANCETYPE t on i.IT_ID = t.IT_ID " +
                    "join APPLICATION a on i.AP_REGNO = a.AP_REGNO " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join SCUSER u on a.AP_RELMNGR = u.USERID " +
                    "where i.IC_ID = '" + companyId + "'";
                conn.ExecuteQuery();

                DataTable dtResult = conn.GetDataTable().Copy();

                if ((dtResult == null) || (dtResult.Rows.Count == 0))
                {
                    return NotFound();
                }

                List<AsuransiAgunanPipeline> listAsuransiAgunanPipeline = new List<AsuransiAgunanPipeline>();

                foreach (DataRow row in dtResult.Rows)
                {
                    if ((row != null) && (row.ItemArray != null))
                    {
                        AsuransiAgunanPipeline asuransiAgunanPipeline = new AsuransiAgunanPipeline();

                        asuransiAgunanPipeline.ApRegno = row["AP_REGNO"].ToString();
                        asuransiAgunanPipeline.CuRef = row["CU_REF"].ToString();
                        asuransiAgunanPipeline.ProductId = row["PRODUCTID"].ToString();
                        asuransiAgunanPipeline.CollateralSeq = int.Parse(row["CL_SEQ"].ToString());
                        asuransiAgunanPipeline.Seq = int.Parse(row["SEQ"].ToString());
                        asuransiAgunanPipeline.CustomerName = row["CU_NAME"].ToString();
                        asuransiAgunanPipeline.OfficerName = row["SU_FULLNAME"].ToString();
                        asuransiAgunanPipeline.InsuranceType = row["INSURANCE_TYPE"].ToString();

                        listAsuransiAgunanPipeline.Add(asuransiAgunanPipeline);
                    }
                }

                return Ok(listAsuransiAgunanPipeline);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Asuransi/Jiwa/5/4
        [Route("Jiwa/{apRegno}/{seq}")]
        [ResponseType(typeof(AsuransiJiwaMobile))]
        public IHttpActionResult GetOneJiwa(string apRegno, int seq)
        {
            try
            {
                AppLifeInsurance appLifeInsurance = db.AppLifeInsurances
                    .Where(p => p.ApRegno == apRegno && p.Seq == seq)
                    .FirstOrDefault();

                if (appLifeInsurance == null)
                {
                    return NotFound();
                }

                AsuransiJiwaMobile asuransiJiwaMobile = new AsuransiJiwaMobile();
                asuransiJiwaMobile.ApRegno = appLifeInsurance.ApRegno;
                asuransiJiwaMobile.Seq = appLifeInsurance.Seq;
                asuransiJiwaMobile.InsuranceCompanyId = appLifeInsurance.InsuranceCompanyId;
                asuransiJiwaMobile.InsuranceTypeId = appLifeInsurance.InsuranceTypeId;
                asuransiJiwaMobile.InsuranceAmount = appLifeInsurance.InsuranceAmount;
                asuransiJiwaMobile.CurrencyId = appLifeInsurance.CurrencyId;
                asuransiJiwaMobile.Percentage = appLifeInsurance.Percentage;
                asuransiJiwaMobile.Premi = appLifeInsurance.Premi;
                asuransiJiwaMobile.DateStart = appLifeInsurance.DateStart;
                asuransiJiwaMobile.DateEnd = appLifeInsurance.DateEnd;
                asuransiJiwaMobile.PolicyNo = appLifeInsurance.PolicyNo;

                return Ok(asuransiJiwaMobile);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Asuransi/Kredit/5/4/3/2/1
        [Route("Kredit/{apRegno}/{appType}/{productId}/{productSeq}/{seq}")]
        [ResponseType(typeof(AsuransiKreditMobile))]
        public IHttpActionResult GetOneKredit(string apRegno, string appType, string productId, int productSeq, int seq)
        {
            try
            {
                AppCredAsurance appCredAsurance = db.AppCredAsurances
                    .Where(p => p.ApRegno == apRegno && p.AppType == appType && p.ProductId == productId && p.ProductSeq == productSeq && p.Seq == seq)
                    .FirstOrDefault();

                if (appCredAsurance == null)
                {
                    return NotFound();
                }

                AsuransiKreditMobile asuransiKreditMobile = new AsuransiKreditMobile();
                asuransiKreditMobile.ApRegno = appCredAsurance.ApRegno;
                asuransiKreditMobile.CuRef = appCredAsurance.CuRef;
                asuransiKreditMobile.AppType = appCredAsurance.AppType;
                asuransiKreditMobile.ProductId = appCredAsurance.ProductId;
                asuransiKreditMobile.ProductSeq = appCredAsurance.ProductSeq;
                asuransiKreditMobile.Seq = appCredAsurance.Seq;
                asuransiKreditMobile.InsuranceCompanyId = appCredAsurance.InsuranceCompanyId;
                asuransiKreditMobile.InsuranceTypeId = appCredAsurance.InsuranceTypeId;
                asuransiKreditMobile.InsuranceAmount = appCredAsurance.InsuranceAmount;
                asuransiKreditMobile.CurrencyId = appCredAsurance.CurrencyId;
                asuransiKreditMobile.Percentage = appCredAsurance.Percentage;
                asuransiKreditMobile.Premi = appCredAsurance.Premi;
                asuransiKreditMobile.DateStart = appCredAsurance.DateStart;
                asuransiKreditMobile.DateEnd = appCredAsurance.DateEnd;
                asuransiKreditMobile.PolicyNo = appCredAsurance.PolicyNo;

                return Ok(asuransiKreditMobile);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Asuransi/Agunan/5/4/3/2
        [Route("Agunan/{apRegno}/{productId}/{collateralSeq}/{seq}")]
        [ResponseType(typeof(AsuransiAgunanMobile))]
        public IHttpActionResult GetOneAgunan(string apRegno, string productId, int collateralSeq, int seq)
        {
            try
            {
                AppColAsurance appColAsurance = db.AppColAsurances
                    .Where(p => p.ApRegno == apRegno && p.ProductId == productId && p.CollateralSeq == collateralSeq && p.Seq == seq)
                    .FirstOrDefault();

                if (appColAsurance == null)
                {
                    return NotFound();
                }

                AsuransiAgunanMobile asuransiAgunanMobile = new AsuransiAgunanMobile();
                asuransiAgunanMobile.ApRegno = appColAsurance.ApRegno;
                asuransiAgunanMobile.CuRef = appColAsurance.CuRef;
                asuransiAgunanMobile.ProductId = appColAsurance.ProductId;
                asuransiAgunanMobile.CollateralSeq = appColAsurance.CollateralSeq;
                asuransiAgunanMobile.Seq = appColAsurance.Seq;
                asuransiAgunanMobile.InsuranceCompanyId = appColAsurance.InsuranceCompanyId;
                asuransiAgunanMobile.InsuranceTypeId = appColAsurance.InsuranceTypeId;
                asuransiAgunanMobile.InsuranceAmount = appColAsurance.InsuranceAmount;
                asuransiAgunanMobile.CurrencyId = appColAsurance.CurrencyId;
                asuransiAgunanMobile.Percentage = appColAsurance.Percentage;
                asuransiAgunanMobile.Premi = appColAsurance.Premi;
                asuransiAgunanMobile.DateStart = appColAsurance.DateStart;
                asuransiAgunanMobile.DateEnd = appColAsurance.DateEnd;
                asuransiAgunanMobile.PolicyNo = appColAsurance.PolicyNo;

                return Ok(asuransiAgunanMobile);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Asuransi/Jiwa/5
        [Route("Jiwa/{apRegno}/{seq}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJiwa(string apRegno, int seq, [FromBody] AsuransiJiwaMobile input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                AppLifeInsurance appLifeInsurance = db.AppLifeInsurances
                    .Where(p => p.ApRegno == input.ApRegno && p.Seq == input.Seq)
                    .FirstOrDefault();

                if (appLifeInsurance == null)
                {
                    return BadRequest("No record found!");
                }

                // appLifeInsurance.InsuranceCompanyId = input.InsuranceCompanyId;
                appLifeInsurance.InsuranceTypeId = input.InsuranceTypeId;
                appLifeInsurance.InsuranceAmount = input.InsuranceAmount;
                appLifeInsurance.CurrencyId = input.CurrencyId;
                appLifeInsurance.Percentage = input.Percentage;
                appLifeInsurance.Premi = input.Premi;
                appLifeInsurance.DateStart = input.DateStart;
                appLifeInsurance.DateEnd = input.DateEnd;
                appLifeInsurance.PolicyNo = input.PolicyNo;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Asuransi/Kredit/5/4/3/2/1
        [Route("Kredit/{apRegno}/{appType}/{productId}/{productSeq}/{seq}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKredit(string apRegno, string appType, string productId, int productSeq, int seq, [FromBody] AsuransiKreditMobile input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                AppCredAsurance appCredAsurance = db.AppCredAsurances
                    .Where(p => p.ApRegno == input.ApRegno && p.AppType == input.AppType && p.ProductId == input.ProductId && p.ProductSeq == input.ProductSeq && p.Seq == input.Seq)
                    .FirstOrDefault();

                if (appCredAsurance == null)
                {
                    return BadRequest("No record found!");
                }

                // appCredAsurance.InsuranceCompanyId = input.InsuranceCompanyId;
                appCredAsurance.InsuranceTypeId = input.InsuranceTypeId;
                appCredAsurance.InsuranceAmount = input.InsuranceAmount;
                appCredAsurance.CurrencyId = input.CurrencyId;
                appCredAsurance.Percentage = input.Percentage;
                appCredAsurance.Premi = input.Premi;
                appCredAsurance.DateStart = input.DateStart;
                appCredAsurance.DateEnd = input.DateEnd;
                appCredAsurance.PolicyNo = input.PolicyNo;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Asuransi/Agunan/5/4/3/2
        [Route("Agunan/{apRegno}/{productId}/{collateralSeq}/{seq}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgunan(string apRegno, string productId, int collateralSeq, int seq, [FromBody] AsuransiAgunanMobile input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                AppColAsurance appColAsurance = db.AppColAsurances
                    .Where(p => p.ApRegno == input.ApRegno && p.ProductId == input.ProductId && p.CollateralSeq == input.CollateralSeq && p.Seq == input.Seq)
                    .FirstOrDefault();

                if (appColAsurance == null)
                {
                    return BadRequest("No record found!");
                }

                // appColAsurance.InsuranceCompanyId = input.InsuranceCompanyId;
                appColAsurance.InsuranceTypeId = input.InsuranceTypeId;
                appColAsurance.InsuranceAmount = input.InsuranceAmount;
                appColAsurance.CurrencyId = input.CurrencyId;
                appColAsurance.Percentage = input.Percentage;
                appColAsurance.Premi = input.Premi;
                appColAsurance.DateStart = input.DateStart;
                appColAsurance.DateEnd = input.DateEnd;
                appColAsurance.PolicyNo = input.PolicyNo;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}