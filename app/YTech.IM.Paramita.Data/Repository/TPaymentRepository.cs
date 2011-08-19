using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction.Payment;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TPaymentRepository : NHibernateRepositoryWithTypedId<TPayment, string>, ITPaymentRepository
    {
    }
}
