namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SCGROUP")]
    public class ScGroup
    {
        [Key]
        [Column("GROUPID")]
        public string GroupId { get; set; }

        [Column("SG_GRPNAME")]
        public string GroupName { get; set; }

        [Column("SG_ACTIVE")]
        public string Active { get; set; }
    }

    [Table("SCUSER")]
    public class ScUser
    {
        [Key]
        [Column("USERID")]
        public string UserId { get; set; }

        [Column("SU_FULLNAME")]
        public string FullName { get; set; }

        [Column("GROUPID")]
        public string GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual ScGroup ScGroup { get; set; }

        [Column("SU_BRANCH")]
        public string BranchId { get; set; }

        [ForeignKey("BranchId")]
        public virtual RfBranch RfBranch { get; set; }

        [Column("SU_EMAIL")]
        public string Email { get; set; }

        [Column("SU_ACTIVE")]
        public string Active { get; set; }
    }
}