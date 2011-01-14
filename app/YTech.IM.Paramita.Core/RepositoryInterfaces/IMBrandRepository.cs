using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IMBrandRepository : INHibernateRepositoryWithTypedId<MBrand, string>
    {
       IEnumerable<MBrand> GetPagedBrandList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
