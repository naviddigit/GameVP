using ProjectLayer.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ProjectLayer.Models.Repository
{
    public class RTransaction : IDatabaseRead, IDatabaseWrite, IDatabase
    {
        public bool Any(int value)
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

        public bool Delete(object items)
        {
            throw new NotImplementedException();
        }

        public object? Get(int value)
        {
            throw new NotImplementedException();
        }

        public object? Get(string value)
        {
            throw new NotImplementedException();
        }

        public object Insert(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var item = context.Transactions.Add((DataService.Entity.Transaction)items);
                context.SaveChanges();
                return item.Entity;
            }
        }

        public object? List(object? accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int?)accountId <= 2)
                    return context.Transactions
                        .Include(i => i.Account)
                        .Include(i => i.Product)
                        .Include(i => i.TransactionType)
                        .Where(i => i.AccountId == (int?)accountId)
                        .OrderByDescending(i => i.Id).ToList();

                return context.Transactions
                    .Include(i => i.Account)
                    .Include(i => i.Product)
                    .Include(i => i.TransactionType)
                    .Where(i => i.AccountId == (int?)accountId || (i.Account.ParentId == (int?)accountId && i.TransactionTypeId == (int)Enums.ETransaction.Type.Buy))
                    .OrderByDescending(i => i.Id).ToList();

            }
        }

        public object? List(object? accountId, bool all = false)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int?)accountId <= 2)
                {
                    var list = context.Transactions
                        .Include(i => i.Account)
                        .Include(i => i.Product)
                        .Include(i => i.TransactionType).ToList();

                    if (!all) { list = list.Where(i => i.AccountId == (int?)accountId).ToList(); }

                    return list.OrderByDescending(i => i.Id).ToList();
                }

                return context.Transactions
                    .Include(i => i.Account)
                    .Include(i => i.Product)
                    .Include(i => i.TransactionType)
                    .Where(i => i.AccountId == (int?)accountId || (i.Account.ParentId == (int?)accountId && i.TransactionTypeId == (int)Enums.ETransaction.Type.Buy))
                    .OrderByDescending(i => i.Id).ToList();

            }
        }

        public object? ListByDateLast(object? accountId,DateTime DateLast, bool all = false)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int?)accountId <= 2)
                {
                    var list = context.Transactions
                        .Include(i => i.Account)
                        .Include(i => i.Product)
                        .Include(i => i.TransactionType).ToList();

                    if (!all) { list = list.Where(i => i.AccountId == (int?)accountId).ToList(); }

                    return list.Where(d=>d.Date< DateLast).OrderByDescending(i => i.Id).ToList(); // new
                }

                return context.Transactions
                    .Include(i => i.Account)
                    .Include(i => i.Product)
                    .Include(i => i.TransactionType)
                    .Where(i => i.AccountId == (int?)accountId || (i.Account.ParentId == (int?)accountId && i.TransactionTypeId == (int)Enums.ETransaction.Type.Buy))
                    .Where(d => d.Date < DateLast) // new
                    .OrderByDescending(i => i.Id).ToList();

            }
        }


        public object? ListAndParent(int accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int?)accountId == 0)
                    return context.Transactions
                        .Include(i => i.Account)
                        .Include(i => i.Product)
                        .Include(i => i.TransactionType)
                        .OrderByDescending(i => i.Id).ToList();

                return context.Transactions
                    .Include(i => i.Account)
                    .Include(i => i.Product)
                    .Include(i => i.TransactionType)
                    .Where(i => i.AccountId == accountId || i.Account.ParentId == accountId)
                    .OrderByDescending(i => i.Id).ToList();

            }
        }

        public void Update(object items)
        {
            throw new NotImplementedException();
        }

        public void Update(object items1, object items2)
        {
            throw new NotImplementedException();
        }
    }
}
