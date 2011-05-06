using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IMUnitTypeRepository : INHibernateRepositoryWithTypedId<MUnitType, string>
    {
        IList<MUnitType> GetByCostCenterId(string costCenterId);
    }
}
