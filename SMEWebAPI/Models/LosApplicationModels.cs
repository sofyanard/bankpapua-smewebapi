namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRACKHISTORY")]
    public class TrackHistory
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("APPTYPE", Order = 2)]
        public string AppType { get; set; }

        [ForeignKey("AppType")]
        public virtual RfApplicationType RfApplicationType { get; set; }

        [Key]
        [Column("PRODUCTID", Order = 3)]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual RfProduct RfProduct { get; set; }

        [Key]
        [Column("PROD_SEQ", Order = 4)]
        public int ProdSeq { get; set; }

        [Key]
        [Column("TH_SEQ", Order = 5)]
        public int ThSeq { get; set; }

        [Column("TRACKCODE")]
        public string TrackCode { get; set; }

        [ForeignKey("TrackCode")]
        public virtual RfTrack RfTrack { get; set; }

        [Column("TH_TRACKDATE")]
        public DateTime? TrackDate { get; set; }
    }
}