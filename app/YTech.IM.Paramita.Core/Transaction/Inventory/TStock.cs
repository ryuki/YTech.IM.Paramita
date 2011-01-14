﻿using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.Transaction.Inventory
{
 public   class TStock : EntityWithTypedId<string>, IHasAssignedId<string>
    {

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual MItem ItemId { get; set; }
        public virtual MWarehouse WarehouseId { get; set; }
        public virtual TTransDet TransDetId { get; set; }
        public virtual DateTime? StockDate { get; set; }
        public virtual decimal? StockQty { get; set; }
        public virtual decimal? StockPrice { get; set; }
        public virtual string StockStatus { get; set; }
        public virtual string StockDesc { get; set; }

        public virtual IList<TStockRef> StockRefs { get; protected set; }

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
