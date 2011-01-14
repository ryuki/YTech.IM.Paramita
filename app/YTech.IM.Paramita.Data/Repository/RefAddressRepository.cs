using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class RefAddressRepository : NHibernateRepositoryWithTypedId<RefAddress, string>, IRefAddressRepository
    {
    }
}
