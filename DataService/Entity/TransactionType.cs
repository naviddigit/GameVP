using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("TransactionType")]
    public partial class TransactionType
    {
        public TransactionType()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string? Class { get; set; }
        [StringLength(20)]
        public string? Icon { get; set; }
        public int? SortOrder { get; set; }
        public int? GroupId { get; set; }

        [InverseProperty("TransactionType")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
