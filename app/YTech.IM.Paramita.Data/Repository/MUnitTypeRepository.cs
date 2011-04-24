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
        public IList<MUnitType> GetByUnitTypeId(string unitTypeId)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"   select ut
                                from MUnitType as ut");

            if (!string.IsNullOrEmpty(unitTypeId))
            {
                sql.AppendLine(@" where ut.UnitTypeId.Id = :unitTypeId");
            }

            IQuery q = Session.CreateQuery(sql.ToString());

            if (!string.IsNullOrEmpty(unitTypeId))
            {
                q.SetString("unitTypeId", unitTypeId);
            }

            return q.List<MUnitType>();
        }
    }
}
