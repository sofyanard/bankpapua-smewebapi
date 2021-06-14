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
    [RoutePrefix("api/SiteVisitKantor")]
    public class SiteVisitKantorController : ApiController
    {
        private Connection conn;
        private LOSSME db = new LOSSME();

        public SiteVisitKantorController()
        {
            conn = new Connection(ConfigurationManager.AppSettings["ConnSME"]);
        }

        // GET: SiteVisitKantor
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

        // GET: api/SiteVisitKantor/5
        [Route("{apRegno}")]
        [ResponseType(typeof(SiteVisitKantor))]
        public IHttpActionResult GetOne(string apRegno)
        {
            try
            {
                CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

                if (custSiteVisit == null)
                {
                    return NotFound();
                }

                SiteVisitKantor siteVisitKantor = new SiteVisitKantor();

                siteVisitKantor.ApRegno = custSiteVisit.ApRegno;
                siteVisitKantor.CuRef = custSiteVisit.CuRef;
                siteVisitKantor.TanggalInvestigasi = custSiteVisit.SvOfInvestigasiDate;
                siteVisitKantor.NamaPemberiKeterangan1 = custSiteVisit.SvOfPemberiKet1;
                siteVisitKantor.HubunganPemberiKeterangan1 = custSiteVisit.SvOfPosisiPemberiKet1;
                siteVisitKantor.NamaPemberiKeterangan2 = custSiteVisit.SvOfPemberiKet2;
                siteVisitKantor.HubunganPemberiKeterangan2 = custSiteVisit.SvOfPosisiPemberiKet2;
                siteVisitKantor.TipePerusahaan = custSiteVisit.SvOfTypeOffice;
                siteVisitKantor.NamaPerusahaan = custSiteVisit.SvOfNamaOffice;
                siteVisitKantor.AlamatPerusahaan = custSiteVisit.SvOfAddressOffice1;
                // siteVisitKantor.PropinsiPerusahaanX = custSiteVisit.SvOfCityCodeOffice;
                // siteVisitKantor.KotaPerusahaanX = custSiteVisit.SvOfCityOffice;
                // siteVisitKantor.KecamatanPerusahaanX = custSiteVisit.SvOfAddressOffice3;
                // siteVisitKantor.KelurahanPerusahaanX = custSiteVisit.SvOfAddressOffice2;
                siteVisitKantor.NoTeleponKantor = custSiteVisit.SvOfNoTelpOffice;
                siteVisitKantor.NoFaxKantor = custSiteVisit.SvOfNoFaxOffice;
                siteVisitKantor.LamaUsahaTahun = custSiteVisit.SvOfYearOffice;
                siteVisitKantor.BidangUsaha = custSiteVisit.SvOfUsahaOffice;
                siteVisitKantor.JumlahKaryawan = custSiteVisit.SvOfStafOffice;
                siteVisitKantor.SkalaPerusahaan = custSiteVisit.SvOfScaleOffice;
                siteVisitKantor.JenisBangunan = custSiteVisit.SvOfBangunanOffice;
                siteVisitKantor.LokasiBangunan = custSiteVisit.SvOfLokasiOffice;
                siteVisitKantor.KondisiBangunan = custSiteVisit.SvOfKondisiOffice;
                siteVisitKantor.StatusKepemilikan = custSiteVisit.SvOfOwnerOffice;
                siteVisitKantor.JenisPekerjaan = custSiteVisit.SvOfNameWork;
                siteVisitKantor.Jabatan = custSiteVisit.SvOfPosisiWork;
                siteVisitKantor.LamaBekerjaTahun = custSiteVisit.SvOfYearWork;
                siteVisitKantor.LamaBekerjaBulan = custSiteVisit.SvOfMonthWork;
                siteVisitKantor.StatusKaryawan = custSiteVisit.SvOfStatusWork;
                siteVisitKantor.UnitPekerjaan = custSiteVisit.SvOfUnitWork;
                siteVisitKantor.KinerjaKaryawan = custSiteVisit.SvOfKinerjaWork;
                siteVisitKantor.IncomeBrutoPemohon = custSiteVisit.SvOfIncomeBrutoPemohon;
                siteVisitKantor.IncomeOtherPemohon = custSiteVisit.SvOfOtherIncomePemohon;
                siteVisitKantor.PengeluaranPemohon = custSiteVisit.SvOfPayPemohon;
                siteVisitKantor.IncomeMargin = custSiteVisit.SvOfIncomeMargin;
                siteVisitKantor.JumlahTanggungan = custSiteVisit.SvHsJumlahTertanggung;

                // Pasangan

                siteVisitKantor.NamaPasangan = custSiteVisit.SvHsCoupleName1;
                siteVisitKantor.JenisPekerjaanPasangan = custSiteVisit.SvHsCoupleJob;
                siteVisitKantor.IncomeTotalPasangan = custSiteVisit.SvOfTotalIncomeSpouse;

                // Keluarga

                siteVisitKantor.NamaKeluarga = custSiteVisit.SvHsNamaFamily1;
                siteVisitKantor.AlamatKeluarga = custSiteVisit.SvHsAddressFamily1;
                siteVisitKantor.KodePosKeluarga = custSiteVisit.SvHsZipCodeFamily;
                // siteVisitKantor.PropinsiKeluargaX = custSiteVisit.SvHsCityCodeFamily;
                // siteVisitKantor.KotaKeluargaX = custSiteVisit.SvHsCityFamily;
                siteVisitKantor.NoTeleponKeluarga = custSiteVisit.SvHsNoTelpFamily;
                siteVisitKantor.NoHpKeluarga = custSiteVisit.SvHsNoHpFamily;
                siteVisitKantor.HubunganKeluarga = custSiteVisit.SvHsHubFamily;

                return Ok(siteVisitKantor);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /*
        // GET: api/SiteVisitKantor/Pekerjaan/5
        [Route("Pekerjaan/{apRegno}")]
        [ResponseType(typeof(SiteVisitKantorPekerjaan))]
        public IHttpActionResult GetOnePekerjaan(string apRegno)
        {
            try
            {
                CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

                if (custSiteVisit == null)
                {
                    return NotFound();
                }

                SiteVisitKantorPekerjaan siteVisitKantorPekerjaan = new SiteVisitKantorPekerjaan();

                siteVisitKantorPekerjaan.ApRegno = custSiteVisit.ApRegno;
                siteVisitKantorPekerjaan.CuRef = custSiteVisit.CuRef;
                siteVisitKantorPekerjaan.TanggalInvestigasi = custSiteVisit.SvOfInvestigasiDate;
                siteVisitKantorPekerjaan.NamaPemberiKeterangan1 = custSiteVisit.SvOfPemberiKet1;
                siteVisitKantorPekerjaan.HubunganPemberiKeterangan1 = custSiteVisit.SvOfPosisiPemberiKet1;
                siteVisitKantorPekerjaan.NamaPemberiKeterangan2 = custSiteVisit.SvOfPemberiKet2;
                siteVisitKantorPekerjaan.HubunganPemberiKeterangan2 = custSiteVisit.SvOfPosisiPemberiKet2;
                siteVisitKantorPekerjaan.TipePerusahaan = custSiteVisit.SvOfTypeOffice;
                siteVisitKantorPekerjaan.NamaPerusahaan = custSiteVisit.SvOfNamaOffice;
                siteVisitKantorPekerjaan.AlamatPerusahaan = custSiteVisit.SvOfAddressOffice1;
                // siteVisitKantorPekerjaan.PropinsiPerusahaanX = custSiteVisit.SvOfCityCodeOffice;
                // siteVisitKantorPekerjaan.KotaPerusahaanX = custSiteVisit.SvOfCityOffice;
                // siteVisitKantorPekerjaan.KecamatanPerusahaanX = custSiteVisit.SvOfAddressOffice3;
                // siteVisitKantorPekerjaan.KelurahanPerusahaanX = custSiteVisit.SvOfAddressOffice2;
                siteVisitKantorPekerjaan.NoTeleponKantor = custSiteVisit.SvOfNoTelpOffice;
                siteVisitKantorPekerjaan.NoFaxKantor = custSiteVisit.SvOfNoFaxOffice;
                siteVisitKantorPekerjaan.LamaUsahaTahun = custSiteVisit.SvOfYearOffice;
                siteVisitKantorPekerjaan.BidangUsaha = custSiteVisit.SvOfUsahaOffice;
                siteVisitKantorPekerjaan.JumlahKaryawan = custSiteVisit.SvOfStafOffice;
                siteVisitKantorPekerjaan.SkalaPerusahaan = custSiteVisit.SvOfScaleOffice;
                siteVisitKantorPekerjaan.JenisBangunan = custSiteVisit.SvOfBangunanOffice;
                siteVisitKantorPekerjaan.LokasiBangunan = custSiteVisit.SvOfLokasiOffice;
                siteVisitKantorPekerjaan.KondisiBangunan = custSiteVisit.SvOfKondisiOffice;
                siteVisitKantorPekerjaan.StatusKepemilikan = custSiteVisit.SvOfOwnerOffice;
                siteVisitKantorPekerjaan.JenisPekerjaan = custSiteVisit.SvOfNameWork;
                siteVisitKantorPekerjaan.Jabatan = custSiteVisit.SvOfPosisiWork;
                siteVisitKantorPekerjaan.LamaBekerjaTahun = custSiteVisit.SvOfYearWork;
                siteVisitKantorPekerjaan.LamaBekerjaBulan = custSiteVisit.SvOfMonthWork;
                siteVisitKantorPekerjaan.StatusKaryawan = custSiteVisit.SvOfStatusWork;
                siteVisitKantorPekerjaan.UnitPekerjaan = custSiteVisit.SvOfUnitWork;
                siteVisitKantorPekerjaan.KinerjaKaryawan = custSiteVisit.SvOfKinerjaWork;
                siteVisitKantorPekerjaan.IncomeBrutoPemohon = custSiteVisit.SvOfIncomeBrutoPemohon;
                siteVisitKantorPekerjaan.IncomeOtherPemohon = custSiteVisit.SvOfOtherIncomePemohon;
                siteVisitKantorPekerjaan.PengeluaranPemohon = custSiteVisit.SvOfPayPemohon;
                siteVisitKantorPekerjaan.IncomeMargin = custSiteVisit.SvOfIncomeMargin;
                siteVisitKantorPekerjaan.JumlahTanggungan = custSiteVisit.SvHsJumlahTertanggung;

                return Ok(siteVisitKantorPekerjaan);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        /*
        // GET: api/SiteVisitKantor/Pasangan/5
        [Route("Pasangan/{apRegno}")]
        [ResponseType(typeof(SiteVisitKantorPasangan))]
        public IHttpActionResult GetOnePasangan(string apRegno)
        {
            try
            {
                CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

                if (custSiteVisit == null)
                {
                    return NotFound();
                }

                SiteVisitKantorPasangan siteVisitKantorPasangan = new SiteVisitKantorPasangan();

                siteVisitKantorPasangan.ApRegno = custSiteVisit.ApRegno;
                siteVisitKantorPasangan.CuRef = custSiteVisit.CuRef;
                siteVisitKantorPasangan.NamaPasangan = custSiteVisit.SvHsCoupleName1;
                siteVisitKantorPasangan.JenisPekerjaanPasangan = custSiteVisit.SvHsCoupleJob;
                siteVisitKantorPasangan.IncomeTotalPasangan = custSiteVisit.SvOfTotalIncomeSpouse;

                return Ok(siteVisitKantorPasangan);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        /*
        // GET: api/SiteVisitKantor/Keluarga/5
        [Route("Keluarga/{apRegno}")]
        [ResponseType(typeof(SiteVisitKantorKeluarga))]
        public IHttpActionResult GetOneKeluarga(string apRegno)
        {
            try
            {
                CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

                if (custSiteVisit == null)
                {
                    return NotFound();
                }

                SiteVisitKantorKeluarga siteVisitKantorKeluarga = new SiteVisitKantorKeluarga();

                siteVisitKantorKeluarga.ApRegno = custSiteVisit.ApRegno;
                siteVisitKantorKeluarga.CuRef = custSiteVisit.CuRef;
                siteVisitKantorKeluarga.NamaKeluarga = custSiteVisit.SvHsNamaFamily1;
                siteVisitKantorKeluarga.AlamatKeluarga = custSiteVisit.SvHsAddressFamily1;
                siteVisitKantorKeluarga.KodePosKeluarga = custSiteVisit.SvHsZipCodeFamily;
                // siteVisitKantorKeluarga.PropinsiKeluargaX = custSiteVisit.SvHsCityCodeFamily;
                // siteVisitKantorKeluarga.KotaKeluargaX = custSiteVisit.SvHsCityFamily;
                siteVisitKantorKeluarga.NoTeleponKeluarga = custSiteVisit.SvHsNoTelpFamily;
                siteVisitKantorKeluarga.NoHpKeluarga = custSiteVisit.SvHsNoHpFamily;
                siteVisitKantorKeluarga.HubunganKeluarga = custSiteVisit.SvHsHubFamily;

                return Ok(siteVisitKantorKeluarga);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        // PUT: api/SiteVisitKantor/5
        [Route("{apRegno}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string apRegno, [FromBody] SiteVisitKantor input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

            if (custSiteVisit == null)
            {
                return NotFound();
            }

            try
            {
                custSiteVisit.SvOfInvestigasiDate = input.TanggalInvestigasi;
                custSiteVisit.SvOfPemberiKet1 = input.NamaPemberiKeterangan1;
                custSiteVisit.SvOfPosisiPemberiKet1 = input.HubunganPemberiKeterangan1;
                custSiteVisit.SvOfPemberiKet2 = input.NamaPemberiKeterangan2;
                custSiteVisit.SvOfPosisiPemberiKet2 = input.HubunganPemberiKeterangan2;
                custSiteVisit.SvOfTypeOffice = input.TipePerusahaan;
                custSiteVisit.SvOfNamaOffice = input.NamaPerusahaan;
                custSiteVisit.SvOfAddressOffice1 = input.AlamatPerusahaan;
                // custSiteVisit.SvOfCityCodeOffice = input.PropinsiPerusahaanX;
                // custSiteVisit.SvOfCityOffice = input.KotaPerusahaanX;
                // custSiteVisit.SvOfAddressOffice3 = input.KecamatanPerusahaanX;
                // custSiteVisit.SvOfAddressOffice2 = input.KelurahanPerusahaanX;
                custSiteVisit.SvOfNoTelpOffice = input.NoTeleponKantor;
                custSiteVisit.SvOfNoFaxOffice = input.NoFaxKantor;
                custSiteVisit.SvOfYearOffice = input.LamaUsahaTahun;
                custSiteVisit.SvOfUsahaOffice = input.BidangUsaha;
                custSiteVisit.SvOfStafOffice = input.JumlahKaryawan;
                custSiteVisit.SvOfScaleOffice = input.SkalaPerusahaan;
                custSiteVisit.SvOfBangunanOffice = input.JenisBangunan;
                custSiteVisit.SvOfLokasiOffice = input.LokasiBangunan;
                custSiteVisit.SvOfKondisiOffice = input.KondisiBangunan;
                custSiteVisit.SvOfOwnerOffice = input.StatusKepemilikan;
                custSiteVisit.SvOfNameWork = input.JenisPekerjaan;
                custSiteVisit.SvOfPosisiWork = input.Jabatan;
                custSiteVisit.SvOfYearWork = input.LamaBekerjaTahun;
                custSiteVisit.SvOfMonthWork = input.LamaBekerjaBulan;
                custSiteVisit.SvOfStatusWork = input.StatusKaryawan;
                custSiteVisit.SvOfUnitWork = input.UnitPekerjaan;
                custSiteVisit.SvOfKinerjaWork = input.KinerjaKaryawan;
                custSiteVisit.SvOfIncomeBrutoPemohon = input.IncomeBrutoPemohon;
                custSiteVisit.SvOfOtherIncomePemohon = input.IncomeOtherPemohon;
                custSiteVisit.SvOfPayPemohon = input.PengeluaranPemohon;
                custSiteVisit.SvOfIncomeMargin = input.IncomeMargin;
                custSiteVisit.SvHsJumlahTertanggung = input.JumlahTanggungan;

                // Pasangan

                custSiteVisit.SvHsCoupleName1 = input.NamaPasangan;
                custSiteVisit.SvHsCoupleJob = input.JenisPekerjaanPasangan;
                custSiteVisit.SvOfTotalIncomeSpouse = input.IncomeTotalPasangan;

                // Keluarga

                custSiteVisit.SvHsNamaFamily1 = input.NamaKeluarga;
                custSiteVisit.SvHsAddressFamily1 = input.AlamatKeluarga;
                custSiteVisit.SvHsZipCodeFamily = input.KodePosKeluarga;
                // custSiteVisit.SvHsCityCodeFamily = input.PropinsiKeluargaX;
                // custSiteVisit.SvHsCityFamily = input.KotaKeluargaX;
                custSiteVisit.SvHsNoTelpFamily = input.NoTeleponKeluarga;
                custSiteVisit.SvHsNoHpFamily = input.NoHpKeluarga;
                custSiteVisit.SvHsHubFamily = input.HubunganKeluarga;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /*
        // PUT: api/SiteVisitKantor/Pekerjaan/5
        [Route("Pekerjaan/{apRegno}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPekerjaan(string apRegno, [FromBody] SiteVisitKantorPekerjaan input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

            if (custSiteVisit == null)
            {
                return NotFound();
            }

            try
            {
                custSiteVisit.SvOfInvestigasiDate = input.TanggalInvestigasi;
                custSiteVisit.SvOfPemberiKet1 = input.NamaPemberiKeterangan1;
                custSiteVisit.SvOfPosisiPemberiKet1 = input.HubunganPemberiKeterangan1;
                custSiteVisit.SvOfPemberiKet2 = input.NamaPemberiKeterangan2;
                custSiteVisit.SvOfPosisiPemberiKet2 = input.HubunganPemberiKeterangan2;
                custSiteVisit.SvOfTypeOffice = input.TipePerusahaan;
                custSiteVisit.SvOfNamaOffice = input.NamaPerusahaan;
                custSiteVisit.SvOfAddressOffice1 = input.AlamatPerusahaan;
                // custSiteVisit.SvOfCityCodeOffice = input.PropinsiPerusahaanX;
                // custSiteVisit.SvOfCityOffice = input.KotaPerusahaanX;
                // custSiteVisit.SvOfAddressOffice3 = input.KecamatanPerusahaanX;
                // custSiteVisit.SvOfAddressOffice2 = input.KelurahanPerusahaanX;
                custSiteVisit.SvOfNoTelpOffice = input.NoTeleponKantor;
                custSiteVisit.SvOfNoFaxOffice = input.NoFaxKantor;
                custSiteVisit.SvOfYearOffice = input.LamaUsahaTahun;
                custSiteVisit.SvOfUsahaOffice = input.BidangUsaha;
                custSiteVisit.SvOfStafOffice = input.JumlahKaryawan;
                custSiteVisit.SvOfScaleOffice = input.SkalaPerusahaan;
                custSiteVisit.SvOfBangunanOffice = input.JenisBangunan;
                custSiteVisit.SvOfLokasiOffice = input.LokasiBangunan;
                custSiteVisit.SvOfKondisiOffice = input.KondisiBangunan;
                custSiteVisit.SvOfOwnerOffice = input.StatusKepemilikan;
                custSiteVisit.SvOfNameWork = input.JenisPekerjaan;
                custSiteVisit.SvOfPosisiWork = input.Jabatan;
                custSiteVisit.SvOfYearWork = input.LamaBekerjaTahun;
                custSiteVisit.SvOfMonthWork = input.LamaBekerjaBulan;
                custSiteVisit.SvOfStatusWork = input.StatusKaryawan;
                custSiteVisit.SvOfUnitWork = input.UnitPekerjaan;
                custSiteVisit.SvOfKinerjaWork = input.KinerjaKaryawan;
                custSiteVisit.SvOfIncomeBrutoPemohon = input.IncomeBrutoPemohon;
                custSiteVisit.SvOfOtherIncomePemohon = input.IncomeOtherPemohon;
                custSiteVisit.SvOfPayPemohon = input.PengeluaranPemohon;
                custSiteVisit.SvOfIncomeMargin = input.IncomeMargin;
                custSiteVisit.SvHsJumlahTertanggung = input.JumlahTanggungan;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        /*
        // PUT: api/SiteVisitKantor/Pasangan/5
        [Route("Pasangan/{apRegno}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPasangan(string apRegno, [FromBody] SiteVisitKantorPasangan input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

            if (custSiteVisit == null)
            {
                return NotFound();
            }

            try
            {
                custSiteVisit.ApRegno = input.ApRegno;
                custSiteVisit.CuRef = input.CuRef;
                custSiteVisit.SvHsCoupleName1 = input.NamaPasangan;
                custSiteVisit.SvHsCoupleJob = input.JenisPekerjaanPasangan;
                custSiteVisit.SvOfTotalIncomeSpouse = input.IncomeTotalPasangan;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        /*
        // PUT: api/SiteVisitKantor/Keluarga/5
        [Route("Keluarga/{apRegno}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKeluarga(string apRegno, [FromBody] SiteVisitKantorKeluarga input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(apRegno);

            if (custSiteVisit == null)
            {
                return NotFound();
            }

            try
            {
                custSiteVisit.ApRegno = input.ApRegno;
                custSiteVisit.CuRef = input.CuRef;
                custSiteVisit.SvHsNamaFamily1 = input.NamaKeluarga;
                custSiteVisit.SvHsAddressFamily1 = input.AlamatKeluarga;
                custSiteVisit.SvHsZipCodeFamily = input.KodePosKeluarga;
                // custSiteVisit.SvHsCityCodeFamily = input.PropinsiKeluargaX;
                // custSiteVisit.SvHsCityFamily = input.KotaKeluargaX;
                custSiteVisit.SvHsNoTelpFamily = input.NoTeleponKeluarga;
                custSiteVisit.SvHsNoHpFamily = input.NoHpKeluarga;
                custSiteVisit.SvHsHubFamily = input.HubunganKeluarga;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        // POST: api/SiteVisitKantor
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(SiteVisitKantor input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(input.ApRegno);

            if (custSiteVisit != null)
            {
                return BadRequest("Record has already exists!");
            }

            try
            {
                Application application = db.Applications.Find(input.ApRegno);

                if (application == null)
                {
                    return BadRequest("Application No is not found!");
                }

                string cuRef = application.CuRef;

                CustSiteVisit newCustSiteVisit = new CustSiteVisit();

                newCustSiteVisit.ApRegno = input.ApRegno;
                newCustSiteVisit.CuRef = cuRef;

                newCustSiteVisit.SvOfInvestigasiDate = input.TanggalInvestigasi;
                newCustSiteVisit.SvOfPemberiKet1 = input.NamaPemberiKeterangan1;
                newCustSiteVisit.SvOfPosisiPemberiKet1 = input.HubunganPemberiKeterangan1;
                newCustSiteVisit.SvOfPemberiKet2 = input.NamaPemberiKeterangan2;
                newCustSiteVisit.SvOfPosisiPemberiKet2 = input.HubunganPemberiKeterangan2;
                newCustSiteVisit.SvOfTypeOffice = input.TipePerusahaan;
                newCustSiteVisit.SvOfNamaOffice = input.NamaPerusahaan;
                newCustSiteVisit.SvOfAddressOffice1 = input.AlamatPerusahaan;
                // newCustSiteVisit.SvOfCityCodeOffice = input.PropinsiPerusahaanX;
                // newCustSiteVisit.SvOfCityOffice = input.KotaPerusahaanX;
                // newCustSiteVisit.SvOfAddressOffice3 = input.KecamatanPerusahaanX;
                // newCustSiteVisit.SvOfAddressOffice2 = input.KelurahanPerusahaanX;
                newCustSiteVisit.SvOfNoTelpOffice = input.NoTeleponKantor;
                newCustSiteVisit.SvOfNoFaxOffice = input.NoFaxKantor;
                newCustSiteVisit.SvOfYearOffice = input.LamaUsahaTahun;
                newCustSiteVisit.SvOfUsahaOffice = input.BidangUsaha;
                newCustSiteVisit.SvOfStafOffice = input.JumlahKaryawan;
                newCustSiteVisit.SvOfScaleOffice = input.SkalaPerusahaan;
                newCustSiteVisit.SvOfBangunanOffice = input.JenisBangunan;
                newCustSiteVisit.SvOfLokasiOffice = input.LokasiBangunan;
                newCustSiteVisit.SvOfKondisiOffice = input.KondisiBangunan;
                newCustSiteVisit.SvOfOwnerOffice = input.StatusKepemilikan;
                newCustSiteVisit.SvOfNameWork = input.JenisPekerjaan;
                newCustSiteVisit.SvOfPosisiWork = input.Jabatan;
                newCustSiteVisit.SvOfYearWork = input.LamaBekerjaTahun;
                newCustSiteVisit.SvOfMonthWork = input.LamaBekerjaBulan;
                newCustSiteVisit.SvOfStatusWork = input.StatusKaryawan;
                newCustSiteVisit.SvOfUnitWork = input.UnitPekerjaan;
                newCustSiteVisit.SvOfKinerjaWork = input.KinerjaKaryawan;
                newCustSiteVisit.SvOfIncomeBrutoPemohon = input.IncomeBrutoPemohon;
                newCustSiteVisit.SvOfOtherIncomePemohon = input.IncomeOtherPemohon;
                newCustSiteVisit.SvOfPayPemohon = input.PengeluaranPemohon;
                newCustSiteVisit.SvOfIncomeMargin = input.IncomeMargin;
                newCustSiteVisit.SvHsJumlahTertanggung = input.JumlahTanggungan;

                // Pasangan

                newCustSiteVisit.SvHsCoupleName1 = input.NamaPasangan;
                newCustSiteVisit.SvHsCoupleJob = input.JenisPekerjaanPasangan;
                newCustSiteVisit.SvOfTotalIncomeSpouse = input.IncomeTotalPasangan;

                // Keluarga

                newCustSiteVisit.ApRegno = input.ApRegno;
                newCustSiteVisit.CuRef = input.CuRef;
                newCustSiteVisit.SvHsNamaFamily1 = input.NamaKeluarga;
                newCustSiteVisit.SvHsAddressFamily1 = input.AlamatKeluarga;
                newCustSiteVisit.SvHsZipCodeFamily = input.KodePosKeluarga;
                // newCustSiteVisit.SvHsCityCodeFamily = input.PropinsiKeluargaX;
                // newCustSiteVisit.SvHsCityFamily = input.KotaKeluargaX;
                newCustSiteVisit.SvHsNoTelpFamily = input.NoTeleponKeluarga;
                newCustSiteVisit.SvHsNoHpFamily = input.NoHpKeluarga;
                newCustSiteVisit.SvHsHubFamily = input.HubunganKeluarga;

                db.CustSiteVisits.Add(newCustSiteVisit);
                db.SaveChanges();

                return Ok();

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /*
        // POST: api/SiteVisitKantor/Pekerjaan
        [Route("Pekerjaan")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostPekerjaan(SiteVisitKantorPekerjaan input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(input.ApRegno);

            if (custSiteVisit != null)
            {
                return BadRequest("Record has already exists!");
            }

            try
            {
                Application application = db.Applications.Find(input.ApRegno);

                if (application == null)
                {
                    return BadRequest("Application No is not found!");
                }

                string cuRef = application.CuRef;

                CustSiteVisit newCustSiteVisit = new CustSiteVisit();

                newCustSiteVisit.ApRegno = input.ApRegno;
                newCustSiteVisit.CuRef = cuRef;

                newCustSiteVisit.SvOfInvestigasiDate = input.TanggalInvestigasi;
                newCustSiteVisit.SvOfPemberiKet1 = input.NamaPemberiKeterangan1;
                newCustSiteVisit.SvOfPosisiPemberiKet1 = input.HubunganPemberiKeterangan1;
                newCustSiteVisit.SvOfPemberiKet2 = input.NamaPemberiKeterangan2;
                newCustSiteVisit.SvOfPosisiPemberiKet2 = input.HubunganPemberiKeterangan2;
                newCustSiteVisit.SvOfTypeOffice = input.TipePerusahaan;
                newCustSiteVisit.SvOfNamaOffice = input.NamaPerusahaan;
                newCustSiteVisit.SvOfAddressOffice1 = input.AlamatPerusahaan;
                // newCustSiteVisit.SvOfCityCodeOffice = input.PropinsiPerusahaanX;
                // newCustSiteVisit.SvOfCityOffice = input.KotaPerusahaanX;
                // newCustSiteVisit.SvOfAddressOffice3 = input.KecamatanPerusahaanX;
                // newCustSiteVisit.SvOfAddressOffice2 = input.KelurahanPerusahaanX;
                newCustSiteVisit.SvOfNoTelpOffice = input.NoTeleponKantor;
                newCustSiteVisit.SvOfNoFaxOffice = input.NoFaxKantor;
                newCustSiteVisit.SvOfYearOffice = input.LamaUsahaTahun;
                newCustSiteVisit.SvOfUsahaOffice = input.BidangUsaha;
                newCustSiteVisit.SvOfStafOffice = input.JumlahKaryawan;
                newCustSiteVisit.SvOfScaleOffice = input.SkalaPerusahaan;
                newCustSiteVisit.SvOfBangunanOffice = input.JenisBangunan;
                newCustSiteVisit.SvOfLokasiOffice = input.LokasiBangunan;
                newCustSiteVisit.SvOfKondisiOffice = input.KondisiBangunan;
                newCustSiteVisit.SvOfOwnerOffice = input.StatusKepemilikan;
                newCustSiteVisit.SvOfNameWork = input.JenisPekerjaan;
                newCustSiteVisit.SvOfPosisiWork = input.Jabatan;
                newCustSiteVisit.SvOfYearWork = input.LamaBekerjaTahun;
                newCustSiteVisit.SvOfMonthWork = input.LamaBekerjaBulan;
                newCustSiteVisit.SvOfStatusWork = input.StatusKaryawan;
                newCustSiteVisit.SvOfUnitWork = input.UnitPekerjaan;
                newCustSiteVisit.SvOfKinerjaWork = input.KinerjaKaryawan;
                newCustSiteVisit.SvOfIncomeBrutoPemohon = input.IncomeBrutoPemohon;
                newCustSiteVisit.SvOfOtherIncomePemohon = input.IncomeOtherPemohon;
                newCustSiteVisit.SvOfPayPemohon = input.PengeluaranPemohon;
                newCustSiteVisit.SvOfIncomeMargin = input.IncomeMargin;
                newCustSiteVisit.SvHsJumlahTertanggung = input.JumlahTanggungan;

                db.CustSiteVisits.Add(newCustSiteVisit);
                db.SaveChanges();

                return Ok();

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        /*
        // POST: api/SiteVisitKantor/Pasangan
        [Route("Pasangan")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostPasangan(SiteVisitKantorPasangan input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(input.ApRegno);

            if (custSiteVisit != null)
            {
                return BadRequest("Record has already exists!");
            }

            try
            {
                Application application = db.Applications.Find(input.ApRegno);

                if (application == null)
                {
                    return BadRequest("Application No is not found!");
                }

                string cuRef = application.CuRef;

                CustSiteVisit newCustSiteVisit = new CustSiteVisit();

                newCustSiteVisit.ApRegno = input.ApRegno;
                newCustSiteVisit.CuRef = cuRef;

                newCustSiteVisit.SvHsCoupleName1 = input.NamaPasangan;
                newCustSiteVisit.SvHsCoupleJob = input.JenisPekerjaanPasangan;
                newCustSiteVisit.SvOfTotalIncomeSpouse = input.IncomeTotalPasangan;

                db.CustSiteVisits.Add(newCustSiteVisit);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */

        /*
        // POST: api/SiteVisitKantor/Keluarga
        [Route("Keluarga")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PostKeluarga(SiteVisitKantorKeluarga input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustSiteVisit custSiteVisit = db.CustSiteVisits.Find(input.ApRegno);

            if (custSiteVisit != null)
            {
                return BadRequest("Record has already exists!");
            }

            try
            {
                Application application = db.Applications.Find(input.ApRegno);

                if (application == null)
                {
                    return BadRequest("Application No is not found!");
                }

                string cuRef = application.CuRef;

                CustSiteVisit newCustSiteVisit = new CustSiteVisit();

                newCustSiteVisit.ApRegno = input.ApRegno;
                newCustSiteVisit.CuRef = cuRef;

                newCustSiteVisit.ApRegno = input.ApRegno;
                newCustSiteVisit.CuRef = input.CuRef;
                newCustSiteVisit.SvHsNamaFamily1 = input.NamaKeluarga;
                newCustSiteVisit.SvHsAddressFamily1 = input.AlamatKeluarga;
                newCustSiteVisit.SvHsZipCodeFamily = input.KodePosKeluarga;
                // newCustSiteVisit.SvHsCityCodeFamily = input.PropinsiKeluargaX;
                // newCustSiteVisit.SvHsCityFamily = input.KotaKeluargaX;
                newCustSiteVisit.SvHsNoTelpFamily = input.NoTeleponKeluarga;
                newCustSiteVisit.SvHsNoHpFamily = input.NoHpKeluarga;
                newCustSiteVisit.SvHsHubFamily = input.HubunganKeluarga;

                db.CustSiteVisits.Add(newCustSiteVisit);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        */
    }
}
