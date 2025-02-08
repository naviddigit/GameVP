using ProjectLayer.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLayer.Models.Repository
{
    internal class RServer : IDatabaseRead, IDatabaseWrite, IDatabase
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
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Servers.Single(i => i.Active == true && i.Ip == value);
            }
        }

        public object Insert(object items)
        {
            throw new NotImplementedException();
        }

        public object? List(object? value)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Servers.Where(i => i.Active == true).OrderBy(i => i.Id).ToList();
            }
        }

        public List<DataService.Entity.Server> List()
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Servers
                             .Where(i => i.Active == true)
                             .OrderBy(i => i.Id)
                             .ToList();
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
