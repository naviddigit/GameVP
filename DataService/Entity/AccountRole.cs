using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("AccountRole")]
    public partial class AccountRole
    {
        public AccountRole()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
