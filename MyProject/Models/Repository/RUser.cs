using ProjectLayer.Models.Interface;
using Microsoft.EntityFrameworkCore;
using DataService.Entity;

namespace ProjectLayer.Models.Repository
{
    public class RUser :   IDatabase, IDatabaseReadMore
    {
        public bool Any(int value)
        {
            throw new NotImplementedException();
        }

        public bool Any1(string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Users.Any(u => u.Username == username);
            }
        }

        public bool Any2(object accountId, object username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int)accountId == 0)
                    return context.Users.Any(i => i.Username == (string)username);
                return context.Users.Any(i => i.AccountId == (int)accountId && i.Username == (string)username);
            }
        }

        public int? Count(object? accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int)accountId == 0)
                    return context.Users.Count();
                return context.Users.Where(i => i.AccountId == (int)accountId).Count();
            }
        }

        public bool Delete(object items)
        {
            throw new NotImplementedException();
        }

        public string GetServerIp(string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Users.Include(x => x.Server).Single(i=>i.Username.ToLower() == username.ToLower()).Server.Ip;
            }
        }


        public object? Get(string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Users.Include(i => i.Account).Include(i => i.Server).Single(i => i.Username == username);
            }
        }

        public object? Get(int accountId, string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if (accountId == 0)
                    return context.Users.Include(i => i.Account).Include(i => i.Server).Single(i => i.Username == username);
                return context.Users.Include(i => i.Account).Include(i => i.Server).Single(i => i.Username == username && i.AccountId == accountId);
            }
        }

        public object Insert(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var item = context.Users.Add((DataService.Entity.User)items);
                context.SaveChanges();
                return item.Entity;
            }
        }

        public object? List(object? accountId = null)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int)accountId == 0)
                    return context.Users.Include(i => i.Account).Include(i => i.Server).ToList();//.OrderByDescending(i => i.RenewDate == null ? i.CreateDate : i.RenewDate).ToList();
                return context.Users.Include(i => i.Account).Include(i => i.Server).Where(i => i.AccountId == (int)accountId).ToList();
            }
        }

        public object? ListByParent(object? parentId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Users.Include(i => i.Account).Include(i => i.Server).Where(i => i.Account.ParentId == (int)parentId).ToList();
            }
        }

        public object? Sum(object value)
        {
            throw new NotImplementedException();
        }

        public void Update(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var _user = (User)items;
                var user = context.Users.Single(t => t.Username == _user.Username);

                user.Password = _user.Password;
                user.PhoneNumber = _user.PhoneNumber == "" ? null : _user.PhoneNumber;
                user.Description = _user.Description == "" ? null : _user.Description;
                user.Banned = _user.Banned;
                user.BannedText = _user.BannedText;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void UpdateRenew(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var _user = (User)items;
                var user = context.Users.Single(t => t.Username == _user.Username);

                user.RenewDate = _user.RenewDate;
                user.ExpirationDate = user.ExpirationDate == null ? null : _user.ExpirationDate;
                user.ProductId = _user.ProductId;

                user.ServerId = _user.ServerId;
                user.Updated = _user.Updated;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateCahngeServerIp(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var _user = (User)items;
                var user = context.Users.Single(t => t.Username == _user.Username);

                user.ServerId = _user.ServerId;
                user.Updated = _user.Updated;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateAccountId(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var _user = (User)items;
                var user = context.Users.Single(t => t.Username == _user.Username);

                user.AccountId = _user.AccountId;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void UpdateUserId_IBSng(string Username,object UserId_IBSng)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var user = context.Users.Single(t => t.Username == Username);

                user.UserIdIbsng = (int)UserId_IBSng;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void UpdateExpairetionDate(object username,DateTime? dateExp)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var user = context.Users.Single(t => t.Username == username);

                user.ExpirationDate = dateExp;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Update(object items1, object items2)
        {
            throw new NotImplementedException();
        }
    }
}
