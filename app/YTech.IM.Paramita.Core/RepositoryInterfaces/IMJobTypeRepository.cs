using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IMJobTypeRepository : INHibernateRepositoryWithTypedId<MJobType, string>
    {
        IEnumerable<MJobType> GetPagedJobTypeList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
