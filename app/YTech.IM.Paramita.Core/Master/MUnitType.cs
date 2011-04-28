using System;
using SharpArch.Core;
using SharpArch.Core.DomainModel;
using NHibernate.Validator.Constraints;

namespace YTech.IM.Paramita.Core.Master
{
    public class MUnitType : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string UnitTypeName { get; set; }
        public virtual int? UnitTypeTotal { get; set; }
        public virtual string UnitTypeStatus { get; set; }
        public virtual string UnitTypeDesc { get; set; }

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
