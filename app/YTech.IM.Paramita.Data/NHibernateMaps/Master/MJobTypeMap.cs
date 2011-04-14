using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Master
{
    public class MJobTypeMap : IAutoMappingOverride<MJobType>
    {
        #region Implementation of IAutoMappingOverride<MJobType>

        public void Override(AutoMapping<MJobType> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.M_JOB_TYPE");
            mapping.Id(x => x.Id, "JOB_TYPE_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.JobTypeName, "JOB_TYPE_NAME");
            mapping.Map(x => x.JobTypeStatus, "JOB_TYPE_STATUS");
            mapping.Map(x => x.JobTypeDesc, "JOB_TYPE_DESC");

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
