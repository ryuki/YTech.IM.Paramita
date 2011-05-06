using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TTransUnitRepository : NHibernateRepositoryWithTypedId<TTransUnit, string>, ITTransUnitRepository
    {
        public TTransUnit GetByUnitId(string unitId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select unit
                                    from TTransUnit as unit
                                    where unit.UnitId.Id = :unitId ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("unitId", unitId);
            q.SetMaxResults(1);
            return q.UniqueResult<TTransUnit>();
        }

        public void DeleteByUnitId(string unitId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  delete from TTransUnit as unit
                                    where unit.UnitId.Id = :unitId ");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("unitId", unitId);
            q.ExecuteUpdate();
        }
    }
}
