using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("Currency")]
    public partial class Currency
    {
        public Currency()
        {
            Products = new HashSet<Product>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Column("symbol")]
        [StringLength(10)]
        public string? Symbol { get; set; }
        [Column("chainId")]
        public int? ChainId { get; set; }

        [InverseProperty("Currency")]
        public virtual ICollection<Product> Products { get; set; }
        [InverseProperty("Currency")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
