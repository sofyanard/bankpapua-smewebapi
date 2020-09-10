namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pengajuan")]
    public class Pengajuan
    {
        [Key]
        public int Id { get; set; }

        // [Required]
        public int NasabahId { get; set; }

        [Required]
        [StringLength(10)]
        public string Product { get; set; }

        [ForeignKey("Product")]
        public virtual RfProduct RfProduct { get; set; }

        [Required]
        public double Limit { get; set; }

        [Required]
        public int Tenor { get; set; }

        [StringLength(10)]
        public string Purpose { get; set; }

        [ForeignKey("Purpose")]
        public virtual RfLoanPurpose RfLoanPurpose { get; set; }

        [Required]
        [StringLength(1)]
        public string CollateralFlag { get; set; }
        // 1 = With Collateral
        // 0 = No Collateral

        public int? CollateralType { get; set; }

        [ForeignKey("CollateralType")]
        public virtual RfCollateralType RfCollateralType { get; set; }

        public double? CollateralValue { get; set; }

        [StringLength(10)]
        public string CertificateType { get; set; }

        [ForeignKey("CertificateType")]
        public virtual RfCertType RfCertType { get; set; }

        [StringLength(50)]
        public string CertificateNo { get; set; }

        [Required]
        [StringLength(10)]
        public string BranchCode { get; set; }

        [StringLength(20)]
        public string LosApRegno { get; set; }
    }
}