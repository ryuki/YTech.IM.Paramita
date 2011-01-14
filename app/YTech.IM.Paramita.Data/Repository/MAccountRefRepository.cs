using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MAccountRefRepository : NHibernateRepositoryWithTypedId<MAccountRef, string>, IMAccountRefRepository
    {
    }
}
