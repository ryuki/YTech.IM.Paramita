using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.RepositoryInterfaces;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class UnitTypeController : Controller
    {
        private readonly IMUnitTypeRepository _mUnitTypeRepository;
        public UnitTypeController(IMUnitTypeRepository mUnitTypeRepository)
        {
            Check.Require(mUnitTypeRepository != null, "mUnitTypeRepository may be not null");
            _mUnitTypeRepository = mUnitTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public ActionResult ListForSubGrid(string id)
        {
            var unitTypes = _mUnitTypeRepository.GetByUnitTypeId(id);
        }
    }
}
