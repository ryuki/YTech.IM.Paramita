using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Transaction
{
    public class TRecAccountMap : IAutoMappingOverride<TRecAccount>
    {
        #region Implementation of IAutoMappingOverride<TRecAccount>

        public void Override(AutoMapping<TRecAccount> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_REC_ACCOUNT");
            mapping.Id(x => x.Id, "REC_ACCOUNT_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.RecPeriodId, "REC_PERIOD_ID").Fetch.Join();
            mapping.References(x => x.CostCenterId, "COST_CENTER_ID").Fetch.Join();
            mapping.References(x => x.AccountId, "ACCOUNT_ID").Fetch.Join();
            mapping.Map(x => x.AccountStatus, "ACCOUNT_STATUS");
            mapping.Map(x => x.RecAccountStart, "REC_ACCOUNT_START");
            mapping.Map(x => x.RecAccountEnd, "REC_ACCOUNT_END");
            mapping.Map(x => x.RecAccountDesc, "REC_ACCOUNT_DESC");

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
