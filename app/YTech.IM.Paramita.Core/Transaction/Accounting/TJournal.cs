using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.Transaction.Accounting
{
    public class TJournal : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public TJournal()
        {
            InitMembers();
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers()
        {
            JournalDets = new List<TJournalDet>();
        }


        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual MCostCenter CostCenterId { get; set; }
        public virtual string JournalType { get; set; }
        public virtual string JournalVoucherNo { get; set; }
        public virtual string JournalPic { get; set; }
        public virtual DateTime? JournalDate { get; set; }
        public virtual string JournalEvidenceNo { get; set; }
        public virtual decimal JournalAmmount { get; set; }
        public virtual string JournalStatus { get; set; }
        public virtual string JournalDesc { get; set; }
       
        public virtual IList<TJournalDet> JournalDets { get; protected set; }

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
