using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DMS.CuBESCore;
using DMS.DBConnection;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/SiteVisitKantor")]
    public class SiteVisitKantorController : ApiController
    {
        private Connection conn;

        public SiteVisitKantorController()
        {
            conn = new Connection(ConfigurationManager.AppSettings["ConnSME"]);
        }

        // GET: SiteVisitUsaha
        public IHttpActionResult Get()
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
                */

                conn.QueryString = "SELECT IN_SMALL, IN_MIDDLE, IN_CORPORATE, IN_MICRO FROM RFINITIAL";
                conn.ExecuteQuery();

                string mInMicro = conn.GetFieldValue("IN_SMALL");
                string mInSmall = conn.GetFieldValue("IN_MIDDLE");
                string mInCorp = conn.GetFieldValue("IN_CORPORATE");
                string mInCons = conn.GetFieldValue("IN_MICRO");

                /*
                conn.QueryString = "select distinct a.AP_REGNO, a.CU_REF, a.AP_RELMNGR as CU_RM, " +
                    "case c.CU_CUSTTYPEID when '01' then isnull(c3.COMPTYPEDESC, '') +' ' + isnull(c2.CU_COMPNAME, '') " +
                    "else isnull(c1.CU_FIRSTNAME, '') + ' ' + isnull(c1.CU_MIDDLENAME, '') + ' ' + isnull(c1.CU_LASTNAME, '') end as CU_NAME, " +
                    "a.AP_SITEVISITSTA " +
                    "from APPLICATION a " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join APPTRACK t on a.AP_REGNO = t.AP_REGNO " +
                    "where a.AP_REJECT <> '1' and a.AP_CANCEL <> '1' " +
                    "and t.AP_CURRTRACK = 'BP5.0' " + // *** temporary hard-coded
                    "and a.AP_COMPLEVEL in ('" + mInCons + "') " + // *** temporary hard-coded
                    "and a.AP_RELMNGR = '" + userid + "'";
                */

                conn.QueryString = "select distinct a.AP_REGNO, a.CU_REF, a.AP_RELMNGR as CU_RM, " +
                    "case c.CU_CUSTTYPEID when '01' then isnull(c3.COMPTYPEDESC, '') +' ' + isnull(c2.CU_COMPNAME, '') " +
                    "else isnull(c1.CU_FIRSTNAME, '') + ' ' + isnull(c1.CU_MIDDLENAME, '') + ' ' + isnull(c1.CU_LASTNAME, '') end as CU_NAME, " +
                    "a.AP_SITEVISITSTA " +
                    "from APPLICATION a " +
                    "join CUSTOMER c on a.CU_REF = c.CU_REF " +
                    "left join CUST_PERSONAL c1 on c.CU_REF = c1.CU_REF " +
                    "left join CUST_COMPANY c2 on c.CU_REF = c2.CU_REF " +
                    "left join RFCOMPTYPE c3 on c2.CU_COMPTYPE = c3.COMPTYPEID " +
                    "join APPTRACK t on a.AP_REGNO = t.AP_REGNO " +
                    "join SITEVISITASSIGNMENT v on a.AP_REGNO = v.AP_REGNO " +
                    "where a.AP_REJECT <> '1' and a.AP_CANCEL <> '1' " +
                    "and t.AP_CURRTRACK = 'BP5.0' " + // *** temporary hard-coded
                    "and a.AP_COMPLEVEL in ('" + mInCons + "') " + // *** temporary hard-coded
                    "and v.OFFICERID = '" + email + "'";
                conn.ExecuteQuery();

                DataTable dtResult = conn.GetDataTable().Copy();

                if ((dtResult == null) || (dtResult.Rows.Count == 0))
                {
                    return NotFound();
                }

                List<SiteVisitPipeLine> listSiteVisitPipeLine = new List<SiteVisitPipeLine>();

                foreach (DataRow row in dtResult.Rows)
                {
                    if ((row != null) && (row.ItemArray != null))
                    {
                        SiteVisitPipeLine siteVisitPipeLine = new SiteVisitPipeLine();
                        siteVisitPipeLine.ApRegno = row["AP_REGNO"].ToString();
                        siteVisitPipeLine.CuRef = row["CU_REF"].ToString();
                        siteVisitPipeLine.CustomerName = row["CU_NAME"].ToString();
                        siteVisitPipeLine.SiteVisitStatus = row["AP_SITEVISITSTA"].ToString();
                        siteVisitPipeLine.SiteVisitDate = null;

                        listSiteVisitPipeLine.Add(siteVisitPipeLine);
                    }
                }

                return Ok(listSiteVisitPipeLine);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
