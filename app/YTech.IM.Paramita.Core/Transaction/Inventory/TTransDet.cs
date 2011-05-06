using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.Transaction.Inventory
{
    public class TTransDet : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public TTransDet() { }

          public TTransDet(TTrans trans)
        {
            Check.Require(trans != null, "trans may not be null");

            TransId = trans;
        }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual TTrans TransId { get; protected set; }
        public virtual MItem ItemId { get; set; }
        public virtual MItemUom ItemUomId { get; set; }
        public virtual MJobType JobTypeId { get; set; }
        public virtual int? TransDetNo { get; set; }
        public virtual decimal? TransDetQty { get; set; }
        public virtual decimal? TransDetPrice { get; set; }
        public virtual decimal? TransDetDisc { get; set; }
        public virtual decimal? TransDetTotal { get; set; }
        public virtual string TransDetDesc { get; set; }

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
