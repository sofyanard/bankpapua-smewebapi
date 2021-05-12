using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMEWebAPI.Models
{
    public class SiteVisitPipeLine
    {
        public string ApRegno { get; set; }
        public string CuRef { get; set; }
        public string CustomerName { get; set; }
        public String SiteVisitStatus { get; set; }
        public DateTime? SiteVisitDate { get; set; }
    }

    [Table("CUST_SITEVISIT")]
    public class CustSiteVisit
    {
        [Key]
        [Column("AP_REGNO")]
        public string ApRegno { get; set; }

        [Column("CU_REF")]
        public string CuRef { get; set; }

        [Column("SV_DATE")]
        public DateTime? SvDate { get; set; }

        [Column("SV_NAME")]
        public string SvName { get; set; }

        [Column("SV_TUJUAN")]
        public string SvTujuan { get; set; }

        [Column("SV_NASABAH")]
        public string SvNasabah { get; set; }

        [Column("SV_BANK")]
        public string SvBank { get; set; }

        [Column("SV_OFFICE")]
        public string SvOffice { get; set; }

        [Column("SV_FACTORY")]
        public string SvFactory { get; set; }

        [Column("SV_MANAGEMENT")]
        public string SvManagement { get; set; }

        [Column("SV_PRODUKSI")]
        public string SvProduksi { get; set; }

        [Column("SV_PEMASARAN")]
        public string SvPemasaran { get; set; }

        [Column("SV_KEUANGAN")]
        public string SvKeuangan { get; set; }

        [Column("SV_AGUNAN")]
        public string SvAgunan { get; set; }

        [Column("SV_PERSOALAN")]
        public string SvPersoalan { get; set; }

        [Column("TG_DATE")]
        public DateTime? TgDate { get; set; }

        [Column("SV_TARGETDATE")]
        public string SvTargetDate { get; set; }

        [Column("SV_HS_INVESTIGASI_DATE")]
        public DateTime? SvHsInvestigasiDate { get; set; }

        [Column("SV_HS_PEMBERI_KET1")]
        public string SvHsPemberiKet1 { get; set; }

        [Column("SV_HS_HUB_PEMBERI_KET1")]
        public string SvHsHubPemberiKet1 { get; set; }

        [Column("SV_HS_PEMBERI_KET2")]
        public string SvHsPemberiKet2 { get; set; }

        [Column("SV_HS_HUB_PEMBERI_KET2")]
        public string SvHsHubPemberiKet2 { get; set; }

        [Column("SV_HS_NM_PEMOHON1")]
        public string SvHsNamaPemohon1 { get; set; }

        [Column("SV_HS_NM_PEMOHON2")]
        public string SvHsNamaPemohon2 { get; set; }

        [Column("SV_HS_NM_PEMOHON3")]
        public string SvHsNamaPemohon3 { get; set; }

        [Column("SV_HS_BIRTH_DATE")]
        public DateTime? SvHsBirthDate { get; set; }

        [Column("SV_HS_SEX_TYP")]
        public string SvHsSexType { get; set; }

        [Column("SV_HS_EMAIL")]
        public string SvHsEmail { get; set; }

        [Column("SV_HS_ADDR_KTP1")]
        public string SvHsAddressKtp1 { get; set; }

        [Column("SV_HS_ADDR_KTP2")]
        public string SvHsAddressKtp2 { get; set; }

        [Column("SV_HS_ADDR_KTP3")]
        public string SvHsAddressKtp3 { get; set; }

        [Column("SV_HS_ZIP_KTP")]
        public string SvHsZipCodeKtp { get; set; }

        [Column("SV_HS_CITY_CODE_KTP")]
        public string SvHsCityCodeKtp { get; set; }

        [Column("SV_HS_CITY_KTP")]
        public string SvHsCityKtp { get; set; }

        [Column("SV_HS_ADDR1")]
        public string SvHsAddress1 { get; set; }

        [Column("SV_HS_ADDR2")]
        public string SvHsAddress2 { get; set; }

        [Column("SV_HS_ADDR3")]
        public string SvHsAddress3 { get; set; }

        [Column("SV_HS_ZIP")]
        public string SvHsZipCode { get; set; }

        [Column("SV_HS_CITY_CODE")]
        public string SvHsCityCode { get; set; }

        [Column("SV_HS_CITY")]
        public string SvHsCity { get; set; }

        [Column("SV_HS_NO_TLP_AREA")]
        public string SvHsTelpArea { get; set; }

        [Column("SV_HS_NO_TLP")]
        public string SvHsTelp { get; set; }

        [Column("SV_HS_NO_HP_AREA")]
        public string SvHsNoHpArea { get; set; }

        [Column("SV_HS_NO_HP")]
        public string SvHsNoHp { get; set; }

        [Column("SV_HS_MARRIAGE")]
        public string SvHsMarriage { get; set; }

        [Column("SV_HS_COUPLE_NM1")]
        public string SvHsCoupleName1 { get; set; }

        [Column("SV_HS_COUPLE_NM2")]
        public string SvHsCoupleName2 { get; set; }

        [Column("SV_HS_COUPLE_NM3")]
        public string SvHsCoupleName3 { get; set; }

        [Column("SV_HS_COUPLE_JOB")]
        public string SvHsCoupleJob { get; set; }

        [Column("SV_HS_MOTHER_NM1")]
        public string SvHsMotherName1 { get; set; }

        [Column("SV_HS_MOTHER_NM2")]
        public string SvHsMotherName2 { get; set; }

        [Column("SV_HS_MOTHER_NM3")]
        public string SvHsMotherName3 { get; set; }

        [Column("SV_HS_JML_TERTANGGUNG")]
        public string SvHsJumlahTertanggung { get; set; }

        [Column("SV_HS_DEBITUR_TYP")]
        public string SvHsDebiturType { get; set; }

        [Column("SV_HS_ADDR_PLUS1")]
        public string SvHsAddressPlus1 { get; set; }

        [Column("SV_HS_ADDR_PLUS2")]
        public string SvHsAddressPlus2 { get; set; }

        [Column("SV_HS_ADDR_PLUS3")]
        public string SvHsAddressPlus3 { get; set; }

        [Column("SV_HS_ZIP_PLUS")]
        public string SvHsZipCodePlus { get; set; }

        [Column("SV_HS_CITY_CODE_PLUS")]
        public string SvHsCityCodePlus { get; set; }

        [Column("SV_HS_CITY_PLUS")]
        public string SvHsCityPlus { get; set; }

        [Column("SV_HS_NO_TLP_AREA_PLUS")]
        public string SvHsNoTelpAreaPlus { get; set; }

        [Column("SV_HS_NO_TLP_PLUS")]
        public string SvHsNoTelpPlus { get; set; }

        [Column("SV_HS_NO_HP_AREA_PLUS")]
        public string SvHsNoHpAreaPlus { get; set; }

        [Column("SV_HS_NO_HP_PLUS")]
        public string SvHsNoHpPlus { get; set; }

        [Column("SV_HS_NM_FAMS1")]
        public string SvHsNamaFamily1 { get; set; }

        [Column("SV_HS_NM_FAMS2")]
        public string SvHsNamaFamily2 { get; set; }

        [Column("SV_HS_NM_FAMS3")]
        public string SvHsNamaFamily3 { get; set; }

        [Column("SV_HS_ADDR_FAMS1")]
        public string SvHsAddressFamily1 { get; set; }

        [Column("SV_HS_ADDR_FAMS2")]
        public string SvHsAddressFamily2 { get; set; }

        [Column("SV_HS_ADDR_FAMS3")]
        public string SvHsAddressFamily3 { get; set; }

        [Column("SV_HS_ZIP_FAMS")]
        public string SvHsZipCodeFamily { get; set; }

        [Column("SV_HS_CITY_CODE_FAMS")]
        public string SvHsCityCodeFamily { get; set; }

        [Column("SV_HS_CITY_FAMS")]
        public string SvHsCityFamily { get; set; }

        [Column("SV_HS_NO_TLP_AREA_FAMS")]
        public string SvHsNoTelpAreaFamily { get; set; }

        [Column("SV_HS_NO_TLP_FAMS")]
        public string SvHsNoTelpFamily { get; set; }

        [Column("SV_HS_NO_OFFICE_AREA_FAMS")]
        public string SvHsNoOfficeAreaFamily { get; set; }

        [Column("SV_HS_NO_OFFICE_FAMS")]
        public string SvHsNoOfficeFamily { get; set; }

        [Column("SV_HS_NO_OFFICE_EXT_FAMS")]
        public string SvHsNoOfficeExtFamily { get; set; }

        [Column("SV_HS_NO_HP_AREA_FAMS")]
        public string SvHsNoHpAreaFamily { get; set; }

        [Column("SV_HS_NO_HP_FAMS")]
        public string SvHsNoHpFamily { get; set; }

        [Column("SV_HS_HUB_FAMS")]
        public string SvHsHubFamily { get; set; }

        [Column("SV_HS_STS_HOME_STAY")]
        public string SvHsStatusHomeStay { get; set; }

        [Column("SV_HS_CEK_ARSIP_STAY")]
        public string SvHsCekArsipStay { get; set; }

        [Column("SV_HS_AGUNAN_STAY")]
        public string SvHsAgunanStay { get; set; }

        [Column("SV_HS_DAY_STAY")]
        public string SvHsDayStay { get; set; }

        [Column("SV_HS_MONTH_STAY")]
        public string SvHsMonthStay { get; set; }

        [Column("SV_HS_BANGUNAN_TYPE_STAY")]
        public string SvHsBangunanTypeStay { get; set; }

        [Column("SV_HS_BANGUNAN_LOKASI_STAY")]
        public string SvHsBangunanLokasiStay { get; set; }

        [Column("SV_HS_BANGUNAN_COND_STAY")]
        public string SvHsBangunanCondStay { get; set; }

        [Column("SV_HS_FASILITAS_STAY")]
        public string SvHsFasilitasStay { get; set; }

        [Column("SV_HS_BARANG_HOME_STAY")]
        public string SvHsBarangHomeStay { get; set; }

        [Column("SV_HS_AKSES_ROAD_STAY")]
        public string SvHsAksesRoadStay { get; set; }

        [Column("SV_HS_LINGKUNGAN_STAY")]
        public string SvHsLingkunganStay { get; set; }

        [Column("SV_HS_LUAS_TANAH_STAY")]
        public string SvHsLuasTanahStay { get; set; }

        [Column("SV_HS_LUAS_BANGUNAN_STAY")]
        public string SvHsLuasBangunanStay { get; set; }

        [Column("SV_HS_GARASI_STAY")]
        public string SvHsGarasiStay { get; set; }

        [Column("SV_HS_CARPORT_STAY")]
        public string SvHsCarPortStay { get; set; }

        [Column("SV_HS_VEHICLE_STAY")]
        public string SvHsVehicleStay { get; set; }

        [Column("SV_HS_VEHICLE_TYPE_STAY")]
        public string SvHsVehicleTypeStay { get; set; }

        [Column("SV_HS_VEHICLE_COUNT_STAY")]
        public string SvHsVehicleCountStay { get; set; }

        [Column("SV_HS_VEHICLE_COND_STAY")]
        public string SvHsVehicleCondStay { get; set; }

        [Column("SV_HS_KETERANGAN_STAY")]
        public string SvHsKeteranganStay { get; set; }

        [Column("SV_OFC_PEMBERI_KET1")]
        public string SvOfPemberiKet1 { get; set; }

        [Column("SV_OFC_POSISI_PEMBERI_KET1")]
        public string SvOfPosisiPemberiKet1 { get; set; }

        [Column("SV_OFC_PEMBERI_KET2")]
        public string SvOfPemberiKet2 { get; set; }

        [Column("SV_OFC_POSISI_PEMBERI_KET2")]
        public string SvOfPosisiPemberiKet2 { get; set; }

        [Column("SV_OFC_TYP_OFFICE")]
        public string SvOfTypeOffice { get; set; }

        [Column("SV_OFC_NM_OFFICE")]
        public string SvOfNamaOffice { get; set; }

        [Column("SV_OFC_ADDR_OFFICE1")]
        public string SvOfAddressOffice1 { get; set; }

        [Column("SV_OFC_ADDR_OFFICE2")]
        public string SvOfAddressOffice2 { get; set; }

        [Column("SV_OFC_ADDR_OFFICE3")]
        public string SvOfAddressOffice3 { get; set; }

        [Column("SV_OFC_ZIPCODE_OFFICE")]
        public string SvOfZipCodeOffice { get; set; }

        [Column("SV_OFC_CITY_CODE_OFFICE")]
        public string SvOfCityCodeOffice { get; set; }

        [Column("SV_OFC_CITY_OFFICE")]
        public string SvOfCityOffice { get; set; }

        [Column("SV_OFC_N0_TLP_AREA_OFFICE")]
        public string SvOfNoTelpAreaOffice { get; set; }

        [Column("SV_OFC_N0_TLP_OFFICE")]
        public string SvOfNoTelpOffice { get; set; }

        [Column("SV_OFC_EXT_OFFICE")]
        public string SvOfNoTelpExtOffice { get; set; }

        [Column("SV_OFC_N0_FAX_AREA_OFFICE")]
        public string SvOfNoFaxAreaOffice { get; set; }

        [Column("SV_OFC_N0_FAX_OFFICE")]
        public string SvOfNoFaxOffice { get; set; }

        [Column("SV_OFC_YEAR_OFFICE")]
        public string SvOfYearOffice { get; set; }

        [Column("SV_OFC_USAHA_OFFICE")]
        public string SvOfUsahaOffice { get; set; }

        [Column("SV_OFC_STAF_OFFICE")]
        public string SvOfStafOffice { get; set; }

        [Column("SV_OFC_SCALE_OFFICE")]
        public string SvOfScaleOffice { get; set; }

        [Column("SV_OFC_BANGUNAN_OFFICE")]
        public string SvOfBangunanOffice { get; set; }

        [Column("SV_OFC_LOKASI_OFFICE")]
        public string SvOfLokasiOffice { get; set; }

        [Column("SV_OFC_KONDISI_OFFICE")]
        public string SvOfKondisiOffice { get; set; }

        [Column("SV_OFC_OWNER_OFFICE")]
        public string SvOfOwnerOffice { get; set; }

        [Column("SV_OFC_INVESTIGASI_DATE")]
        public DateTime? SvOfInvestigasiDate { get; set; }

        [Column("SV_OFC_TYP_OFFICE_PLUS")]
        public string SvOfTypeOfficePlus { get; set; }

        [Column("SV_OFC_NM_OFFICE_PLUS")]
        public string SvOfNamaOfficePlus { get; set; }

        [Column("SV_OFC_POSISI_PEMOHON_PLUS")]
        public string SvOfPosisiPemohonPlus { get; set; }

        [Column("SV_OFC_NO_TLP_AREA_PLUS")]
        public string SvOfNoTelpAreaPlus { get; set; }

        [Column("SV_OFC_NO_TLP_PLUS")]
        public string SvOfNoTelpPlus { get; set; }

        [Column("SV_OFC_NO_FAX_AREA_PLUS")]
        public string SvOfNoFaxAreaPlus { get; set; }

        [Column("SV_OFC_NO_FAX_PLUS")]
        public string SvOfNoFaxPlus { get; set; }

        [Column("SV_OFC_ADDR_PLUS1")]
        public string SvOfAddressPlus1 { get; set; }

        [Column("SV_OFC_ADDR_PLUS2")]
        public string SvOfAddressPlus2 { get; set; }

        [Column("SV_OFC_ADDR_PLUS3")]
        public string SvOfAddressPlus3 { get; set; }

        [Column("SV_OFC_ZIPCODE_PLUS")]
        public string SvOfZipCodePlus { get; set; }

        [Column("SV_OFC_CITY_CODE_PLUS")]
        public string SvOfCityCodePlus { get; set; }

        [Column("SV_OFC_CITY_PLUS")]
        public string SvOfCityPlus { get; set; }

        [Column("SV_OFC_NAME_WORK")]
        public string SvOfNameWork { get; set; }

        [Column("SV_OFC_POSISI_WORK")]
        public string SvOfPosisiWork { get; set; }

        [Column("SV_OFC_YEAR_WORK")]
        public string SvOfYearWork { get; set; }

        [Column("SV_OFC_MONTH_WORK")]
        public string SvOfMonthWork { get; set; }

        [Column("SV_OFC_STATUS_WORK")]
        public string SvOfStatusWork { get; set; }

        [Column("SV_OFC_UNIT_WORK")]
        public string SvOfUnitWork { get; set; }

        [Column("SV_OFC_KINERJA_WORK")]
        public string SvOfKinerjaWork { get; set; }

        [Column("SV_OFC_OFFICE_NM_HISTORY")]
        public string SvOfOfficeNameWork { get; set; }

        [Column("SV_OFC_NO_TLP_AREA_HISTORY")]
        public string SvOfNoTelpAreaHistory { get; set; }

        [Column("SV_OFC_NO_TLP_HISTORY")]
        public string SvOfNoTelpHistory { get; set; }

        [Column("SV_OFC_OFFICE_YEAR_HISTORY")]
        public string SvOfOfficeYearHistory { get; set; }

        [Column("SV_OFC_OFFICE_MONTH_HISTORY")]
        public string SvOfOfficeMonthHistory { get; set; }

        [Column("SV_OFC_INCOME_BRUTO_PEMOHON")]
        public double? SvOfIncomeBrutoPemohon { get; set; }

        [Column("SV_OFC_INCOME_NETTO_PEMOHON")]
        public double? SvOfIncomeNettoPemohon { get; set; }

        [Column("SV_OFC_OTHER_INCOME_PEMOHON")]
        public double? SvOfOtherIncomePemohon { get; set; }

        [Column("SV_OFC_TOTAL_INCOME_PEMOHON")]
        public double? SvOfTotalIncomePemohon { get; set; }

        [Column("SV_OFC_PAY_PEMOHON")]
        public double? SvOfPayPemohon { get; set; }

        [Column("SV_OFC_INCOME_BRUTO_SPOUSE")]
        public double? SvOfIncomeBrutoSpouse { get; set; }

        [Column("SV_OFC_INCOME_NETTO_SPOUSE")]
        public double? SvOfIncomeNettoSpouse { get; set; }

        [Column("SV_OFC_OTHER_INCOME_SPOUSE")]
        public double? SvOfOtherIncomeSpouse { get; set; }

        [Column("SV_OFC_TOTAL_INCOME_SPOUSE")]
        public double? SvOfTotalIncomeSpouse { get; set; }

        [Column("SV_OFC_PAY_SPOUSE")]
        public double? SvOfPaySpouse { get; set; }

        [Column("SV_OFC_INCOME_MARGIN")]
        public double? SvOfIncomeMargin { get; set; }

        [Column("SV_OFC_PEMOHON_SUMMARY")]
        public string SvOfPemohonSummary { get; set; }

        [Column("SV_OFC_OTHER_SUMMARY")]
        public string SvOfOtherSummary { get; set; }
    }

    public class SiteVisitUsaha
    {
        public string ApRegno { get; set; }
        public string CuRef { get; set; }
        public DateTime? TanggalInvestigasi { get; set; }
        public string NamaPemberiKeterangan { get; set; }
        public string TujuanKunjungan { get; set; }
        public string HasilNasabah { get; set; }
        public string HasilBank { get; set; }
        public string AlamatKantor { get; set; }
        public string AlamatPabrik { get; set; }
        public string AspekManagement { get; set; }
        public string AspekProduksi { get; set; }
        public string AspekPemasaran { get; set; }
        public string AspekKeuangan { get; set; }
        public string AspekAgunan { get; set; }
        public string Persoalan { get; set; }
        public DateTime? TanggalTarget { get; set; }
    }

    public class SiteVisitUsahaInput
    {
        [Required]
        public string ApRegno { get; set; }

        // [Required]
        public string CuRef { get; set; }

        [Required]
        public DateTime? TanggalInvestigasi { get; set; }

        public string NamaPemberiKeterangan { get; set; }

        // public string TujuanKunjungan { get; set; }
        // public string HasilNasabah { get; set; }
        // public string HasilBank { get; set; }

        public string AlamatKantor { get; set; }

        public string AlamatPabrik { get; set; }

        public string AspekManagement { get; set; }

        public string AspekProduksi { get; set; }

        public string AspekPemasaran { get; set; }

        public string AspekKeuangan { get; set; }

        public string AspekAgunan { get; set; }

        // public string Persoalan { get; set; }
        // public DateTime? TanggalTarget { get; set; }
    }
}