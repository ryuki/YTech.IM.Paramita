using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.Transaction.Inventory
{
    public class TReal : EntityWithTypedId<string>, IHasAssignedId<string>
    {

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual DateTime? RealDate { get; set; }
        public virtual decimal? RealPercentValue { get; set; }
        public virtual MCostCenter CostCenterId { get; set; }
        public virtual string RealStatus { get; set; }
        public virtual string RealDesc { get; set; }

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
