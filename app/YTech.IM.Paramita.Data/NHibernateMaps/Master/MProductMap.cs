using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Master
{
    public class MProductMap : IAutoMappingOverride<MProduct>
    {
        #region Implementation of IAutoMappingOverride<MProduct>

        public void Override(AutoMapping<MProduct> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.M_PRODUCT");
            mapping.Id(x => x.Id, "PRODUCT_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.CostCenterId, "COST_CENTER_ID").LazyLoad();
            mapping.Map(x => x.ProductName, "PRODUCT_NAME");
            mapping.Map(x => x.ProductQty, "PRODUCT_QTY");
            mapping.Map(x => x.ProductStatus, "PRODUCT_STATUS");
            mapping.Map(x => x.ProductDesc, "PRODUCT_DESC");

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
