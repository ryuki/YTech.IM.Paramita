using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MUnitTypeRepository : NHibernateRepositoryWithTypedId<MUnitType,string >, IMUnitTypeRepository
    {
        public IList<MUnitType> GetByCostCenterId(string costCenterId)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"   select ut
                                from MUnitType as ut");

            if (!string.IsNullOrEmpty(costCenterId))
            {
                sql.AppendLine(@" where ut.CostCenterId.Id = :costCenterId");
            }

            IQuery q = Session.CreateQuery(sql.ToString());

            if (!string.IsNullOrEmpty(costCenterId))
            {
                q.SetString("costCenterId", costCenterId);
            }

            return q.List<MUnitType>();
        }
    }
}
