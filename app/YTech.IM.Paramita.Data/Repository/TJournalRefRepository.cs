using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Accounting;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TJournalRefRepository : NHibernateRepositoryWithTypedId<TJournalRef, string>, ITJournalRefRepository
    {
        public TJournalRef GetByReference(EnumReferenceTable enumReferenceTable, string transStatus, string transId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select ref
                                from TJournalRef as ref
                                    where ref.ReferenceTable = :enumReferenceTable 
                                        and ref.ReferenceType = :transStatus 
                                        and ref.ReferenceId = :transId ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetMaxResults(1);
            q.SetString("enumReferenceTable", enumReferenceTable.ToString());
            q.SetString("transStatus", transStatus);
            q.SetString("transId", transId);
            return q.UniqueResult<TJournalRef>();
        }
    }
}
