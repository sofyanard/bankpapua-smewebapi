﻿namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RefPropinsi")]
    public class RefPropinsi
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    [Table("RefKotaKab")]
    public class RefKotaKab
    {
        [Key]
        public string Id { get; set; }

        public string PropId { get; set; }

        [ForeignKey("PropId")]
        public virtual RefPropinsi RefPropinsi { get; set; }

        public string Name { get; set; }
    }

    [Table("RefKecamatan")]
    public class RefKecamatan
    {
        [Key]
        public string Id { get; set; }

        public string KotaId { get; set; }

        [ForeignKey("KotaId")]
        public virtual RefKotaKab RefKotaKab { get; set; }

        public string Name { get; set; }
    }

    [Table("RefKelurahan")]
    public class RefKelurahan
    {
        [Key]
        public string Id { get; set; }

        public string KecId { get; set; }

        [ForeignKey("KecId")]
        public virtual RefKecamatan RefKecamatan { get; set; }

        public string Name { get; set; }
    }

    [Table("RFSEX")]
    public class RfSex
    {
        [Key]
        [Column("SEXID")]
        public string SexId { get; set; }

        [Column("SEXDESC")]
        public string SexDesc { get; set; }
    }

    [Table("RFEDUCATION")]
    public class RfEducation
    {
        [Key]
        [Column("EDUCATIONID")]
        public string EducationId { get; set; }

        [Column("EDUCATIONDESC")]
        public string EducationDesc { get; set; }
    }

    [Table("RFMARITAL")]
    public class RfMarital
    {
        [Key]
        [Column("MARITALID")]
        public string MaritalId { get; set; }

        [Column("MARITALDESC")]
        public string MaritalDesc { get; set; }
    }

    [Table("RFCITIZENSHIP")]
    public class RfCitizenship
    {
        [Key]
        [Column("CITIZENID")]
        public string CitizenId { get; set; }

        [Column("CITIZENDESC")]
        public string CitizenDesc { get; set; }
    }

    [Table("RFHOMESTA")]
    public class RfHomeStatus
    {
        [Key]
        [Column("HM_CODE")]
        public string HomeStatusId { get; set; }

        [Column("HM_DESC")]
        public string HomeStatusDesc { get; set; }
    }

    [Table("RFJOBTITLE")]
    public class RfJobTitle
    {
        [Key]
        [Column("JOBTITLEID")]
        public string JobTitleId { get; set; }

        [Column("JOBTITLEDESC")]
        public string JobTitleDesc { get; set; }
    }

    [Table("RFRELATIONSHIP")]
    public class RfRelationship
    {
        [Key]
        [Column("RELTYPEID")]
        public string RelationshipId { get; set; }

        [Column("RELTYPEDESC")]
        public string RelationshipDesc { get; set; }
    }

    [Table("RFPRODUCT")]
    public class RfProduct
    {
        [Key]
        [Column("PRODUCTID")]
        public string ProductId { get; set; }

        [Column("PRODUCTDESC")]
        public string ProductDesc { get; set; }

        [Column("ISSUBAPPPROD")]
        public string IsSubAppProd { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFLOANPURPOSE")]
    public class RfLoanPurpose
    {
        [Key]
        [Column("LOANPURPID")]
        public string LoanPurpId { get; set; }

        [Column("LOANPURPDESC")]
        public string LoanPurpDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFCOLLATERALTYPE")]
    public class RfCollateralType
    {
        [Key]
        [Column("COLTYPESEQ")]
        public int ColTypeSeq { get; set; }

        [Column("COLTYPEID")]
        public string ColTypeId { get; set; }

        [Column("COLTYPEDESC")]
        public string ColTypeDesc { get; set; }

        [Column("COLLINKTABLE")]
        public string ColLinkTable { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFCERTTYPE")]
    public class RfCertType
    {
        [Key]
        [Column("CERTTYPEID")]
        public string CertTypeId { get; set; }

        [Column("CERTTYPEDESC")]
        public string CertTypeDesc { get; set; }

        [Column("COLFLAG")]
        public string ColFlag { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFAREA")]
    public class RfArea
    {
        [Key]
        [Column("AREAID")]
        public string AreaId { get; set; }

        [Column("AREANAME")]
        public string AreaName { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFCITY")]
    public class RfCity
    {
        [Key]
        [Column("CITYID")]
        public string CityId { get; set; }

        [Column("AREAID")]
        public string AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual RfArea RfArea { get; set; }

        [Column("CITYNAME")]
        public string CityName { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFBRANCH")]
    public class RfBranch
    {
        [Key]
        [Column("BRANCH_CODE")]
        public string BranchCode { get; set; }

        [Column("CITYID")]
        public string CityId { get; set; }

        [ForeignKey("CityId")]
        public virtual RfCity RfCity { get; set; }

        [Column("BRANCH_NAME")]
        public string BranchName { get; set; }

        [Column("BR_ISBRANCH")]
        public string IsBranch { get; set; }

        [Column("BRANCH_TYPE")]
        public string BranchType { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFTRACK")]
    public class RfTrack
    {
        [Key]
        [Column("TRACKCODE")]
        public string TrackCode { get; set; }

        [Column("TRACKNAME")]
        public string TrackName { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFAPPLICATIONTYPE")]
    public class RfApplicationType
    {
        [Key]
        [Column("APPTYPEID")]
        public string AppTypeId { get; set; }

        [Column("APPTYPEDESC")]
        public string AppTypeDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_MARKETABILITY")]
    public class RfApprMarketability
    {
        [Key]
        [Column("APPR_MRCODE")]
        public string ApprMrCode { get; set; }

        [Column("APPR_MRDESC")]
        public string ApprMrDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_IKATSEMPURNA")]
    public class RfApprIkatSempurna
    {
        [Key]
        [Column("APPR_IKSCODE")]
        public string ApprIksCode { get; set; }

        [Column("APPR_IKSDESC")]
        public string ApprIksDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_KUASA")]
    public class RfApprKuasa
    {
        [Key]
        [Column("APPR_KUCODE")]
        public string ApprKuCode { get; set; }

        [Column("APPR_KUDESC")]
        public string ApprKuDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_MASALAH")]
    public class RfApprMasalah
    {
        [Key]
        [Column("APPR_PMCODE")]
        public string ApprPmCode { get; set; }

        [Column("APPR_PMDESC")]
        public string ApprPmDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_JENISBANGUNAN")]
    public class RfApprJenisBangunan
    {
        [Key]
        [Column("BANGUNANID")]
        public string BangunanId { get; set; }

        [Column("BANGUNANDESC")]
        public string BangunanDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_LOKASIBANGUNAN")]
    public class RfApprLokasiBangunan
    {
        [Key]
        [Column("LOKASIID")]
        public string LokasiId { get; set; }

        [Column("LOKASIDESC")]
        public string LokasiDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_PENGGUNAANBANGUNAN")]
    public class RfApprPenggunaanBangunan
    {
        [Key]
        [Column("GUNAID")]
        public string GunaId { get; set; }

        [Column("GUNADESC")]
        public string GunaDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_KELENGKAPAN")]
    public class RfApprKelengkapan
    {
        [Key]
        [Column("LENGKAPID")]
        public string LengkapId { get; set; }

        [Column("LENGKAPDESC")]
        public string LengkapDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_JALAN")]
    public class RfApprJalan
    {
        [Key]
        [Column("JALANID")]
        public string JalanId { get; set; }

        [Column("JALANDESC")]
        public string JalanDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFCURRENCY")]
    public class RfCurrency
    {
        [Key]
        [Column("CURRENCYID")]
        public string CurrencyId { get; set; }

        [Column("CURRENCYDESC")]
        public string CurrencyDesc { get; set; }

        [Column("CURRENCYRATE")]
        public double? CurrencyRate { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFGROUPINSURANCETYPE")]
    public class RfGroupInsuranceType
    {
        [Key]
        [Column("GRIT_ID")]
        public string GroupInsuranceTypeId { get; set; }

        [Column("GRIT_DESC")]
        public string GroupInsuranceTypeDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFINSURANCETYPE")]
    public class RfInsuranceType
    {
        [Key]
        [Column("IT_ID")]
        public string InsuranceTypeId { get; set; }

        [Column("IT_DESC")]
        public string InsuranceTypeDesc { get; set; }

        [Column("GRIT_ID")]
        public string GroupInsuranceTypeId { get; set; }

        [ForeignKey("GroupInsuranceTypeId")]
        public virtual RfGroupInsuranceType RfGroupInsuranceType { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFPEKERJAAN")]
    public class RfPekerjaan
    {
        [Key]
        [Column("PEKERJAANID")]
        public string PekerjaanId { get; set; }

        [Column("PEKERJAANDESC")]
        public string PekerjaanDesc { get; set; }

        [Column("PEKERJAANSLA")]
        public int? SlaDays { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFCOMPTYPE")]
    public class RfCompanyType
    {
        [Key]
        [Column("COMPTYPEID")]
        public string CompanyTypeId { get; set; }

        [Column("COMPTYPEDESC")]
        public string CompanyTypeDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFBUSINESSTYPE")]
    public class RfBusinessType
    {
        [Key]
        [Column("BUSSTYPEID")]
        public string BussTypeId { get; set; }

        [Column("BUSSTYPEDESC")]
        public string BussTypeDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFJENISBANGUNAN")]
    public class RfJenisBangunan
    {
        [Key]
        [Column("BANGUNANID")]
        public string BangunanId { get; set; }

        [Column("BANGUNANDESC")]
        public string BangunanDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RF_APPR_UMUR")]
    public class RfApprUmur
    {
        [Key]
        [Column("UMURID")]
        public string UmurId { get; set; }

        [Column("UMURDESC")]
        public string UmurDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }

    [Table("RFRATING")]
    public class RfRating
    {
        [Key]
        [Column("RATEID")]
        public string RateId { get; set; }

        [Column("RATEDESC")]
        public string RateDesc { get; set; }

        [Column("ACTIVE")]
        public string Active { get; set; }
    }
}