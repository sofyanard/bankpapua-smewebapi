namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NasabahUpload")]
    public class NasabahUpload
    {
        [Key]
        public int Id { get; set; }

        public int NasabahId { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(50)]
        public string Caption { get; set; }
    }

    [NotMapped]
    public class NasabahUploadView : NasabahUpload
    {
        public string DownloadUrl { get; set; }

        public NasabahUploadView(NasabahUpload nasabahUpload)
        {
            this.Id = nasabahUpload.Id;
            this.NasabahId = nasabahUpload.NasabahId;
            this.FileName = nasabahUpload.FileName;
            this.Caption = nasabahUpload.Caption;
        }
    }
}