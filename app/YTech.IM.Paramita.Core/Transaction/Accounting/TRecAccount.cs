using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.Transaction.Accounting
{
    public class TRecAccount : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public TRecAccount() { }

        public TRecAccount(TRecPeriod recPeriod)
        {
            Check.Require(recPeriod != null, "recPeriod may not be null");

            RecPeriodId = recPeriod;
        }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual TRecPeriod RecPeriodId { get; protected set; }
        public virtual MCostCenter CostCenterId { get; set; }
        public virtual MAccount AccountId { get; set; }
        public virtual string AccountStatus { get; set; }
        public virtual decimal? RecAccountStart { get; set; }
        public virtual decimal? RecAccountEnd { get; set; }
        public virtual string RecAccountDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
