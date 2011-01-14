using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MItemUomRepository : NHibernateRepositoryWithTypedId<MItemUom, string>, IMItemUomRepository
    {
        #region Implementation of IMItemUomRepository

        public MItemUom GetByItem(MItem item)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MItemUom));
            criteria.Add(Expression.Eq("ItemId", item));
            return criteria.UniqueResult() as MItemUom;
        }

        #endregion
    }
}
