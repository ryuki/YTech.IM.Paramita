using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Serialization;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Data.Repository;
using YTech.IM.Paramita.Enums;
using YTech.IM.Paramita.Web.Controllers.Helper;

namespace YTech.IM.Paramita.Web.Controllers.Master
{
    [HandleError]
    public class AccountController : Controller
    {
        public AccountController() : this(new MAccountRepository() ,new MAccountCatRepository())
        {}

        private readonly IMAccountRepository _mAccountRepository;
        private readonly IMAccountCatRepository _mAccountCatRepository;

        public AccountController(IMAccountRepository mAccountRepository, IMAccountCatRepository mAccountCatRepository)
        {
            Check.Require(mAccountRepository != null, "mAccountRepository may not be null");
            Check.Require(mAccountCatRepository != null, "mAccountCatRepository may not be null");

            this._mAccountRepository = mAccountRepository;
            this._mAccountCatRepository = mAccountCatRepository;
        }


        public ActionResult Index()
        {
            IList<MAccountCat> list = _mAccountCatRepository.GetAll();
            ViewData["AccountCatList"] = new SelectList(list, "Id", "AccountCatName");
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string accountCatId)
        {
            MAccountCat accountCat = _mAccountCatRepository.Get(accountCatId);

            int totalRecords = 0;
            var accounts = _mAccountRepository.GetPagedAccountList(sidx, sord, page, 0, ref totalRecords, accountCat);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            string level;

            //var jsonData = new
            //{
            //    total = totalPages,
            //    page = page,
            //    records = totalRecords,
            //    rows = (
            //        from itemCat in itemCats
            //        //where itemCat.AccountParentId == null
            //        select new
            //        {
            //            i = itemCat.Id.ToString(),
            //            cell = new string[] {
            //                itemCat.Id, 
            //                itemCat.AccountName, 
            //                itemCat.AccountDesc,
            //                 "0"

            //            }
            //        }).ToArray()
            //};

            IEnumerable<MAccount> result = new List<MAccount>();
            result = Helper.Extensions<MAccount>.Traverse(accounts, i => i.Children);
            //result = accounts.Traverse(i => i.Children);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from acc in result
                    select new
                    {
                        i = acc.Id.ToString(),
                        cell = new string[] {
                            acc.Id, 
                            acc.Id, 
                            acc.AccountName, 
                            acc.AccountParentId != null ? acc.AccountParentId.Id : null,
                            //acc.AccountParentId != null ? acc.AccountParentId.AccountName : null,
                            acc.AccountDesc,
                            GetLevel(acc,true).ToString(),
                            acc.AccountParentId != null ? string.Format("<![CDATA[{0}]]>", acc.AccountParentId.Id) : "NULL",
                           // acc.Children.Count == 0  ? true.ToString() : false.ToString(),
                           //acc.AccountParentId != null ? false.ToString():true.ToString(),
                           true.ToString(),
                            true.ToString()

                        }
                    }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private int _lvl = 0;
        private int GetLevel(MAccount acc, bool firstTime)
        {
            if (firstTime)
                _lvl = 0;
            if (acc.AccountParentId != null)
            {
                _lvl++;
                MAccount accParent = _mAccountRepository.Get(acc.AccountParentId.Id);
                if (accParent != null)
                    if (accParent.AccountParentId != null)
                        GetLevel(accParent, false);
            }
            return _lvl;
        }

        [Transaction]
        public ActionResult Insert(MAccount viewModel, FormCollection formCollection)
        {

            MAccount mCompanyToInsert = new MAccount();
            TransferFormValuesTo(mCompanyToInsert, viewModel);
            mCompanyToInsert.AccountParentId = _mAccountRepository.Get(formCollection["ParentId"]);
            mCompanyToInsert.SetAssignedIdTo(viewModel.Id);
            mCompanyToInsert.CreatedDate = DateTime.Now;
            mCompanyToInsert.CreatedBy = User.Identity.Name;
            mCompanyToInsert.DataStatus = EnumDataStatus.New.ToString();
            _mAccountRepository.Save(mCompanyToInsert);

            try
            {
                _mAccountRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mAccountRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MAccount viewModel, FormCollection formCollection)
        {
            MAccount mCompanyToDelete = _mAccountRepository.Get(viewModel.Id);

            if (mCompanyToDelete != null)
            {
                _mAccountRepository.Delete(mCompanyToDelete);
            }

            try
            {
                _mAccountRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mAccountRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MAccount viewModel, FormCollection formCollection)
        {
            MAccount mCompanyToUpdate = _mAccountRepository.Get(viewModel.Id);
            TransferFormValuesTo(mCompanyToUpdate, viewModel);
            mCompanyToUpdate.AccountParentId = _mAccountRepository.Get(formCollection["ParentId"]);
            mCompanyToUpdate.ModifiedDate = DateTime.Now;
            mCompanyToUpdate.ModifiedBy = User.Identity.Name;
            mCompanyToUpdate.DataStatus = EnumDataStatus.Updated.ToString();
            _mAccountRepository.Update(mCompanyToUpdate);

            try
            {
                _mAccountRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mAccountRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MAccount mCompanyToUpdate, MAccount mCompanyFromForm)
        {
            mCompanyToUpdate.AccountName = mCompanyFromForm.AccountName;
            mCompanyToUpdate.AccountDesc = mCompanyFromForm.AccountDesc;
            mCompanyToUpdate.AccountCatId = mCompanyFromForm.AccountCatId;
            mCompanyToUpdate.AccountParentId = mCompanyFromForm.AccountParentId;
            mCompanyToUpdate.AccountStatus = mCompanyFromForm.AccountStatus;
        }


        [Transaction]
        public virtual ActionResult GetList(string accountCatId)
        {
            IList<MAccount> accounts;
            if (!string.IsNullOrEmpty(accountCatId))
            {
                MAccountCat accountCat = _mAccountCatRepository.Get(accountCatId);
                accounts = _mAccountRepository.GetByAccountCat(accountCat);
            }
            else
            {
                accounts = _mAccountRepository.GetAll();
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1}", string.Empty, "-Pilih Akun-");
            foreach (MAccount mAccount in accounts)
            {
                sb.AppendFormat(";{0}:{1}", mAccount.Id, mAccount.AccountName);
            }
            return Content(sb.ToString());
        }

    }
}
