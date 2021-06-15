using System;
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
}