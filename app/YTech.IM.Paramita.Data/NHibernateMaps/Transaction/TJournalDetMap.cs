using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Transaction
{
    public class TJournalDetMap : IAutoMappingOverride<TJournalDet>
    {
        #region Implementation of IAutoMappingOverride<TJournalDet>

        public void Override(AutoMapping<TJournalDet> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_JOURNAL_DET");
            mapping.Id(x => x.Id, "JOURNAL_DET_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.JournalId, "JOURNAL_ID").Not.Nullable();
            mapping.References(x => x.AccountId, "ACCOUNT_ID").Fetch.Join();
            mapping.Map(x => x.JournalDetNo, "JOURNAL_DET_NO");
            mapping.Map(x => x.JournalDetStatus, "JOURNAL_DET_STATUS");
            mapping.Map(x => x.JournalDetAmmount, "JOURNAL_DET_AMMOUNT");
            mapping.Map(x => x.JournalDetDesc, "JOURNAL_DET_DESC");
            mapping.Map(x => x.JournalDetEvidenceNo, "JOURNAL_DET_EVIDENCE_NO");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();
        }

        #endregion
    }
}
