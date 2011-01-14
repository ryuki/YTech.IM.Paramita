using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Accounting;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Transaction
{
    public class TRecPeriodMap : IAutoMappingOverride<TRecPeriod>
    {
        #region Implementation of IAutoMappingOverride< TRecPeriod>

        public void Override(AutoMapping<TRecPeriod> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_REC_PERIOD");
            mapping.Id(x => x.Id, "REC_PERIOD_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.PeriodFrom, "PERIOD_FROM");
            mapping.Map(x => x.PeriodTo, "PERIOD_TO");
            mapping.Map(x => x.PeriodType, "PERIOD_TYPE");
            mapping.Map(x => x.PeriodStatus, "PERIOD_STATUS");
            mapping.Map(x => x.PeriodDesc, "PERIOD_DESC");

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
