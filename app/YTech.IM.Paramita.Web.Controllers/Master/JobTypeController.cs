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
        private readonly IMJobTypeRepository _mJobTypeRepository;
        private readonly IMAccountRefRepository _mAccountRefRepository;
        private readonly IMAccountRepository _mAccountRepository;
        public JobTypeController(IMJobTypeRepository mJobTypeRepository, IMAccountRefRepository mAccountRefRepository, IMAccountRepository mAccountRepository)
        {
            Check.Require(mJobTypeRepository != null, "mJobTypeRepository may not be null");
            Check.Require(mAccountRefRepository != null, "mAccountRefRepository may not be null");
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");

            _mJobTypeRepository = mJobTypeRepository;
            this._mAccountRefRepository = mAccountRefRepository;
            this._mAccountRepository = mAccountRepository;
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
                             GetAccountRef(jobType.Id) != null ? GetAccountRef(jobType.Id).AccountId.Id : null,
                         GetAccountRef(jobType.Id) != null ? GetAccountRef(jobType.Id).AccountId.AccountName : null,
                            jobType.JobTypeDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private MAccountRef GetAccountRef(string jobTypeId)
        {
            MAccountRef accountRef = _mAccountRefRepository.GetByRefTableId(EnumReferenceTable.JobType, jobTypeId);
            if (accountRef != null)
            {
                return accountRef;
            }
            return null;
        }

        [Transaction]
        public ActionResult Insert(MJobType viewModel, FormCollection formCollection)
        {
            try
            {
                _mJobTypeRepository.DbContext.BeginTransaction();

                MJobType mJobTypeToInsert = new MJobType();
                TransferFormValuesTo(mJobTypeToInsert, viewModel);
                mJobTypeToInsert.SetAssignedIdTo(viewModel.Id);
                mJobTypeToInsert.CreatedDate = DateTime.Now;
                mJobTypeToInsert.CreatedBy = User.Identity.Name;
                mJobTypeToInsert.DataStatus = EnumDataStatus.New.ToString();

                _mJobTypeRepository.Save(mJobTypeToInsert);

                MAccountRef accountRef = new MAccountRef();
                accountRef.SetAssignedIdTo(Guid.NewGuid().ToString());
                accountRef.ReferenceId = mJobTypeToInsert.Id;
                accountRef.ReferenceTable = EnumReferenceTable.JobType.ToString();
                accountRef.ReferenceType = EnumReferenceTable.JobType.ToString();
                accountRef.AccountId = _mAccountRepository.Get(formCollection["AccountId"]);
                accountRef.CreatedDate = DateTime.Now;
                accountRef.CreatedBy = User.Identity.Name;
                accountRef.DataStatus = EnumDataStatus.New.ToString();
                _mAccountRefRepository.Save(accountRef);
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
            try
            {
                _mJobTypeRepository.DbContext.BeginTransaction();

                MJobType mJobTypeToUpdate = _mJobTypeRepository.Get(viewModel.Id);
                TransferFormValuesTo(mJobTypeToUpdate, viewModel);
                mJobTypeToUpdate.ModifiedDate = DateTime.Now;
                mJobTypeToUpdate.ModifiedBy = User.Identity.Name;
                mJobTypeToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

                _mJobTypeRepository.Update(mJobTypeToUpdate);

                bool isSave = false;
                MAccountRef accountRef = GetAccountRef(mJobTypeToUpdate.Id);
                if (accountRef == null)
                {
                    accountRef = new MAccountRef();
                    accountRef.SetAssignedIdTo(Guid.NewGuid().ToString());
                    accountRef.CreatedDate = DateTime.Now;
                    accountRef.CreatedBy = User.Identity.Name;
                    accountRef.DataStatus = EnumDataStatus.New.ToString();
                    isSave = true;
                }
                else
                {
                    accountRef.ModifiedDate = DateTime.Now;
                    accountRef.ModifiedBy = User.Identity.Name;
                    accountRef.DataStatus = EnumDataStatus.Updated.ToString();
                }
                accountRef.ReferenceId = mJobTypeToUpdate.Id;
                accountRef.ReferenceTable = EnumReferenceTable.JobType.ToString();
                accountRef.ReferenceType = EnumReferenceTable.JobType.ToString();
                accountRef.AccountId = _mAccountRepository.Get(formCollection["AccountId"]);
                if (isSave)
                {
                    _mAccountRefRepository.Save(accountRef);
                }
                else
                {
                    _mAccountRefRepository.Update(accountRef);

                }
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
