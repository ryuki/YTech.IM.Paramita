using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction.Payment;


namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface ITPaymentRepository : INHibernateRepositoryWithTypedId<TPayment, string>
    {
    }
}
