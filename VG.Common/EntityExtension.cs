using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace VG.Common
{
    public static class EntityExtension
    {
        //public static IQueryable<T> DumpNoLock<T>(this IQueryable<T> query)
        //{
        //    using (var txn = GetNewReadUncommittedScope())
        //    {
        //        return query.Dump();
        //    }
        //}

        public static TransactionScope GetNewReadUncommittedScope()
        {
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            });
        }
        //public static IQueryable<T> DumpNoLock<T>(this IQueryable<T> query, string description)
        //{
        //    using (var txn = GetNewReadUncommittedScope())
        //    {
        //        return query.Dump(description);
        //    }
        //}

        /// <summary>
        /// set level read uncommited trước khi thực thi truy vấn
        /// hạn chế: nếu có nhiều query phía sau, nó cũng sẽ read uncommited
        /// </summary>
        /// <returns></returns>
        public static List<T> ToListNoLock<T>(this IQueryable<T> query)
        {
            using (var txn = GetNewReadUncommittedScope())
            {
                return query.ToList();
            }
        }

        public static U NoLock<T, U>(this IQueryable<T> query, Func<IQueryable<T>, U> expr)
        {
            using (var txn = GetNewReadUncommittedScope())
            {
                return expr(query);
            }
        }
    }
}
