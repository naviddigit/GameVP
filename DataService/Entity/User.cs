using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("User")]
    public partial class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int? AccountId { get; set; }
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [StringLength(50)]
        public string Password { get; set; } = null!;
        public int ServerId { get; set; }
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExpirationDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RenewDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column("S_online")]
        public bool SOnline { get; set; }
        [Column("S_download_bytes", TypeName = "decimal(18, 0)")]
        public decimal SDownloadBytes { get; set; }
        public int? ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updated { get; set; }
        [Column("UserId_IBSng")]
        public int? UserIdIbsng { get; set; }
        [StringLength(250)]
        public string? Comment { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }
        public bool Banned { get; set; }
        [StringLength(150)]
        public string? BannedText { get; set; }
        public int LimitUser { get; set; }

        [ForeignKey("AccountId")]
        [InverseProperty("Users")]
        public virtual Account? Account { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Users")]
        public virtual Product? Product { get; set; }
        [ForeignKey("ServerId")]
        [InverseProperty("Users")]
        public virtual Server Server { get; set; } = null!;
    }
}
