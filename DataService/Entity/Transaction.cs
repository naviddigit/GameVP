using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? ProductUserId { get; set; }
        public int SiteId { get; set; }
        [StringLength(10)]
        public string? DocumentNo { get; set; }
        public int? ProductId { get; set; }
        public int AccountId { get; set; }
        public int? AccountIdParent { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Date { get; set; }
        [StringLength(150)]
        public string? Description { get; set; }
        public int TransactionTypeId { get; set; }
        public int? CurrencyId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RawAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Changerate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debtor { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Creditor { get; set; }
        [Required]
        public bool? Settell { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SettellDate { get; set; }

        [ForeignKey("AccountId")]
        [InverseProperty("Transactions")]
        public virtual Account Account { get; set; } = null!;
        [ForeignKey("CurrencyId")]
        [InverseProperty("Transactions")]
        public virtual Currency? Currency { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Transactions")]
        public virtual Product? Product { get; set; }
        [ForeignKey("TransactionTypeId")]
        [InverseProperty("Transactions")]
        public virtual TransactionType TransactionType { get; set; } = null!;
    }
}
