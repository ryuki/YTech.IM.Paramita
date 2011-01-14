using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Master
{
    public class MAccountRefMap : IAutoMappingOverride<MAccountRef>
    {
        #region Implementation of IAutoMappingOverride<MAccountRef>

        public void Override(AutoMapping<MAccountRef> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("M_ACCOUNT_REF");
            mapping.Id(x => x.Id, "ACCOUNT_REF_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.ReferenceTable, "REFERENCE_TABLE");
            mapping.Map(x => x.ReferenceType, "REFERENCE_TYPE");
            mapping.Map(x => x.ReferenceId, "REFERENCE_ID");
            mapping.References(x => x.AccountId, "ACCOUNT_ID").Fetch.Join();

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
