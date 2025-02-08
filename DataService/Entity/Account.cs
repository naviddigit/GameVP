using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    public partial class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
            Users = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [StringLength(50)]
        public string? DisplayName { get; set; }
        public int? RoleId { get; set; }
        [StringLength(50)]
        public string? Username { get; set; }
        [StringLength(500)]
        public string? Password { get; set; }
        [StringLength(200)]
        public string? Token { get; set; }
        public bool Active { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        [StringLength(15)]
        public string? Mobile { get; set; }
        public int CountFailed { get; set; }
        public int CountFailedLimit { get; set; }
        public int? ParentId { get; set; }
        [StringLength(100)]
        public string? AvatarUrl { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CreditorLimit { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("Accounts")]
        public virtual AccountRole? Role { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<Transaction> Transactions { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<User> Users { get; set; }
    }
}
