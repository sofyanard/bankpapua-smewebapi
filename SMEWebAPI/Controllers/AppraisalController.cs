using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DMS.CuBESCore;
using DMS.DBConnection;
using SMEWebAPI.Models;
using System.Web.Http.Description;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Appraisal")]
    public class AppraisalController : ApiController
    {
        private Connection conn;
        private LOSSME db = new LOSSME();

        public AppraisalController()
        {
            conn = new Connection(ConfigurationManager.AppSettings["ConnSME"]);
        }

        // GET: api/Appraisal
        public IHttpActionResult GetPipeline()
        {
            try
            {
                string email = User.Identity.GetUserName();

                /*
                conn.QueryString = "SELECT USERID FROM SCUSER WHERE SU_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                string userid = conn.GetFieldValue("USERID");

                if ((userid == null) || (userid.Trim() == ""))
                {
                    throw new Exception("User email is not found");
                }

                conn.QueryString = "select distinct l.AP_REGNO, l.CU_REF, l.CL_SEQ, " +
                    "case c.CU_CUSTTYPEID when '01' then isnull(c3.COMPTYPEDESC, '') +' ' + isnull(c2.CU_COMPNAME, '') " +
                    "else isnull(c1.CU_FIRSTNAME, '') + ' ' + isnull(c1.CU_MIDDLENAME, '') + ' ' + isnull(c1.CU_LASTNAME, '') end as CU_NAME, " +
                    "e.COLTYPEDESC, d.CL_DESC, l.LA_APPRSTATUS " +
                    "from APPLICATION a " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join LISTASSIGNMENT l on a.AP_REGNO = l.AP_REGNO and a.CU_REF = l.CU_REF " +
                    "join COLLATERAL d on l.CU_REF = d.CU_REF and l.CL_SEQ = d.CL_SEQ " +
                    "join RFCOLLATERALTYPE e on d.CL_TYPE = e.COLTYPESEQ " +
                    "where l.LA_APPRSTATUS = '2' and a.AP_REJECT = '0' and a.AP_CANCEL = '0' " +
                    "and l.OFFICERSEQ = '" + userid + "'";
                conn.ExecuteQuery();
                */

                conn.QueryString = "select distinct l.AP_REGNO, l.CU_REF, l.CL_SEQ, " +
                    "case c.CU_CUSTTYPEID when '01' then isnull(c3.COMPTYPEDESC, '') +' ' + isnull(c2.CU_COMPNAME, '') " +
                    "else isnull(c1.CU_FIRSTNAME, '') + ' ' + isnull(c1.CU_MIDDLENAME, '') + ' ' + isnull(c1.CU_LASTNAME, '') end as CU_NAME, " +
                    "e.COLTYPEDESC, d.CL_DESC, l.LA_APPRSTATUS " +
                    "from APPLICATION a " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join LISTASSIGNMENT l on a.AP_REGNO = l.AP_REGNO and a.CU_REF = l.CU_REF " +
                    "join COLLATERAL d on l.CU_REF = d.CU_REF and l.CL_SEQ = d.CL_SEQ " +
                    "join RFCOLLATERALTYPE e on d.CL_TYPE = e.COLTYPESEQ " +
                    "join RFAGENCY g on l.AGENCYID = g.AGENCYID and g.AGENCYTYPEID = '01'" +
                    "where l.LA_APPRSTATUS in ('1', '2') and a.AP_REJECT = '0' and a.AP_CANCEL = '0' " +
                    "and g.AGENCY_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                DataTable dtResult = conn.GetDataTable().Copy();

                if ((dtResult == null) || (dtResult.Rows.Count == 0))
                {
                    return NotFound();
                }

                List<AppraisalPipeLine> listAppraisalPipeLine = new List<AppraisalPipeLine>();

                foreach (DataRow row in dtResult.Rows)
                {
                    if ((row != null) && (row.ItemArray != null))
                    {
                        AppraisalPipeLine appraisalPipeLine = new AppraisalPipeLine();
                        appraisalPipeLine.ApRegno = row["AP_REGNO"].ToString();
                        appraisalPipeLine.CuRef = row["CU_REF"].ToString();
                        appraisalPipeLine.ClSeq = int.Parse(row["CL_SEQ"].ToString());
                        appraisalPipeLine.CustomerName = row["CU_NAME"].ToString();
                        appraisalPipeLine.CollateralType = row["COLTYPEDESC"].ToString();
                        appraisalPipeLine.CollateralDesc = row["CL_DESC"].ToString();
                        appraisalPipeLine.AppraisalStatus = row["LA_APPRSTATUS"].ToString();
                        appraisalPipeLine.AppraisalDate = null;

                        listAppraisalPipeLine.Add(appraisalPipeLine);
                    }
                }

                return Ok(listAppraisalPipeLine);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Appraisal/5/4/3
        [Route("{apRegno}/{cuRef}/{clSeq}")]
        [ResponseType(typeof(AppraisalMobile))]
        public IHttpActionResult GetOne(string apRegno, string cuRef, int clSeq)
        {
            try
            {
                AppraisalResultNew appraisalResultNew = db.AppraisalResultNews
                    .Where(p => p.ApRegno == apRegno && p.CuRef == cuRef && p.ClSeq == clSeq)
                    .FirstOrDefault();

                if (appraisalResultNew == null)
                {
                    return NotFound();
                }

                AppraisalMobile appraisalMobile = new AppraisalMobile();
                appraisalMobile.ApRegno = appraisalResultNew.ApRegno;
                appraisalMobile.CuRef = appraisalResultNew.CuRef;
                appraisalMobile.ClSeq = appraisalResultNew.ClSeq;
                appraisalMobile.ApprDate = appraisalResultNew.ApprDate;
                appraisalMobile.ApprValueBank = appraisalResultNew.ApprValueBank;
                appraisalMobile.ApprValuePasar = appraisalResultNew.ApprValuePasar;
                appraisalMobile.ApprValueLikuidasi = appraisalResultNew.ApprValueLikuidasi;
                appraisalMobile.ApprMarketabilityCode = appraisalResultNew.ApprMrCode;
                appraisalMobile.RfApprMarketability = appraisalResultNew.RfApprMarketability;
                appraisalMobile.ApprIkatSempurnaCode = appraisalResultNew.ApprIksCode;
                appraisalMobile.RfApprIkatSempurna = appraisalResultNew.RfApprIkatSempurna;
                appraisalMobile.ApprKuasaCode = appraisalResultNew.ApprKuCode;
                appraisalMobile.RfApprKuasa = appraisalResultNew.RfApprKuasa;
                appraisalMobile.ApprMasalahCode = appraisalResultNew.ApprPmCode;
                appraisalMobile.RfApprMasalah = appraisalResultNew.RfApprMasalah;

                return Ok(appraisalMobile);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Appraisal/5/4/3
        [Route("{apRegno}/{cuRef}/{clSeq}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string apRegno, string cuRef, int clSeq, [FromBody] AppraisalMobile input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                AppraisalResultNew appraisalResultNew = db.AppraisalResultNews
                    .Where(p => p.ApRegno == input.ApRegno && p.CuRef == input.CuRef && p.ClSeq == input.ClSeq)
                    .FirstOrDefault();

                if (appraisalResultNew == null)
                {
                    return BadRequest("No record found!");
                }

                appraisalResultNew.ApprDate = input.ApprDate;
                appraisalResultNew.ApprValueBank = input.ApprValueBank;
                appraisalResultNew.ApprValuePasar = input.ApprValuePasar;
                appraisalResultNew.ApprValueLikuidasi = input.ApprValueLikuidasi;
                appraisalResultNew.ApprMrCode = input.ApprMarketabilityCode;
                appraisalResultNew.ApprIksCode = input.ApprIkatSempurnaCode;
                appraisalResultNew.ApprKuCode = input.ApprKuasaCode;
                appraisalResultNew.ApprPmCode = input.ApprMasalahCode;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // POST: api/Appraisal
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(AppraisalMobile input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                AppraisalResultNew appraisalResultNew = db.AppraisalResultNews
                    .Where(p => p.ApRegno == input.ApRegno && p.CuRef == input.CuRef && p.ClSeq == input.ClSeq)
                    .FirstOrDefault();

                if (appraisalResultNew != null)
                {
                    return BadRequest("Record has already exists!");
                }

                AppraisalResultNew newAppraisalResultNew = new AppraisalResultNew();

                newAppraisalResultNew.ApRegno = input.ApRegno;
                newAppraisalResultNew.CuRef = input.CuRef;
                newAppraisalResultNew.ClSeq = input.ClSeq;
                newAppraisalResultNew.ApprDate = input.ApprDate;
                newAppraisalResultNew.ApprValueBank = input.ApprValueBank;
                newAppraisalResultNew.ApprValuePasar = input.ApprValuePasar;
                newAppraisalResultNew.ApprValueLikuidasi = input.ApprValueLikuidasi;
                newAppraisalResultNew.ApprMrCode = input.ApprMarketabilityCode;
                newAppraisalResultNew.ApprIksCode = input.ApprIkatSempurnaCode;
                newAppraisalResultNew.ApprKuCode = input.ApprKuasaCode;
                newAppraisalResultNew.ApprPmCode = input.ApprMasalahCode;

                db.AppraisalResultNews.Add(newAppraisalResultNew);
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
