namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Nasabah")]
    public class Nasabah
    {
        [Key]
        public int Id { get; set; }

        // [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string GelarSebelum { get; set; }

        [Required]
        [StringLength(50)]
        public string NamaLengkap { get; set; }

        [StringLength(50)]
        public string GelarSesudah { get; set; }

        [Required]
        [StringLength(10)]
        public string JenisKelamin { get; set; }

        [ForeignKey("JenisKelamin")]
        public virtual RfSex RfSex { get; set; }

        [Required]
        [StringLength(50)]
        public string TempatLahir { get; set; }

        [Required]
        public DateTime? TanggalLahir { get; set; }

        [Required]
        [StringLength(50)]
        public string NoIdentitas { get; set; }

        [Required]
        [StringLength(100)]
        public string AlamatRumah { get; set; }

        [Required]
        [StringLength(10)]
        public string PropinsiRumah { get; set; }

        [ForeignKey("PropinsiRumah")]
        public virtual RefPropinsi RefPropinsiRumah { get; set; }

        [Required]
        [StringLength(10)]
        public string KotaKabRumah { get; set; }

        [ForeignKey("KotaKabRumah")]
        public virtual RefKotaKab RefKotaKabRumah { get; set; }

        [StringLength(50)]
        public string KecamatanRumah { get; set; }

        [ForeignKey("KecamatanRumah")]
        public virtual RefKecamatan RefKecamatanRumah { get; set; }

        [StringLength(50)]
        public string KelurahanRumah { get; set; }

        [ForeignKey("KelurahanRumah")]
        public virtual RefKelurahan RefKelurahanRumah { get; set; }

        [Required]
        [StringLength(10)]
        public string KodePosRumah { get; set; }

        [StringLength(50)]
        public string TeleponRumah { get; set; }

        [Required]
        [StringLength(50)]
        public string TeleponGenggam { get; set; }

        [Required]
        [StringLength(50)]
        public string NamaIbuKandung { get; set; }

        [StringLength(10)]
        public string Pendidikan { get; set; }

        [ForeignKey("Pendidikan")]
        public virtual RfEducation RfEducation { get; set; }

        [StringLength(10)]
        public string StatusPerkawinan { get; set; }

        [ForeignKey("StatusPerkawinan")]
        public virtual RfMarital RfMarital { get; set; }

        [StringLength(10)]
        public string Kewarganegaraan { get; set; }

        [ForeignKey("Kewarganegaraan")]
        public virtual RfCitizenship RfCitizenship { get; set; }

        [StringLength(10)]
        public string StatusRumah { get; set; }

        [ForeignKey("StatusRumah")]
        public virtual RfHomeStatus RfHomeStatus { get; set; }



        [StringLength(10)]
        public string JenisPekerjaan { get; set; }

        [ForeignKey("JenisPekerjaan")]
        public virtual RfJobTitle RfJobTitle { get; set; }

        public double? Pendapatan { get; set; }

        [StringLength(100)]
        public string AlamatKantor { get; set; }

        [StringLength(50)]
        public string TeleponKantor { get; set; }

        [StringLength(10)]
        public string PropinsiKantor { get; set; }

        [ForeignKey("PropinsiKantor")]
        public virtual RefPropinsi RefPropinsiKantor { get; set; }

        [StringLength(10)]
        public string KotaKabKantor { get; set; }

        [ForeignKey("KotaKabKantor")]
        public virtual RefKotaKab RefKotaKabKantor { get; set; }

        [StringLength(10)]
        public string KodePosKantor { get; set; }



        [StringLength(50)]
        public string NamaSaudara { get; set; }

        [StringLength(100)]
        public string AlamatSaudara { get; set; }

        [StringLength(10)]
        public string PropinsiSaudara { get; set; }

        [ForeignKey("PropinsiSaudara")]
        public virtual RefPropinsi RefPropinsiSaudara { get; set; }

        [StringLength(10)]
        public string KotaKabSaudara { get; set; }

        [ForeignKey("KotaKabSaudara")]
        public virtual RefKotaKab RefKotaKabSaudara { get; set; }

        [StringLength(10)]
        public string KodePosSaudara { get; set; }

        [StringLength(10)]
        public string HubunganSaudara { get; set; }

        [ForeignKey("HubunganSaudara")]
        public virtual RfRelationship RfRelationship { get; set; }
    }

    public class NasabahPekerjaan
    {
        [Key]
        public int Id { get; set; }

        [StringLength(10)]
        public string JenisPekerjaan { get; set; }

        [ForeignKey("JenisPekerjaan")]
        public virtual RfJobTitle RfJobTitle { get; set; }

        public double? Pendapatan { get; set; }

        [StringLength(100)]
        public string AlamatKantor { get; set; }

        [StringLength(50)]
        public string TeleponKantor { get; set; }

        [StringLength(10)]
        public string PropinsiKantor { get; set; }

        [ForeignKey("PropinsiKantor")]
        public virtual RefPropinsi RefPropinsiKantor { get; set; }

        [StringLength(10)]
        public string KotaKabKantor { get; set; }

        [ForeignKey("KotaKabKantor")]
        public virtual RefKotaKab RefKotaKabKantor { get; set; }

        [StringLength(10)]
        public string KodePosKantor { get; set; }
    }

    public class NasabahSaudara
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string NamaSaudara { get; set; }

        [StringLength(100)]
        public string AlamatSaudara { get; set; }

        [StringLength(10)]
        public string PropinsiSaudara { get; set; }

        [ForeignKey("PropinsiSaudara")]
        public virtual RefPropinsi RefPropinsiSaudara { get; set; }

        [StringLength(10)]
        public string KotaKabSaudara { get; set; }

        [ForeignKey("KotaKabSaudara")]
        public virtual RefKotaKab RefKotaKabSaudara { get; set; }

        [StringLength(10)]
        public string KodePosSaudara { get; set; }

        [StringLength(10)]
        public string HubunganSaudara { get; set; }

        [ForeignKey("HubunganSaudara")]
        public virtual RfRelationship RfRelationship { get; set; }
    }
}
