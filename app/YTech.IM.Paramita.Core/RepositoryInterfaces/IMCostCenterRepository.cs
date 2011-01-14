using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IMCostCenterRepository : INHibernateRepositoryWithTypedId<MCostCenter, string>
    {
        IEnumerable<MCostCenter> GetPagedCostCenterList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
