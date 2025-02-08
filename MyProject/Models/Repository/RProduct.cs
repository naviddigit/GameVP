using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectLayer.Models.Repository
{
    public class RProduct : Interface.IDatabaseRead
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

        public object? Get(int value)
        {
            throw new NotImplementedException();
        }

        public object? Get(string value)
        {
            throw new NotImplementedException();
        }

        public object? List(object? value)
        {
            using (var context = new DataService.Data.SoftGameContext())
            {
                return context.Products.OrderBy(i => i.Id).ToList();
            }
        }
    }
}
