using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NET1717_Lab01_ProductManagement.Repository.DBContextConfiguration
{
    public static class IQueryableExtentions
    {
        public static IQueryable<T> HasQuery<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate);
        }
    }
}

