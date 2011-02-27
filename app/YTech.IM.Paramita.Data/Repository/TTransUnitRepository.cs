using System.Collections.Generic;
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
    }
}
