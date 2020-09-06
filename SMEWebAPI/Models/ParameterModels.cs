namespace SMEWebAPI.Models
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
}