using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TTransRefRepository : NHibernateRepository<TTransRef>, ITTransRefRepository
    {
        public TTransRef GetByRefId(string transId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select ref
                                from TTransRef as ref
                                    where ref.TransIdRef.Id = :transId ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("transId", transId);
            q.SetMaxResults(1);
            return q.UniqueResult<TTransRef>();
        }
    }
}
