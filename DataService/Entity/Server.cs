using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataService.Entity
{
    [Table("Server")]
    public partial class Server
    {
        public Server()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("vpnserver_host")]
        [StringLength(50)]
        public string? VpnserverHost { get; set; }
        [Column("vpnserver_port")]
        public int? VpnserverPort { get; set; }
        [Column("admin_password")]
        [StringLength(50)]
        public string? AdminPassword { get; set; }
        [Column("hub_name")]
        [StringLength(50)]
        public string? HubName { get; set; }
        [Required]
        [Column("active")]
        public bool? Active { get; set; }
        [Column("limitUserActive")]
        public int LimitUserActive { get; set; }
        [Column("lastUpdate", TypeName = "datetime")]
        public DateTime? LastUpdate { get; set; }
        [Column("ip")]
        [StringLength(20)]
        public string? Ip { get; set; }

        [InverseProperty("Server")]
        public virtual ICollection<User> Users { get; set; }
    }
}
