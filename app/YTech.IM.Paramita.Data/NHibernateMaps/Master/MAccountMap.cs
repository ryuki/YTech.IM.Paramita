using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Master
{
    public class MAccountMap : IAutoMappingOverride<MAccount>
    {
        #region Implementation of IAutoMappingOverride<MAccount>

        public void Override(AutoMapping<MAccount> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("M_ACCOUNT");
            mapping.Id(x => x.Id, "ACCOUNT_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.AccountCatId, "ACCOUNT_CAT_ID").Fetch.Join();
            mapping.References(x => x.AccountParentId, "ACCOUNT_PARENT_ID").Fetch.Join();
            mapping.Map(x => x.AccountStatus, "ACCOUNT_STATUS");
            mapping.Map(x => x.AccountName, "ACCOUNT_NAME");
            mapping.Map(x => x.AccountDesc, "ACCOUNT_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();

            mapping.HasMany(x => x.Children)
                .KeyColumn("ACCOUNT_PARENT_ID")
                .LazyLoad();
        }

        #endregion
    }
}
