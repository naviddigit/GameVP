using ProjectLayer.Models.Stucture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLayer.Models.Mod
{
    public class MProductUser
    {
        public object? List(string token)
        {
            var account = (DataService.Entity.Account?)new Repository.RAccount().GetByToken(token);

            if (account?.Id == 1)
                return null;

            var productUserList = (List<DataService.Entity.ProductUser>?)new Repository.RProductUser().List(account?.Id);

            return productUserList.Select(i => new
            {
                value = i.Id,
                title = i.Product.Name,
                staticPrice = i.StaticPrice,
                description = Structure.Publics.CurrencyFormat2(i.StaticPrice, " Toman", 0),
                disabled = i.Active,
            });
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


        public object? Update(string token, string username, int productUserId, decimal staticPrice)
        {
            var accountIssu = (DataService.Entity.Account?)new Repository.RAccount().GetByToken(token);
            var account = (DataService.Entity.Account?)new Repository.RAccount().Get(username);

            if (account.ParentId != accountIssu.Id && account.RoleId > 2)
            {
                return null;
            }

            DataService.Entity.ProductUser p = (DataService.Entity.ProductUser) new Repository.RProductUser().Get(productUserId);
            var myPriceMin = new Repository.RProductUser().GetPrice(accountIssu.Id, p.ProductId);

            if (staticPrice < myPriceMin && accountIssu.RoleId > 2)
            {
                return null;
            }


            new Repository.RProductUser().Update(productUserId, (decimal)staticPrice);

            return new MResponce { Success = true };
        }


        public SProductManager? GetProductManager(string token, int producUsertId)
        {
            var account = (DataService.Entity.Account?)new Repository.RAccount().GetByToken(token);

            var productUser = (DataService.Entity.ProductUser?)new Repository.RProductUser().Get(producUsertId);

            var parentProduct =
                (account.ParentId == null || account.ParentId <= 2)
                ? null
                : (DataService.Entity.ProductUser?)new Repository.RProductUser().GetByProductId((int)productUser.ProductId, (int)account.ParentId);


            return new SProductManager
            {
                id = productUser.ProductId,

                name = productUser.Product.Name,
                Group_IBSng = productUser.Product.GroupIbsng,

                AccountId = account.Id,
                price = productUser.StaticPrice,

                parentAccountId = account.ParentId,
                parentPrice = parentProduct?.StaticPrice,

                parentProfit = account.ParentId <= 2 ? 0 : (productUser?.StaticPrice - parentProduct?.StaticPrice),
            };
        }

    }
}
