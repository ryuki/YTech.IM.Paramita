using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Core.Transaction.Unit
{
    public class TUnit : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string UnitNo { get; set; }
        public virtual MUnitType UnitTypeId { get; set; }
        public virtual int? UnitLandWide { get; set; }
        public virtual int? UnitWide { get; set; }
        public virtual string UnitLocation { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual string UnitStatus { get; set; }
        public virtual string UnitDesc { get; set; }
        
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
