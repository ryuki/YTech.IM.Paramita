using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
   public interface IMItemUomRepository : INHibernateRepositoryWithTypedId<MItemUom, string>
   {
       MItemUom GetByItem(MItem item);
   }
}
