using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entity
{
    [Table("Employee")]
    public partial class Employee
    {
        public Employee()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalaryBase { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
