using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MUnitTypeRepository : NHibernateRepositoryWithTypedId<MUnitType,string >, IMUnitTypeRepository
    {
    }
}
