using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;
using YTech.IM.Paramita.Core.View;

namespace YTech.IM.Paramita.Data.NHibernateMaps.View
{
    public class VJournalDetFlowMap : IAutoMappingOverride<VJournalDetFlow>
    {
        #region Implementation of IAutoMappingOverride<VJournalDetFlow>

        public void Override(AutoMapping<VJournalDetFlow> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.V_JOURNAL_DET_FLOW");
            mapping.Id(x => x.Id, "JOURNAL_DET_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.RowNumber, "ROWNUMBER");
            mapping.References(x => x.JournalId, "JOURNAL_ID").Not.Nullable();
            mapping.References(x => x.AccountId, "ACCOUNT_ID").Fetch.Join();
            mapping.Map(x => x.JournalDetNo, "JOURNAL_DET_NO");
            mapping.Map(x => x.JournalDetStatus, "JOURNAL_DET_STATUS");
            mapping.Map(x => x.JournalDetAmmount, "JOURNAL_DET_AMMOUNT");
            mapping.Map(x => x.JournalDetDesc, "JOURNAL_DET_DESC");
            mapping.Map(x => x.JournalDetEvidenceNo, "JOURNAL_DET_EVIDENCE_NO");
            mapping.Map(x => x.Saldo, "SALDO");

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
