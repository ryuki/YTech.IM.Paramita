﻿using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;


namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
   public interface IRefAddressRepository : INHibernateRepositoryWithTypedId<RefAddress, string>
    {
    }
}
