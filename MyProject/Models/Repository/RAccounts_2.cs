using Microsoft.EntityFrameworkCore;
using DataService.Data;
using DataService.Entity;

namespace Riko.Models.Repository
{
    public class RAccounts
    {
        private data db = new rikofootballcom_newContext();

        public AdminAccount GetById(int id)
        {
            return db.AdminAccounts.Single(i => i.Id == id);
        }

        public List<AdminAccount> List()
        {
            return db.AdminAccounts.Include(r => r.Role).ToList();
        }

        public bool AnyUserByUsername(string username)
        {
            return db.AdminAccounts.Any(u => u.Username == username);
        }

        public bool AnyUser(string username, string password)
        {
            return db.AdminAccounts.Any(u => u.Username == username && u.Password == password);
        }

        public bool AnyUserByToken(string token)
        {
            return db.AdminAccounts.Any(u => u.Token == token);
        }

        public AdminAccount GetByUsername(string username)
        {
            return db.AdminAccounts.Single(u => u.Username == username);
        }

        public int GetRoleIdByToken(string token)
        {
            return db.AdminAccounts.Single(u => u.Token == token).Id;
        }

        public void UpdateCountFailed(string username, int count = 0)
        {
            AdminAccount user = db.AdminAccounts.Where(t => t.Username == username).FirstOrDefault();
            user.CountFailed = count;

            db.Attach(user);
            db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateToken(string username, string token)
        {
            AdminAccount user = db.AdminAccounts.Where(t => t.Username == username).FirstOrDefault();
            user.Token = token;

            db.Attach(user);
            db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

    }
}
