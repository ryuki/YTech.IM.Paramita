using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Unit
{
    public class TTransUnitMap : IAutoMappingOverride<TTransUnit>
    {
        #region Implementation of IAutoMappingOverride<TTransUnit>

        public void Override(AutoMapping<TTransUnit> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_TRANS_UNIT");
            mapping.Id(x => x.Id, "TRANS_UNIT_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.TransUnitFactur, "TRANS_UNIT_FACTUR");
            mapping.References(x => x.UnitId, "UNIT_ID").Fetch.Join();
            mapping.References(x => x.CustomerId, "CUSTOMER_ID").Fetch.Join();
            mapping.Map(x => x.TransUnitDate, "TRANS_UNIT_DATE");
            mapping.Map(x => x.TransUnitPrice, "TRANS_UNIT_PRICE");
            mapping.Map(x => x.TransUnitStatus, "TRANS_UNIT_STATUS");
            mapping.Map(x => x.TransUnitDesc, "TRANS_UNIT_DESC"); 

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
