using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMEWebAPI.Models
{
    public class AppraisalPipeLine
    {
        public string ApRegno { get; set; }
        public string CuRef { get; set; }
        public int ClSeq { get; set; }
        public string CustomerName { get; set; }
        public string CollateralType { get; set; }
        public string CollateralDesc { get; set; }
        public String AppraisalStatus { get; set; }
        public DateTime? AppraisalDate { get; set; }
    }

    [Table("LISTASSIGNMENT")]
    public class ListAssignment
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("CU_REF", Order = 2)]
        public string CuRef { get; set; }

        [Key]
        [Column("CL_SEQ", Order = 3)]
        public int ClSeq { get; set; }

        [Column("LA_CREDITOPR")]
        public string LaCreditOpr { get; set; }

        [Column("LA_ASSIGNDATE")]
        public DateTime LaAssignDate { get; set; }

        [Column("AGENCYID")]
        public string AgencyId { get; set; }

        [Column("OFFICERSEQ")]
        public string OfficerSeq { get; set; }

        [Column("LA_COMPLETEDATE")]
        public DateTime LaCompleteDate { get; set; }

        [Column("LA_APPRSTATUS")]
        public string LaApprStatus { get; set; }

        [Column("LA_APPRTYPE")]
        public string LaApprType { get; set; }

        [Column("LA_COASSIGNDATE")]
        public DateTime LaCoAssignDate { get; set; }

        [Column("LA_AGENCYRATING")]
        public string LaAgencyRating { get; set; }

        [Column("LA_APPRKHUSUS")]
        public string LaApprKhusus { get; set; }

        [Column("LA_REMARKTOBU")]
        public string LaRemarkToBu { get; set; }
    }

    [Table("APPRAISAL_RESULT_NEW")]
    public class AppraisalResultNew
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("CU_REF", Order = 2)]
        public string CuRef { get; set; }

        [Key]
        [Column("CL_SEQ", Order = 3)]
        public int ClSeq { get; set; }

        [Column("APPR_DATE")]
        public DateTime? ApprDate { get; set; }

        [Column("APPR_CURRENCYID")]
        public string ApprCurrencyId { get; set; }

        [Column("APPR_LOCATIONID")]
        public string ApprLocationId { get; set; }

        [Column("APPR_VALUEACCORDINGID")]
        public string ApprValueAccordingId { get; set; }

        [Column("APPR_JENISAGUNANID")]
        public string ApprJenisAgunanId { get; set; }

        [Column("APPR_VALUEBANK")]
        public double ApprValueBank { get; set; }

        [Column("APPR_VALUEPASAR")]
        public double ApprValuePasar { get; set; }

        [Column("APPR_VALUELIKUIDASI")]
        public double ApprValueLikuidasi { get; set; }

        [Column("APPR_SAFETYMARGIN")]
        public double ApprSafetyMargin { get; set; }

        [Column("APPR_SCORE")]
        public double ApprScore { get; set; }

        [Column("APPR_MRCODE")]
        public string ApprMrCode { get; set; }

        [Column("APPR_IKSCODE")]
        public string ApprIksCode { get; set; }

        [Column("APPR_KUCODE")]
        public string ApprKuCode { get; set; }

        [Column("APPR_PMCODE")]
        public string ApprPmCode { get; set; }
    }

    public class AppraisalMobile
    {
        [Required]
        public string ApRegno { get; set; }

        [Required]
        public string CuRef { get; set; }

        [Required]
        public int ClSeq { get; set; }

        public DateTime? ApprDate { get; set; }

        public double ApprValueBank { get; set; }

        public double ApprValuePasar { get; set; }

        public double ApprValueLikuidasi { get; set; }

        public string ApprMarketabilityCode { get; set; }

        [ForeignKey("ApprMarketabilityCode")]
        public virtual RfApprMarketability RfApprMarketability { get; set; }

        public string ApprIkatSempurnaCode { get; set; }

        [ForeignKey("ApprIkatSempurnaCode")]
        public virtual RfApprIkatSempurna RfApprIkatSempurna { get; set; }

        public string ApprKuasaCode { get; set; }

        [ForeignKey("ApprKuasaCode")]
        public virtual RfApprKuasa RfApprKuasa { get; set; }

        public string ApprMasalahCode { get; set; }

        [ForeignKey("ApprMasalahCode")]
        public virtual RfApprMasalah RfApprMasalah { get; set; }
    }

    [Table("APPRAISALNEW_FILEUPLOAD")]
    public class AppraisalNewFileUpload
    {
        [Key]
        [Column("AP_REGNO", Order = 1)]
        public string ApRegno { get; set; }

        [Key]
        [Column("CU_REF", Order = 2)]
        public string CuRef { get; set; }

        [Key]
        [Column("CL_SEQ", Order = 3)]
        public int ClSeq { get; set; }

        [Key]
        [Column("FU_SEQ", Order = 4)]
        public int FuSeq { get; set; }

        [Column("FU_FILENAME")]
        public string FuFileName { get; set; }

        [Column("FU_DATE")]
        public DateTime FuDate { get; set; }

        [Column("FU_USERID")]
        public string FuUserId { get; set; }
    }

    [NotMapped]
    public class AppraisalNewFileUploadView : AppraisalNewFileUpload
    {
        public string DownloadUrl { get; set; }

        public AppraisalNewFileUploadView(AppraisalNewFileUpload appraisalNewFileUpload)
        {
            this.ApRegno = appraisalNewFileUpload.ApRegno;
            this.CuRef = appraisalNewFileUpload.CuRef;
            this.ClSeq = appraisalNewFileUpload.ClSeq;
            this.FuSeq = appraisalNewFileUpload.FuSeq;
            this.FuFileName = appraisalNewFileUpload.FuFileName;
            this.FuDate = appraisalNewFileUpload.FuDate;
            this.FuUserId = appraisalNewFileUpload.FuUserId;
        }
    }
}