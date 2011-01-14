using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;

namespace YTech.IM.Paramita.Core
{
    public abstract class MyBaseEntity : EntityWithTypedId<string>
    {
      public virtual string DataStatus { get; set; }
      public virtual string CreatedBy { get; set; }
      public virtual DateTime? CreatedDate { get; set; }
      public virtual string ModifiedBy { get; set; }
      public virtual DateTime? ModifiedDate { get; set; }
    }
}
