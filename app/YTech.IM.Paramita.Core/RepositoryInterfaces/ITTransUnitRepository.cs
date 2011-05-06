using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITTransUnitRepository : INHibernateRepositoryWithTypedId<TTransUnit, string>
    {
        TTransUnit GetByUnitId(string unitId);

        void DeleteByUnitId(string unitId);
    }
}
