using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using DMS.CuBESCore;
using DMS.DBConnection;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notaris")]
    public class NotarisController : ApiController
    {
        private Connection conn;
        private LOSSME db = new LOSSME();

        public NotarisController()
        {
            conn = new Connection(ConfigurationManager.AppSettings["ConnSME"]);
        }

        // GET: api/Notaris
        public IHttpActionResult GetPipeline()
        {
            try
            {
                string email = User.Identity.GetUserName();
                string companyId;

                conn.QueryString = "SELECT NTID FROM RFNOTARY WHERE NT_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                if (conn.GetRowCount() > 0)
                {
                    companyId = conn.GetFieldValue("NTID");
                }
                else
                {
                    return NotFound();
                }

                conn.QueryString = "select " +
                    // Key
                    "n.AP_REGNO, n.SEQ, " +
                    // Application Data
                    "case c.CU_CUSTTYPEID when '02' then c1.CU_FIRSTNAME + isnull(' ' + c1.CU_MIDDLENAME, '') + isnull(' ' + c1.CU_LASTNAME, '') " +
                    "when '01' then c3.COMPTYPEDESC + ' ' + c2.CU_COMPNAME end as CU_NAME, " +
                    "u.SU_FULLNAME " +
                    "from NOTARYASSIGN n " +
                    "join APPLICATION a on n.AP_REGNO = a.AP_REGNO " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join SCUSER u on a.AP_RELMNGR = u.USERID " +
                    "where n.NTID = '" + companyId + "'";
                conn.ExecuteQuery();

                DataTable dtResult = conn.GetDataTable().Copy();

                if ((dtResult == null) || (dtResult.Rows.Count == 0))
                {
                    return NotFound();
                }

                List<NotarisPipeline> listNotarisPipeline = new List<NotarisPipeline>();

                foreach (DataRow row in dtResult.Rows)
                {
                    if ((row != null) && (row.ItemArray != null))
                    {
                        NotarisPipeline notarisPipeline = new NotarisPipeline();

                        notarisPipeline.ApRegno = row["AP_REGNO"].ToString();
                        notarisPipeline.Seq = int.Parse(row["SEQ"].ToString());
                        notarisPipeline.CustomerName = row["CU_NAME"].ToString();
                        notarisPipeline.OfficerName = row["SU_FULLNAME"].ToString();

                        listNotarisPipeline.Add(notarisPipeline);
                    }
                }

                return Ok(listNotarisPipeline);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Notaris/5/4
        [Route("{apRegno}/{seq}")]
        [ResponseType(typeof(NotaryAssign))]
        public IHttpActionResult GetOne(string apRegno, int seq)
        {
            try
            {
                NotaryAssign notaryAssign = db.NotaryAssigns
                    .Where(p => p.ApRegno == apRegno && p.Seq == seq)
                    .FirstOrDefault();

                if (notaryAssign == null)
                {
                    return NotFound();
                }

                List<NotaryAssignDetail> listNotaryAssignDetail = db.NotaryAssignDetails
                    .Where(p => p.ApRegno == apRegno && p.Seq == seq)
                    .ToList();

                notaryAssign.NotaryAssignDetails = listNotaryAssignDetail;

                return Ok(notaryAssign);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Notaris/5/4
        [Route("{apRegno}/{seq}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string apRegno, int seq, [FromBody] NotaryAssignMobile input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                NotaryAssign notaryAssign = db.NotaryAssigns
                    .Where(p => p.ApRegno == apRegno && p.Seq == seq)
                    .FirstOrDefault();

                if (notaryAssign == null)
                {
                    return BadRequest("No record found!");
                }

                notaryAssign.OrderNo = input.OrderNo;
                notaryAssign.OrderDate = input.OrderDate;
                notaryAssign.Remarks = input.Remarks;

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
