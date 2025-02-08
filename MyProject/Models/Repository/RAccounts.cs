using ProjectLayer.Models.Interface;
using Microsoft.EntityFrameworkCore;
namespace ProjectLayer.Models.Repository
{
    public class RAccount : IDatabaseRead, IDatabaseWrite, IDatabase
    {
        public object Insert(object items)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var item = context.Accounts.Add((DataService.Entity.Account)items);
                context.SaveChanges();
                return item.Entity;
            }
        }

        public object? Get(int id)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.SingleOrDefault(i => i.Id == id);
            }
        }

        public object? Get(string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.SingleOrDefault(u => u.Username == username);
            }
        }


        public object? Get(int accountId, string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if (accountId == 0)
                    return context.Accounts.Single(i => i.Username == username);
                return context.Accounts.Single(i => i.Username == username && i.ParentId == accountId);
            }
        }


        public object? GetByToken(string? token)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.SingleOrDefault(u => u.Token == token);
            }
        }


        public string? GetUsernameById(int? id)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.SingleOrDefault(u => u.Id == id)?.Username;
            }
        }

        public object? GetById(int? accountId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.SingleOrDefault(u => u.Id == accountId);
            }
        }

        public object? GetByUseranem(string? usertname)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.SingleOrDefault(u => u.Username == usertname);
            }
        }

        public object List(object parentId)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                if ((int)parentId != 0)
                    return context.Accounts.Include(i => i.Role).Where(i => i.ParentId == (int)parentId).OrderByDescending(i=>i.Id).ToList();
                
                return context.Accounts.Include(i => i.Role).OrderByDescending(i => i.Id).ToList();
            }
        }

        public bool Any1(string username)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.Any(u => u.Username == username);
            }
        }
        public bool Any2(object username, object password)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.Any(u => u.Username == (string)username && u.Password == (string)password);
            }
        }
        public bool Any(int value)
        {
            throw new NotImplementedException();
        }

        public bool AnyByToken(string? token)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Accounts.Any(u => u.Token == token);
            }
        }

        public void Update(object username)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object items)
        {
            throw new NotImplementedException();
        }

        public void Update(object username, object token)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var user = context.Accounts.Where(t => t.Username == (string)username).FirstOrDefault();
                user.Token = token.ToString();
                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Update(DataService.Entity.Account item)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var user = context.Accounts.Where(t => t.Username == item.Username).FirstOrDefault();
                user.Password = item.Password;
                user.CreditorLimit = item.CreditorLimit;
                user.Active = item.Active;
                user.Email = item.Email;
                user.Mobile = item.Mobile;

                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateCountFailed(object username, int count = 0)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                var user = context.Accounts.Single(t => t.Username == (string)username);
                user.CountFailed = count;
                context.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
