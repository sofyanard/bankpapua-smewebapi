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

        // GET: api/Notaris/5/4/InfoCustomer
        [Route("{apRegno}/{seq}/InfoCustomer")]
        [ResponseType(typeof(NotarisInfoCustomer))]
        public IHttpActionResult GetCustomerInfo(string apRegno, int seq)
        {
            try
            {
                conn.QueryString = "select top 1 " +
                    "a.AP_REGNO, " +
                    "a.BRANCH_CODE + ' -  ' + b.BRANCH_NAME as BRANCHNAME, " +
                    "u.SU_FULLNAME + isnull(' ('+u.SU_HPNUM+')','') as AONAME, " +
                    "case c.CU_CUSTTYPEID when '02' then c1.CU_FIRSTNAME + isnull(' '+c1.CU_MIDDLENAME,'') + isnull(' '+c1.CU_LASTNAME,'') " +
                    "when '01' then c3.COMPTYPEDESC + ' ' + c2.CU_COMPNAME end as CU_NAME, " +
                    "case c.CU_CUSTTYPEID when '02' then c1.CU_ADDR1 + isnull(' '+c1.CU_ADDR2,'') + isnull(' '+c1.CU_ADDR3,'') " +
                    "when '01' then c2.CU_COMPADDR1 + isnull(' '+c2.CU_COMPADDR2,'') + isnull(' '+c2.CU_COMPADDR3,'') end as CU_ADDRESS, " +
                    "case c.CU_CUSTTYPEID when '02' then c4.CITYNAME + isnull(' '+c1.CU_ZIPCODE,'') " +
                    "when '01' then c5.CITYNAME + isnull(' '+c2.CU_COMPZIPCODE,'') end as CU_CITY, " +
                    "case c.CU_CUSTTYPEID when '02' then isnull(c1.CU_PHNAREA+' ','') + isnull(c1.CU_PHNNUM,'') + isnull(' '+c1.CU_PHNEXT,'') " +
                    "when '01' then isnull(c2.CU_COMPPHNAREA+' ','') + isnull(c2.CU_COMPPHNNUM,'') + isnull(' '+c2.CU_COMPPHNEXT,'') end as CU_PHONENO, " +
                    "case c.CU_CUSTTYPEID when '02' then isnull(c1.CU_FAXAREA+' ','') + isnull(c1.CU_FAXNUM,'') + isnull(' '+c1.CU_FAXEXT,'') " +
                    "when '01' then isnull(c2.CU_COMPFAXAREA+' ','') + isnull(c2.CU_COMPFAXNUM,'') + isnull(' '+c2.CU_COMPFAXEXT,'') end as CU_MOBILENO, " +
                    "case c.CU_CUSTTYPEID when '02' then '' " +
                    "when '01' then c2.CU_CONTACTPERSON end as CU_CONTACT " +
                    "from APPLICATION a " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "left join RFCITY c4 on c1.CU_CITY = c4.CITYID " +
                    "left join RFCITY c5 on c2.CU_COMPCITY = c5.CITYID " +
                    "join RFBRANCH b on a.BRANCH_CODE = b.BRANCH_CODE " +
                    "join SCUSER u on a.AP_RELMNGR = u.USERID " +
                    "where a.AP_REGNO = '" + apRegno + "'";
                conn.ExecuteQuery();

                if (conn.GetRowCount() == 0)
                {
                    return NotFound();
                }

                NotarisInfoCustomer notarisInfoCustomer = new NotarisInfoCustomer();

                notarisInfoCustomer.ApRegno = conn.GetFieldValue("AP_REGNO");
                notarisInfoCustomer.BranchName = conn.GetFieldValue("BRANCHNAME");
                notarisInfoCustomer.AccountOfficer = conn.GetFieldValue("AONAME");
                notarisInfoCustomer.CustomerName = conn.GetFieldValue("CU_NAME");
                notarisInfoCustomer.CustomerAddress = conn.GetFieldValue("CU_ADDRESS");
                notarisInfoCustomer.CustomerCity = conn.GetFieldValue("CU_CITY");
                notarisInfoCustomer.HomePhoneNo = conn.GetFieldValue("CU_PHONENO");
                notarisInfoCustomer.MobilePhoneNo = conn.GetFieldValue("CU_MOBILENO");
                notarisInfoCustomer.ContactPerson = conn.GetFieldValue("CU_CONTACT");

                return Ok(notarisInfoCustomer);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Notaris/5/4/InfoCollateral
        [Route("{apRegno}/{seq}/InfoCollateral")]
        [ResponseType(typeof(NotarisInfoCollateral))]
        public IHttpActionResult GetCollateralInfo(string apRegno, int seq)
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

                string nCuRef = notaryAssign.CuRef;
                int? nClSeq = notaryAssign.CollateralSeq;

                if ((nCuRef == null) || (nCuRef.Trim() == String.Empty) || (nClSeq == null))
                {
                    return NotFound();
                }

                conn.QueryString = "select top 1 l.AP_REGNO, l.CU_REF, l.CL_SEQ, " +
                    "c.CL_DESC, " +
                    "c3.COLTYPEDESC, " +
                    "isnull(c2.CL_LOCJLN,'') + isnull(' '+c2.CL_LOCKAVNO,'') + isnull(' RT '+c2.CL_LOCRT,'') + isnull(' RW '+c2.CL_LOCRW,'') as CL_ADDRESS, " +
                    "d.BI_DESC, " +
                    "c2.CL_OWNER, " +
                    "e.CERTTYPEDESC, " +
                    "c2.CL_CERTNO " +
                    "from LISTASSIGNMENT l " +
                    "join COLLATERAL c on l.CU_REF = c.CU_REF and l.CL_SEQ = c.CL_SEQ " +
                    "join COLLATERAL_RE c2 on c.CU_REF = c2.CU_REF and c.CL_SEQ = c2.CL_SEQ " +
                    "join RFCOLLATERALTYPE c3 on c.CL_TYPE = c3.COLTYPESEQ " +
                    "left join RFBICODE d on c2.CL_COLLOC = d.BI_SEQ and d.BG_GROUP = '4' " +
                    "left join RFCERTTYPE e on c.CL_CERTTYPE1 = e.CERTTYPEID " +
                    "where l.AP_REGNO = '" + apRegno + "' and l.CU_REF = '" + nCuRef + "' and l.CL_SEQ = " + nClSeq;
                conn.ExecuteQuery();

                if (conn.GetRowCount() == 0)
                {
                    return NotFound();
                }

                NotarisInfoCollateral notarisInfoCollateral = new NotarisInfoCollateral();

                notarisInfoCollateral.ApRegno = conn.GetFieldValue("AP_REGNO");
                notarisInfoCollateral.CuRef = conn.GetFieldValue("CU_REF");
                notarisInfoCollateral.ClSeq = conn.GetFieldValue("CL_SEQ");
                notarisInfoCollateral.Description = conn.GetFieldValue("CL_DESC");
                notarisInfoCollateral.CollateralType = conn.GetFieldValue("COLTYPEDESC");
                notarisInfoCollateral.Location = conn.GetFieldValue("BI_DESC");
                notarisInfoCollateral.Owner = conn.GetFieldValue("CL_OWNER");
                notarisInfoCollateral.CertificateType = conn.GetFieldValue("CERTTYPEDESC");
                notarisInfoCollateral.CertificateNo = conn.GetFieldValue("CL_CERTNO");

                return Ok(notarisInfoCollateral);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/Notaris/5/4/InfoFacility
        [Route("{apRegno}/{seq}/InfoFacility")]
        [ResponseType(typeof(NotarisInfoFacility))]
        public IHttpActionResult GetFacilityInfo(string apRegno, int seq)
        {
            try
            {
                conn.QueryString = "select top 1 " +
                    "a.AP_REGNO, c.APPTYPE, c.PRODUCTID, c.PROD_SEQ, " +
                    "p.PRODUCTDESC, " +
                    "c.CP_LIMIT, " +
                    "c.CP_JANGKAWKT, " +
                    "r.LOANPURPDESC, " +
                    "l.APL_PKDATE, " +
                    "l.APL_PKNO " +
                    "from APPLICATION a " +
                    "join CUSTPRODUCT c on a.AP_REGNO = c.AP_REGNO " +
                    "join RFPRODUCT p on c.PRODUCTID = p.PRODUCTID " +
                    "left join RFLOANPURPOSE r on c.CP_LOANPURPOSE = r.LOANPURPID " +
                    "left join APPPRODUCTLEGAL l on c.AP_REGNO = l.AP_REGNO and c.APPTYPE = l.APPTYPE and c.PRODUCTID = l.PRODUCTID and c.PROD_SEQ = l.PROD_SEQ " +
                    "where a.AP_REGNO = '" + apRegno + "'";
                conn.ExecuteQuery();

                if (conn.GetRowCount() == 0)
                {
                    return NotFound();
                }

                NotarisInfoFacility notarisInfoFacility = new NotarisInfoFacility();

                notarisInfoFacility.ApRegno = conn.GetFieldValue("AP_REGNO");
                notarisInfoFacility.AppType = conn.GetFieldValue("APPTYPE");
                notarisInfoFacility.ProductId = conn.GetFieldValue("PRODUCTID");
                notarisInfoFacility.ProductSeq = conn.GetFieldValue("PROD_SEQ");
                notarisInfoFacility.ProductDesc = conn.GetFieldValue("PRODUCTDESC");
                if (conn.GetFieldValue("CP_LIMIT").Trim() != "")
                    notarisInfoFacility.Limit = double.Parse(conn.GetFieldValue("CP_LIMIT"));
                if (conn.GetFieldValue("CP_JANGKAWKT").Trim() != "")
                    notarisInfoFacility.Tenor = int.Parse(conn.GetFieldValue("CP_JANGKAWKT"));
                notarisInfoFacility.LoanPurpose = conn.GetFieldValue("LOANPURPDESC");
                if (conn.GetFieldValue("APL_PKDATE").Trim() != "")
                    notarisInfoFacility.PkDate = DateTime.Parse(conn.GetFieldValue("APL_PKDATE"));
                notarisInfoFacility.PkNo = conn.GetFieldValue("APL_PKNO");

                return Ok(notarisInfoFacility);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
