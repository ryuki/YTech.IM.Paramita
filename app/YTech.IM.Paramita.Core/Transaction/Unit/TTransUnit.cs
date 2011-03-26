using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.Transaction.Unit
{
    public class TTransUnit : EntityWithTypedId<string>, IHasAssignedId<string>
    {

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string TransUnitFactur { get; set; }
        public virtual TUnit UnitId { get; set; }
        public virtual MCustomer CustomerId { get; set; }
        public virtual DateTime? TransUnitDate { get; set; } 
        public virtual decimal? TransUnitPrice { get; set; }
        public virtual string TransUnitStatus { get; set; }
        public virtual string TransUnitDesc { get; set; }
        public virtual MCostCenter CostCenterId { get; set; }
        public virtual string TransUnitPaymentMethod { get; set; }

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
