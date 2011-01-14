using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IRefPersonRepository : INHibernateRepositoryWithTypedId<RefPerson, string>
    {
        
    }
}
