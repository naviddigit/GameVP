using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            ProductUsers = new HashSet<ProductUser>();
            Transactions = new HashSet<Transaction>();
            Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        [Column("Group_IBSng")]
        [StringLength(50)]
        public string? GroupIbsng { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public int CurrencyId { get; set; }
        public int Stock { get; set; }
        [Column("x")]
        public int? X { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("Products")]
        public virtual Currency Currency { get; set; } = null!;
        [InverseProperty("Product")]
        public virtual ICollection<ProductUser> ProductUsers { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Transaction> Transactions { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<User> Users { get; set; }
    }
}
