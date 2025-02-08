using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entity
{
    [Table("TransactionType")]
    public partial class TransactionType
    {
        public TransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [InverseProperty("Type")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
