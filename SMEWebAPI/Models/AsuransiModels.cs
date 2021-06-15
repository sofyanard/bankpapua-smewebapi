using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMEWebAPI.Models
{
    public class AsuransiJiwaPipeline
    {
        public string ApRegno { get; set; }
        public int Seq { get; set; }
        public string CustomerName { get; set; }
        public string OfficerName { get; set; }
        public string InsuranceType { get; set; }
    }

    public class AsuransiKreditPipeline
    {
        public string ApRegno { get; set; }
        public string AppType { get; set; }
        public string ProductId { get; set; }
        public int ProductSeq { get; set; }
        public int Seq { get; set; }
        public string CustomerName { get; set; }
        public string OfficerName { get; set; }
        public string InsuranceType { get; set; }
    }

    public class AsuransiAgunanPipeline
    {
        public string ApRegno { get; set; }
        public string CuRef { get; set; }
        public string ProductId { get; set; }
        public int CollateralSeq { get; set; }
        public int Seq { get; set; }
        public string CustomerName { get; set; }
        public string OfficerName { get; set; }
        public string InsuranceType { get; set; }
    }

    [Table("APPLIFEINSURANCE")]
    public class AppLifeInsurance
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("SEQ", Order = 2)]
        public int Seq { get; set; }

        [Column("IC_ID")]
        public string InsuranceCompanyId { get; set; }

        [Column("IT_ID")]
        public string InsuranceTypeId { get; set; }

        [Column("ALI_AMOUNT")]
        public double? InsuranceAmount { get; set; }

        [Column("ALI_CUR")]
        public string CurrencyId { get; set; }

        [Column("ALI_DURATION")]
        public int? Duration { get; set; }

        [Column("ALI_PERCENTAGE")]
        public int? Percentage { get; set; }

        [Column("ALI_PREMI")]
        public double? Premi { get; set; }

        [Column("ALI_ICRATE")]
        public string RateId { get; set; }

        [Column("ALI_DATESTART")]
        public DateTime? DateStart { get; set; }

        [Column("ALI_DATEEND")]
        public DateTime? DateEnd { get; set; }

        [Column("ALI_POLICYNO")]
        public string PolicyNo { get; set; }
    }

    [Table("APPCREDASURANCE")]
    public class AppCredAsurance
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Required]
        [Column("CU_REF")]
        public string CuRef { get; set; }

        [Key]
        [Column("APPTYPE", Order = 2)]
        public string AppType { get; set; }

        [Key]
        [Column("PRODUCTID", Order = 3)]
        public string ProductId { get; set; }

        [Key]
        [Column("PROD_SEQ", Order = 4)]
        public int ProductSeq { get; set; }

        [Key]
        [Column("SEQ", Order = 5)]
        public int Seq { get; set; }

        [Column("IC_ID")]
        public string InsuranceCompanyId { get; set; }

        [Column("IT_ID")]
        public string InsuranceTypeId { get; set; }

        [Column("ACR_AMOUNT")]
        public double? InsuranceAmount { get; set; }

        [Column("ACR_CUR")]
        public string CurrencyId { get; set; }

        [Column("ACR_DURATION")]
        public int? Duration { get; set; }

        [Column("ACR_PERCENTAGE")]
        public int? Percentage { get; set; }

        [Column("ACR_PREMI")]
        public double? Premi { get; set; }

        [Column("ACR_ICRATE")]
        public string RateId { get; set; }

        [Column("ACR_DATESTART")]
        public DateTime? DateStart { get; set; }

        [Column("ACR_DATEEND")]
        public DateTime? DateEnd { get; set; }

        [Column("ACR_POLICYNO")]
        public string PolicyNo { get; set; }

        [Column("ICT_LEAD")]
        public string IctLead { get; set; }

        [Column("ICT_ID")]
        public string IctId { get; set; }
    }

    [Table("APPCOLASURANCE")]
    public class AppColAsurance
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Required]
        [Column("CU_REF")]
        public string CuRef { get; set; }

        [Key]
        [Column("PRODUCTID", Order = 2)]
        public string ProductId { get; set; }

        [Key]
        [Column("Cl_SEQ", Order = 3)]
        public int CollateralSeq { get; set; }

        [Key]
        [Column("SEQ", Order = 4)]
        public int Seq { get; set; }

        [Column("IC_ID")]
        public string InsuranceCompanyId { get; set; }

        [Column("IT_ID")]
        public string InsuranceTypeId { get; set; }

        [Column("ACA_AMOUNT")]
        public double? InsuranceAmount { get; set; }

        [Column("ACA_CUR")]
        public string CurrencyId { get; set; }

        [Column("ACA_DURATION")]
        public int? Duration { get; set; }

        [Column("ACA_PERCENTAGE")]
        public int? Percentage { get; set; }

        [Column("ACA_PREMI")]
        public double? Premi { get; set; }

        [Column("ACA_ICRATE")]
        public string RateId { get; set; }

        [Column("ACA_DATESTART")]
        public DateTime? DateStart { get; set; }

        [Column("ACA_DATEEND")]
        public DateTime? DateEnd { get; set; }

        [Column("ACA_POLICYNO")]
        public string PolicyNo { get; set; }

        [Column("ICT_LEAD")]
        public string IctLead { get; set; }

        [Column("ICT_ID")]
        public string IctId { get; set; }

        [Column("BROKER_ID")]
        public string BrokerId { get; set; }

        [Column("ACA_AMOUNT_BANG")]
        public double? AmountBangunan { get; set; }

        [Column("ACA_AMOUNT_MESIN")]
        public double? AmountMesin { get; set; }

        [Column("ACA_AMOUNT_LAIN")]
        public double? AmountLain { get; set; }

        [Column("ACA_PREMI_DIBAYAR")]
        public double? PremiDibayar { get; set; }

        [Column("ACA_PREMIDATE")]
        public DateTime? PremiDate { get; set; }

        [Column("ACA_ORDERNO")]
        public string OrderNo { get; set; }

        [Column("ACA_ORDERDATE")]
        public DateTime? OrderDate { get; set; }

        [Column("ACA_COVERNO")]
        public string CoverNo { get; set; }

        [Column("ACA_COVERDATE")]
        public DateTime? CoverDate { get; set; }

        [Column("ACA_COVERDUEDATE")]
        public DateTime? CoverDueDate { get; set; }

        [Column("ACA_POLICYDATE")]
        public DateTime? PolicyDate { get; set; }

        [Column("ACA_CLASS")]
        public string ClassId { get; set; }
    }

    public class AsuransiJiwaMobile
    {
        [Required]
        public string ApRegno { get; set; }

        [Required]
        public int Seq { get; set; }

        [Required]
        public string InsuranceCompanyId { get; set; }

        [Required]
        public string InsuranceTypeId { get; set; }

        public double? InsuranceAmount { get; set; }

        public string CurrencyId { get; set; }

        public int? Percentage { get; set; }

        public double? Premi { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string PolicyNo { get; set; }
    }

    public class AsuransiKreditMobile
    {
        [Required]
        public string ApRegno { get; set; }

        [Required]
        public string CuRef { get; set; }

        [Required]
        public string AppType { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int ProductSeq { get; set; }

        [Required]
        public int Seq { get; set; }

        [Required]
        public string InsuranceCompanyId { get; set; }

        [Required]
        public string InsuranceTypeId { get; set; }

        public double? InsuranceAmount { get; set; }

        public string CurrencyId { get; set; }

        public int? Percentage { get; set; }

        public double? Premi { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string PolicyNo { get; set; }
    }

    public class AsuransiAgunanMobile
    {
        [Required]
        public string ApRegno { get; set; }

        [Required]
        public string CuRef { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int CollateralSeq { get; set; }

        [Required]
        public int Seq { get; set; }

        [Required]
        public string InsuranceCompanyId { get; set; }

        [Required]
        public string InsuranceTypeId { get; set; }

        public double? InsuranceAmount { get; set; }

        public string CurrencyId { get; set; }

        public int? Percentage { get; set; }

        public double? Premi { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string PolicyNo { get; set; }
    }

    public class AsuransiInfoCustomer
    {
        public string ApRegno { get; set; }

        public string BranchName { get; set; }

        public string AccountOfficer { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerCity { get; set; }

        public string HomePhoneNo { get; set; }

        public string MobilePhoneNo { get; set; }

        public string ContactPerson { get; set; }
    }

    public class AsuransiInfoCollateral
    {
        public string ApRegno { get; set; }

        public string CuRef { get; set; }

        public string ClSeq { get; set; }

        public string Description { get; set; }

        public string CollateralType { get; set; }

        public string Address { get; set; }

        public string Location { get; set; }

        public string Owner { get; set; }

        public string CertificateType { get; set; }

        public string CertificateNo { get; set; }
    }

    public class AsuransiInfoFacility
    {
        public string ApRegno { get; set; }

        public string AppType { get; set; }

        public string ProductId { get; set; }

        public string ProductSeq { get; set; }

        public string ProductDesc { get; set; }

        public double? Limit { get; set; }

        public int? Tenor { get; set; }

        public string LoanPurpose { get; set; }

        public DateTime? PkDate { get; set; }

        public string PkNo { get; set; }
    }
}