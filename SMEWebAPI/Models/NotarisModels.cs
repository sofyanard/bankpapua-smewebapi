﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMEWebAPI.Models
{
    public class NotarisPipeline
    {
        public string ApRegno { get; set; }
        public int Seq { get; set; }
        public string CustomerName { get; set; }
        public string OfficerName { get; set; }
    }

    [Table("NOTARYASSIGN")]
    public class NotaryAssign
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("SEQ", Order = 2)]
        public int Seq { get; set; }

        [Column("NTID")]
        public string NotaryId { get; set; }

        [Column("NA_APPNTDATETIME")]
        public DateTime? AppnDateTime { get; set; }

        [Column("NA_REMARKS")]
        public string Remarks { get; set; }

        [Column("CU_REF")]
        public string CuRef { get; set; }

        [Column("CL_SEQ")]
        public int? CollateralSeq { get; set; }

        [Column("NA_COVERNO")]
        public string CoverNo { get; set; }

        [Column("NA_COVERDATE")]
        public DateTime? CoverDate { get; set; }

        [Column("NA_COVERDUEDATE")]
        public DateTime? CoverDueDate { get; set; }

        [Column("NA_ORDERNO")]
        public string OrderNo { get; set; }

        [Column("NA_ORDERDATE")]
        public DateTime? OrderDate { get; set; }

        [Column("NA_PKDATE")]
        public DateTime? PkDate { get; set; }

        public virtual ICollection<NotaryAssignDetail> NotaryAssignDetails { get; set; }
    }

    [Table("NOTARYASSIGN_DETAIL")]
    public class NotaryAssignDetail
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("SEQ", Order = 2)]
        public int Seq { get; set; }

        [Key]
        [Column("SUBSEQ", Order = 3)]
        public int SubSeq { get; set; }

        [Column("NA_FINISHDATE")]
        public DateTime? FinishDate { get; set; }

        [Column("ITEMPEKERJAAN")]
        public string PekerjaanId { get; set; }

        [ForeignKey("PekerjaanId")]
        public virtual RfPekerjaan RfPekerjaan { get; set; }
    }

    public class NotaryAssignMobile
    {
        [Required]
        public string ApRegno { get; set; }

        [Required]
        public int Seq { get; set; }

        public string OrderNo { get; set; }

        public DateTime? OrderDate { get; set; }

        public string Remarks { get; set; }
    }

    public class NotarisInfoCustomer
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

    public class NotarisInfoCollateral
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

    public class NotarisInfoFacility
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