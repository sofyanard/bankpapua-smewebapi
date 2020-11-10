namespace SMEWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspNetUsers")]
    public class AspNetUser
    {
        [Key]
        public string Id { get; set; }

        public string Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool? LockoutEnabled { get; set; }

        public int? AccessFailedCount { get; set; }

        public string UserName { get; set; }
    }

    [Table("AspNetRoles")]
    public class AspNetRole
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    [Table("AspNetUserRoles")]
    public class AspNetUserRole
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUser AspNetUser { get; set; }

        [Key]
        [Column(Order = 2)]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual AspNetRole AspNetRole { get; set; }
    }

    public class RoleViewModel
    {
        public string RoleName { get; set; }
    }
}