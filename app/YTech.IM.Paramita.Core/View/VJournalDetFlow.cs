using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Core.View
{
    public class VJournalDetFlow : EntityWithTypedId<string>, IHasAssignedId<string>
    {

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual int? RowNumber { get; protected set; }
        public virtual TJournal JournalId { get; protected set; }
        public virtual MAccount AccountId { get; set; }
        public virtual int? JournalDetNo { get; set; }
        public virtual string JournalDetStatus { get; set; }
        public virtual decimal? JournalDetAmmount { get; set; }
        public virtual string JournalDetDesc { get; set; }
        public virtual string JournalDetEvidenceNo { get; set; }
        public virtual decimal? Saldo { get; set; }

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