using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Transaction
{
    public class TRealMap : IAutoMappingOverride<TReal>
    {
        #region Implementation of IAutoMappingOverride<TReal>

        public void Override(AutoMapping<TReal> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_REAL");
            mapping.Id(x => x.Id, "REAL_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.CostCenterId, "COST_CENTER_ID"); 
            mapping.Map(x => x.RealDate, "REAL_DATE");
            mapping.Map(x => x.RealPercentValue, "REAL_PERCENT_VALUE"); 
            mapping.Map(x => x.RealStatus, "REAL_STATUS");
            mapping.Map(x => x.RealDesc, "REAL_DESC");

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
