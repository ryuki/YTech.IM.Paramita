using FluentNHibernate.Automapping;
using YTech.IM.Paramita.Core.Master;
using FluentNHibernate.Automapping.Alterations;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Data.NHibernateMaps.Transaction
{
    public class TTransRefMap : IAutoMappingOverride<TTransRef>
    {
        #region Implementation of IAutoMappingOverride<TTransRef>

        public void Override(AutoMapping<TTransRef> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.T_TRANS_REF");
            mapping.Id(x => x.Id, "TRANS_REF_ID")
                 .GeneratedBy.Assigned();
            
            //mapping.CompositeId()
            //    .KeyProperty(x => x.TransId)
            //    .KeyProperty(x => x.TransIdRef)
            //;

            mapping.Map(x => x.TransRefStatus, "TRANS_REF_STATUS");
            mapping.Map(x => x.TransRefDesc, "TRANS_REF_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();

            mapping.References(x => x.TransId, "TRANS_ID");
            mapping.References(x => x.TransIdRef, "TRANS_ID_REF");
            
        }

        #endregion
    }
}