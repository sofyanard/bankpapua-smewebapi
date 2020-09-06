namespace SMEWebAPI
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using SMEWebAPI.Models;

    public partial class LOSSME : DbContext
    {
        public LOSSME()
            : base("name=LOSSME")
        {
        }

        public virtual DbSet<Nasabah> Nasabahs { get; set; }
        public virtual DbSet<RefPropinsi> RefPropinsis { get; set; }
        public virtual DbSet<RefKotaKab> RefKotaKabs { get; set; }
        public virtual DbSet<RefKecamatan> RefKecamatans { get; set; }
        public virtual DbSet<RefKelurahan> RefKelurahans { get; set; }
        public virtual DbSet<RfSex> RfSexs { get; set; }
        public virtual DbSet<RfEducation> RfEducations { get; set; }
        public virtual DbSet<RfMarital> RfMaritals { get; set; }
        public virtual DbSet<RfCitizenship> RfCitizenships { get; set; }
        public virtual DbSet<RfHomeStatus> RfHomeStatuses { get; set; }
        public virtual DbSet<RfJobTitle> RfJobTitles { get; set; }
        public virtual DbSet<RfRelationship> RfRelationships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
