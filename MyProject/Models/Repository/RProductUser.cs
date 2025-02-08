using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Entity;
using Microsoft.EntityFrameworkCore;

namespace ProjectLayer.Models.Repository
{
    public class RProductUser : Interface.IDatabaseRead , Interface.IDatabaseWrite, Interface.IDatabase
    {
        public bool AnyByAccountId(int accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Any(i => i.AccountId == accountId);
            }
        }

        public bool Any(int accountId)
        {
            throw new NotImplementedException();
        }

        public bool Any1(string value)
        {
            throw new NotImplementedException();
        }

        public bool Any2(object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object? Get(int productUserId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).Single(i => i.Id == productUserId);
            }
        }

        public int? GetProductUserId(int? id,int accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product)?.SingleOrDefault(i => i.ProductId == id && i.AccountId == accountId)?.Id;
            }
        }

        public string? GetProductNameUserId(int? id, int accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product)?.SingleOrDefault(i => i.ProductId == id && i.AccountId == accountId)?.Product?.Name?.ToString();
            }
        }

        public object? GetByProductId(int productId, int accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).Single(i => i.ProductId == productId && i.AccountId == accountId);
            }
        }


        public decimal? GetPrice(int accountId)
        {
            if (!AnyByAccountId(accountId)) return null;
            
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).First(i => i.AccountId == accountId).StaticPrice;
            }
        }

        public decimal? GetPrice(int accountId,int ProductId)
        {
            if (!AnyByAccountId(accountId)) return null;

            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).Single(i => i.AccountId == accountId && i.ProductId == ProductId).StaticPrice;
            }
        }

        public int GetProductId(int ProductUserId)
        {

            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).Single(i => i.Id == ProductUserId).Product.Id;
            }
        }
        public int GetProductUserId(int productId,int accountId)
        {

            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).Single(i => i.ProductId == productId && i.AccountId ==accountId).Id;
            }
        }



        public object? Get(string value)
        {
            throw new NotImplementedException();
        }

        public object? List(object? accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ProductUsers.Include(i => i.Product).Where(i => i.AccountId == (int?)accountId).OrderBy(i => i.Id).ToList();
            }
        }

        public object Insert(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var item = context.ProductUsers.Add((DataService.Entity.ProductUser)items);
                context.SaveChanges();
                return item.Entity;
            }
        }

        public void Update(object items)
        {
            throw new NotImplementedException();
        }



        public void Update(object productUserId, decimal StaticPrice)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var user = context.ProductUsers.Single(t => t.Id == (int)productUserId);

                user.StaticPrice = StaticPrice;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public bool Delete(object items)
        {
            throw new NotImplementedException();
        }

        public void Update(object items1, object items2)
        {
            throw new NotImplementedException();
        }
    }
}