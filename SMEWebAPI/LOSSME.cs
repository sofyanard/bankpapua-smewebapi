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

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<Nasabah> Nasabahs { get; set; }
        public virtual DbSet<NasabahUpload> NasabahUploads { get; set; }
        public virtual DbSet<Pengajuan> Pengajuans { get; set; }
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
        public virtual DbSet<RfProduct> RfProducts { get; set; }
        public virtual DbSet<RfLoanPurpose> RfLoanPurposes { get; set; }
        public virtual DbSet<RfCollateralType> RfCollateralTypes { get; set; }
        public virtual DbSet<RfCertType> RfCertTypes { get; set; }
        public virtual DbSet<RfArea> RfAreas { get; set; }
        public virtual DbSet<RfCity> RfCities { get; set; }
        public virtual DbSet<RfBranch> RfBranches { get; set; }
        public virtual DbSet<RfTrack> RfTracks { get; set; }
        public virtual DbSet<RfApplicationType> RfApplicationTypes { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<TrackHistory> TrackHistories { get; set; }
        public virtual DbSet<ScGroup> ScGroups { get; set; }
        public virtual DbSet<ScUser> ScUsers { get; set; }
        public virtual DbSet<CustSiteVisit> CustSiteVisits { get; set; }
        public virtual DbSet<DocUploadFileUpload> DocUploadFileUploads { get; set; }
        public virtual DbSet<ListAssignment> ListAssignments { get; set; }
        public virtual DbSet<AppraisalResultNew> AppraisalResultNews { get; set; }
        public virtual DbSet<RfApprMarketability> RfApprMarketabilities { get; set; }
        public virtual DbSet<RfApprIkatSempurna> RfApprIkatSempurnas { get; set; }
        public virtual DbSet<RfApprKuasa> RfApprKuasas { get; set; }
        public virtual DbSet<RfApprMasalah> RfApprMasalahs { get; set; }
        public virtual DbSet<AppraisalNewFileUpload> AppraisalNewFileUploads { get; set; }
        public virtual DbSet<RfApprJenisBangunan> RfApprJenisBangunans { get; set; }
        public virtual DbSet<RfApprLokasiBangunan> RfApprLokasiBangunans { get; set; }
        public virtual DbSet<RfApprPenggunaanBangunan> RfApprPenggunaanBangunans { get; set; }
        public virtual DbSet<RfApprKelengkapan> RfApprKelengkapans { get; set; }
        public virtual DbSet<RfApprJalan> RfApprJalans { get; set; }
        public virtual DbSet<AppLifeInsurance> AppLifeInsurances { get; set; }
        public virtual DbSet<AppCredAsurance> AppCredAsurances { get; set; }
        public virtual DbSet<AppColAsurance> AppColAsurances { get; set; }
        public virtual DbSet<RfCurrency> RfCurrencies { get; set; }
        public virtual DbSet<RfGroupInsuranceType> RfGroupInsuranceTypes { get; set; }
        public virtual DbSet<RfInsuranceType> RfInsuranceTypes { get; set; }
        public virtual DbSet<RfPekerjaan> RfPekerjaans { get; set; }
        public virtual DbSet<NotaryAssign> NotaryAssigns { get; set; }
        public virtual DbSet<NotaryAssignDetail> NotaryAssignDetails { get; set; }
        public virtual DbSet<RfCompanyType> RfCompanyTypes { get; set; }
        public virtual DbSet<RfBusinessType> RfBusinessTypes { get; set; }
        public virtual DbSet<RfJenisBangunan> RfJenisBangunans { get; set; }
        public virtual DbSet<RfApprUmur> RfApprUmurs { get; set; }
        public virtual DbSet<RfRating> RfRatings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
