using ProjectLayer.Models.Stucture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLayer.Models.Mod
{
    public class MServer
    {
        public object? List()
        {
            return (DataService.Entity.Server?)new Repository.RServer().List(null);
        }
        public object? ListIp()
        {
            List<DataService.Entity.Server> ipList = new Repository.RServer().List();

            return ipList.Select(i => i.Ip);
        }
        public object? List(string token, string username)
        {
            var accountIssu = (DataService.Entity.Account?)new Repository.RAccount().GetByToken(token);
            var account = (DataService.Entity.Account?)new Repository.RAccount().Get(username);

            if (account.ParentId != accountIssu.Id && account.RoleId > 2)
            {
                return null;
            }


            var productUserList = (List<DataService.Entity.ProductUser>?)new Repository.RProductUser().List(account.Id);

            return productUserList.Select(i => new
            {
                value = i.Id,
                title = i.Product.Name,
                staticPrice = i.StaticPrice,
                description = Structure.Publics.CurrencyFormat2(i.StaticPrice, " Toman", 0),
                disabled = i.Active,
                username = username
            });
        }
    }


}
