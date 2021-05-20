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
using System.Web.Http.Description;

namespace SMEWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/SiteVisitRumah")]
    public class SiteVisitRumahController : ApiController
    {
        private Connection conn;
        private LOSSME db = new LOSSME();

        public SiteVisitRumahController()
        {
            conn = new Connection(ConfigurationManager.AppSettings["ConnSME"]);
        }

        // GET: SiteVisitRumah
        public IHttpActionResult Get()
        {
            try
            {
                string email = User.Identity.GetUserName();
                conn.QueryString = "SELECT USERID FROM SCUSER WHERE SU_EMAIL = '" + email + "'";
                conn.ExecuteQuery();

                string userid = conn.GetFieldValue("USERID");

                if ((userid == null) || (userid.Trim() == ""))
                {
                    throw new Exception("User email is not found");
                }

                conn.QueryString = "SELECT IN_SMALL, IN_MIDDLE, IN_CORPORATE, IN_MICRO FROM RFINITIAL";
                conn.ExecuteQuery();

                string mInMicro = conn.GetFieldValue("IN_SMALL");
                string mInSmall = conn.GetFieldValue("IN_MIDDLE");
                string mInCorp = conn.GetFieldValue("IN_CORPORATE");
                string mInCons = conn.GetFieldValue("IN_MICRO");

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

        // GET: api/SiteVisitRumah/5
        [Route("{apRegno}")]
        [ResponseType(typeof(SiteVisitRumahInput))]
        public IHttpActionResult GetOne(string apRegno)
        {
            try
            {
                CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

                if (custSiteVisit == null)
                {
                    return NotFound();
                }

                SiteVisitRumahInput siteVisitRumahInput = new SiteVisitRumahInput();
                siteVisitRumahInput.ApRegno = custSiteVisit.ApRegno;
                siteVisitRumahInput.CuRef = custSiteVisit.CuRef;
                siteVisitRumahInput.TanggalInvestigasi = custSiteVisit.SvHsInvestigasiDate;
                siteVisitRumahInput.NamaPemberiKeterangan1 = custSiteVisit.SvHsPemberiKet1;
                siteVisitRumahInput.HubunganPemberiKeterangan1 = custSiteVisit.SvHsHubPemberiKet1;
                siteVisitRumahInput.NamaPemberiKeterangan2 = custSiteVisit.SvHsPemberiKet2;
                siteVisitRumahInput.HubunganPemberiKeterangan2 = custSiteVisit.SvHsHubPemberiKet2;
                siteVisitRumahInput.StatusRumah = custSiteVisit.SvHsStatusHomeStay;
                siteVisitRumahInput.CekBerdasarkanX = custSiteVisit.SvHsCekArsipStay;
                siteVisitRumahInput.RumahSebagaiAgunanX = custSiteVisit.SvHsAgunanStay;
                siteVisitRumahInput.LamaMenetapTahun = custSiteVisit.SvHsDayStay;
                siteVisitRumahInput.LamaMenetapBulan = custSiteVisit.SvHsMonthStay;
                siteVisitRumahInput.JenisBangunan = custSiteVisit.SvHsBangunanTypeStay;
                siteVisitRumahInput.LokasiBangunan = custSiteVisit.SvHsBangunanLokasiStay;
                siteVisitRumahInput.KondisiBangunan = custSiteVisit.SvHsBangunanCondStay;
                siteVisitRumahInput.FasilitasRumah = custSiteVisit.SvHsFasilitasStay;
                siteVisitRumahInput.IsiRumahX = custSiteVisit.SvHsBarangHomeStay;
                siteVisitRumahInput.AksesJalan = custSiteVisit.SvHsAksesRoadStay;
                siteVisitRumahInput.KondisiLingkungan = custSiteVisit.SvHsLingkunganStay;
                siteVisitRumahInput.LuasTanah = custSiteVisit.SvHsLuasTanahStay;
                siteVisitRumahInput.LuasBangunan = custSiteVisit.SvHsLuasBangunanStay;
                siteVisitRumahInput.Garasi = custSiteVisit.SvHsGarasiStay;
                siteVisitRumahInput.Carport = custSiteVisit.SvHsCarPortStay;
                siteVisitRumahInput.Kendaraan = custSiteVisit.SvHsVehicleStay;
                siteVisitRumahInput.AlamatSesuaiKTPX = null;
                siteVisitRumahInput.TeleponDapatDihubungiX = null;

                return Ok(siteVisitRumahInput);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/SiteVisitRumah/5
        [Route("{apRegno}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string apRegno, [FromBody] SiteVisitRumahInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

                if (custSiteVisit == null)
                {
                    return BadRequest("No record found!");
                }

                custSiteVisit.SvHsInvestigasiDate = input.TanggalInvestigasi;
                custSiteVisit.SvHsPemberiKet1 = input.NamaPemberiKeterangan1;
                custSiteVisit.SvHsHubPemberiKet1 = input.HubunganPemberiKeterangan1;
                custSiteVisit.SvHsPemberiKet2 = input.NamaPemberiKeterangan2;
                custSiteVisit.SvHsHubPemberiKet2 = input.HubunganPemberiKeterangan2;
                custSiteVisit.SvHsStatusHomeStay = input.StatusRumah;
                // custSiteVisit.SvHsCekArsipStay = input.CekBerdasarkanX;
                // custSiteVisit.SvHsAgunanStay = input.RumahSebagaiAgunanX;
                custSiteVisit.SvHsDayStay = input.LamaMenetapTahun;
                custSiteVisit.SvHsMonthStay = input.LamaMenetapBulan;
                custSiteVisit.SvHsBangunanTypeStay = input.JenisBangunan;
                custSiteVisit.SvHsBangunanLokasiStay = input.LokasiBangunan;
                custSiteVisit.SvHsBangunanCondStay = input.KondisiBangunan;
                custSiteVisit.SvHsFasilitasStay = input.FasilitasRumah;
                // custSiteVisit.SvHsBarangHomeStay = input.IsiRumahX;
                custSiteVisit.SvHsAksesRoadStay = input.AksesJalan;
                custSiteVisit.SvHsLingkunganStay = input.KondisiLingkungan;
                custSiteVisit.SvHsLuasTanahStay = input.LuasTanah;
                custSiteVisit.SvHsLuasBangunanStay = input.LuasBangunan;
                custSiteVisit.SvHsGarasiStay = input.Garasi;
                custSiteVisit.SvHsCarPortStay = input.Carport;
                custSiteVisit.SvHsVehicleStay = input.Kendaraan;
                // AlamatSesuaiKTPX
                // TeleponDapatDihubungiX

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // POST: api/SiteVisitRumah
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(SiteVisitRumahInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Application application = db.Applications.Find(input.ApRegno);

                if (application == null)
                {
                    return BadRequest("No application found!");
                }

                string cuRef = application.CuRef;

                CustSiteVisit custSiteVisit = new CustSiteVisit();

                custSiteVisit.ApRegno = input.ApRegno;
                custSiteVisit.CuRef = cuRef;
                custSiteVisit.SvHsInvestigasiDate = input.TanggalInvestigasi;
                custSiteVisit.SvHsPemberiKet1 = input.NamaPemberiKeterangan1;
                custSiteVisit.SvHsHubPemberiKet1 = input.HubunganPemberiKeterangan1;
                custSiteVisit.SvHsPemberiKet2 = input.NamaPemberiKeterangan2;
                custSiteVisit.SvHsHubPemberiKet2 = input.HubunganPemberiKeterangan2;
                custSiteVisit.SvHsStatusHomeStay = input.StatusRumah;
                // custSiteVisit.SvHsCekArsipStay = input.CekBerdasarkanX;
                // custSiteVisit.SvHsAgunanStay = input.RumahSebagaiAgunanX;
                custSiteVisit.SvHsDayStay = input.LamaMenetapTahun;
                custSiteVisit.SvHsMonthStay = input.LamaMenetapBulan;
                custSiteVisit.SvHsBangunanTypeStay = input.JenisBangunan;
                custSiteVisit.SvHsBangunanLokasiStay = input.LokasiBangunan;
                custSiteVisit.SvHsBangunanCondStay = input.KondisiBangunan;
                custSiteVisit.SvHsFasilitasStay = input.FasilitasRumah;
                // custSiteVisit.SvHsBarangHomeStay = input.IsiRumahX;
                custSiteVisit.SvHsAksesRoadStay = input.AksesJalan;
                custSiteVisit.SvHsLingkunganStay = input.KondisiLingkungan;
                custSiteVisit.SvHsLuasTanahStay = input.LuasTanah;
                custSiteVisit.SvHsLuasBangunanStay = input.LuasBangunan;
                custSiteVisit.SvHsGarasiStay = input.Garasi;
                custSiteVisit.SvHsCarPortStay = input.Carport;
                custSiteVisit.SvHsVehicleStay = input.Kendaraan;
                // AlamatSesuaiKTPX
                // TeleponDapatDihubungiX

                db.CustSiteVisits.Add(custSiteVisit);
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
