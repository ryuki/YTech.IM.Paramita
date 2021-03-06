﻿using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.Paramita.Core.Master;

namespace YTech.IM.Paramita.Core.RepositoryInterfaces
{
    public interface IMAccountRefRepository : INHibernateRepositoryWithTypedId<MAccountRef, string>
    {

        MAccountRef GetByRefTableId(Enums.EnumReferenceTable enumReferenceTable, string referenceId);
    }
}
