using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;
using YTech.IM.Paramita.Core.Transaction.Unit;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Unit
{
    public class TUnitMap : IAutoMappingOverride<TUnit>
    {
        #region Implementation of IAutoMappingOverride<TUnit>

        public void Override(AutoMapping<TUnit> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_UNIT");
            mapping.Id(x => x.Id, "UNIT_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.UnitNo, "UNIT_NO");
            mapping.References(x => x.UnitTypeId, "UNIT_TYPE_ID").LazyLoad();
            mapping.Map(x => x.UnitLandWide, "UNIT_LAND_WIDE");
            mapping.Map(x => x.UnitWide, "UNIT_WIDE");
            mapping.Map(x => x.UnitLocation, "UNIT_LOCATION");
            mapping.Map(x => x.UnitPrice, "UNIT_PRICE");
            mapping.Map(x => x.UnitStatus, "UNIT_STATUS");
            mapping.Map(x => x.UnitDesc, "UNIT_DESC");
            mapping.References(x => x.CostCenterId, "COST_CENTER_ID").LazyLoad();

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
