using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Master
{
    public class MUnitTypeMap : IAutoMappingOverride<MUnitType>
    {
        #region Implementation of IAutoMappingOverride<MEmployee>

        public void Override(AutoMapping<MUnitType> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_UNIT_TYPE");
            mapping.Id(x => x.Id, "UNIT_TYPE_ID").GeneratedBy.Assigned();
            mapping.Map(x => x.UnitTypeName, "UNIT_TYPE_NAME");
            mapping.Map(x => x.UnitTypeTotal, "UNIT_TYPE_TOTAL");
            mapping.Map(x => x.UnitTypeStatus, "UNIT_TYPE_STATUS");
            mapping.Map(x => x.UnitTypeDesc, "UNIT_TYPE_DESC");
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
