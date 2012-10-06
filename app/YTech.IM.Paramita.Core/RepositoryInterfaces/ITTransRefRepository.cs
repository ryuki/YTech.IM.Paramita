using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITTransRefRepository : INHibernateRepositoryWithTypedId<TTransRef, string>
    {
        TTransRef GetByRefId(string transId);

        void DeleteByTransId(string transId);
    }
}
