using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("ProductUser")]
    public partial class ProductUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("accountId")]
        public int AccountId { get; set; }
        [Column("productId")]
        public int ProductId { get; set; }
        [Column("pesent", TypeName = "decimal(5, 2)")]
        public decimal Pesent { get; set; }
        [Column("staticPrice", TypeName = "decimal(18, 0)")]
        public decimal StaticPrice { get; set; }
        [Required]
        [Column("active")]
        public bool? Active { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductUsers")]
        public virtual Product Product { get; set; } = null!;
    }
}
