using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entity
{
    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int TypeId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RawAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debtor { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Creditor { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("Transactions")]
        public virtual Employee Employee { get; set; } = null!;
        [ForeignKey("TypeId")]
        [InverseProperty("Transactions")]
        public virtual TransactionType Type { get; set; } = null!;
    }
}
