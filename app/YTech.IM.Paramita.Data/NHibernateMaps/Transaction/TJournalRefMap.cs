using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Transaction
{
    public class TJournalRefMap : IAutoMappingOverride<TJournalRef>
    {
        #region Implementation of IAutoMappingOverride< TJournalRef>

        public void Override(AutoMapping<TJournalRef> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_JOURNAL_REF");
            mapping.Id(x => x.Id, "JOURNAL_REF_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.ReferenceTable, "REFERENCE_TABLE");
            mapping.Map(x => x.ReferenceType, "REFERENCE_TYPE");
            mapping.Map(x => x.ReferenceId, "REFERENCE_ID");
            mapping.References(x => x.JournalId, "JOURNAL_ID").Fetch.Join();
            mapping.Map(x => x.JournalRefDesc, "JOURNAL_REF_DESC");

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
