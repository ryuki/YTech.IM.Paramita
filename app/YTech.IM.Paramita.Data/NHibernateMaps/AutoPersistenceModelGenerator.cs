﻿using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using YTech.IM.Paramita.Core;
using YTech.IM.Paramita.Data.NHibernateMaps.Conventions;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace YTech.IM.Paramita.Data.NHibernateMaps
{

    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {

        #region IAutoPersistenceModelGenerator Members

        public AutoPersistenceModel Generate()
        {
            return AutoMap.AssemblyOf<Class1>(new AutomappingConfiguration())
                .Conventions.Setup(GetConventions())
                .IgnoreBase<Entity>()
                .IgnoreBase(typeof(EntityWithTypedId<>))
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
        }

        #endregion

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.ForeignKeyConvention>();
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.HasManyConvention>();
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.HasManyToManyConvention>();
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.ManyToManyTableNameConvention>();
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.PrimaryKeyConvention>();
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.ReferenceConvention>();
                c.Add<YTech.IM.Paramita.Data.NHibernateMaps.Conventions.TableNameConvention>();
            };
        }
    }
}
