using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Data.Repository
{
    public class MAccountRefRepository : NHibernateRepositoryWithTypedId<MAccountRef, string>, IMAccountRefRepository
    {
        public MAccountRef GetByRefTableId(EnumReferenceTable enumReferenceTable, string referenceId)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MAccountRef));
            criteria.Add(Expression.Eq("ReferenceTable", enumReferenceTable.ToString()));
            if (!string.IsNullOrEmpty(referenceId))
            {
                criteria.Add(Expression.Eq("ReferenceId", referenceId));
            }
            return criteria.UniqueResult<MAccountRef>();
        }
    }
}
