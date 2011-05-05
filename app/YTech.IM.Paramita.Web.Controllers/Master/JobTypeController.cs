using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class JobTypeController : Controller
    {
        public JobTypeController() : this(new MJobTypeRepository())
         {}

        private readonly IMJobTypeRepository _mJobTypeRepository;
        public JobTypeController(IMJobTypeRepository mJobTypeRepository)
        {
            Check.Require(mJobTypeRepository != null, "mJobTypeRepository may not be null");

            _mJobTypeRepository = mJobTypeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var jobTypes = _mJobTypeRepository.GetPagedJobTypeList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from jobType in jobTypes
                    select new
                    {
                        i = jobType.Id.ToString(),
                        cell = new string[] {
                            jobType.Id, 
                            jobType.JobTypeName, 
                            jobType.JobTypeStatus,
                            jobType.JobTypeDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert(MJobType viewModel, FormCollection formCollection)
        {
            MJobType mJobTypeToInsert = new MJobType();
            TransferFormValuesTo(mJobTypeToInsert, viewModel);
            mJobTypeToInsert.SetAssignedIdTo(viewModel.Id);
            mJobTypeToInsert.CreatedDate = DateTime.Now;
            mJobTypeToInsert.CreatedBy = User.Identity.Name;
            mJobTypeToInsert.DataStatus = EnumDataStatus.New.ToString();

            _mJobTypeRepository.Save(mJobTypeToInsert);

            try
            {
                _mJobTypeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mJobTypeRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MJobType viewModel, FormCollection formCollection)
        {
            MJobType mJobTypeToDelete = _mJobTypeRepository.Get(viewModel.Id);

            if (mJobTypeToDelete != null)
            {
                _mJobTypeRepository.Delete(mJobTypeToDelete);
            }

            try
            {
                _mJobTypeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mJobTypeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MJobType viewModel, FormCollection formCollection)
        {
            MJobType mJobTypeToUpdate = _mJobTypeRepository.Get(viewModel.Id);
            TransferFormValuesTo(mJobTypeToUpdate, viewModel);
            mJobTypeToUpdate.ModifiedDate = DateTime.Now;
            mJobTypeToUpdate.ModifiedBy = User.Identity.Name;
            mJobTypeToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

            _mJobTypeRepository.Update(mJobTypeToUpdate);

            try
            {
                _mJobTypeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mJobTypeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private static void TransferFormValuesTo(MJobType mJobTypeToUpdate, MJobType mJobTypeFromForm)
        {
            mJobTypeToUpdate.JobTypeName = mJobTypeFromForm.JobTypeName;
            mJobTypeToUpdate.JobTypeDesc = mJobTypeFromForm.JobTypeDesc;
            mJobTypeToUpdate.JobTypeStatus = mJobTypeFromForm.JobTypeStatus;
        }
    }
}
