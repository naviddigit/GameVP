using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectLayer.Models.Repository
{
    public class RServerSoft : Interface.IDatabaseRead
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

        public object? Get(int id)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ServerSofts.Single(i => i.Id == id);
            }
        }

        public object? Get(string value)
        {
            throw new NotImplementedException();
        }

        public object? List(object? value)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.ServerSofts.Where(i => i.Active == true).OrderBy(i => i.Id).ToList();
            }
        }
    }
}
