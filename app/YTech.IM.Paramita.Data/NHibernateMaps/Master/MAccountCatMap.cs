using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Master
{
    public class MAccountCatMap : IAutoMappingOverride<MAccountCat>
    {
        #region Implementation of IAutoMappingOverride<MAccountCat>

        public void Override(AutoMapping<MAccountCat> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadOnly();

            mapping.Table("dbo.M_ACCOUNT_CAT");
            mapping.Id(x => x.Id, "ACCOUNT_CAT_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.AccountCatName, "ACCOUNT_CAT_NAME");
            mapping.Map(x => x.AccountCatType, "ACCOUNT_CAT_TYPE");
            mapping.Map(x => x.AccountCatStatus, "ACCOUNT_CAT_STATUS");
            mapping.Map(x => x.AccountCatDesc, "ACCOUNT_CAT_DESC");

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
