using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;

namespace YTech.IM.Paramita.Core.Master
{
    public class MProduct : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public MProduct() { }

        public MProduct(MCostCenter costCenter)
        {
            Check.Require(costCenter != null, "costCenter may not be null");

            CostCenterId = costCenter;
        }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string ProductName { get; set; }
        public virtual MCostCenter CostCenterId { get; set; }
        public virtual decimal? ProductQty { get; set; }
        public virtual string ProductDesc { get; set; }
        public virtual string ProductStatus { get; set; }

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
