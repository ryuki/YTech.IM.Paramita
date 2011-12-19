using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.IM.Paramita.Core.Master;
using YTech.IM.Paramita.Core.RepositoryInterfaces;
using YTech.IM.Paramita.Core.Transaction;
using YTech.IM.Paramita.Core.Transaction.Inventory;

namespace YTech.IM.Paramita.Data.Repository
{
    public class TTransRepository : NHibernateRepositoryWithTypedId<TTrans, string>, ITTransRepository
    {
        public IList<TTrans> GetByWarehouseStatusTransBy(MWarehouse warehouse, string transStatus, string transBy)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TTrans));
            criteria.Add(Expression.Eq("TransStatus", transStatus));
            criteria.Add(Expression.Eq("WarehouseId", warehouse));
            criteria.Add(Expression.Eq("TransBy", transBy));
            criteria.SetCacheable(true);
            IList<TTrans> list = criteria.List<TTrans>();
            return list;
        }

        public IEnumerable<TTrans> GetPagedTransList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText, string transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TTrans as t
                                where 1=1 ");
            if (!string.IsNullOrEmpty(transStatus))
            {
                sql.AppendLine(@"   and t.TransStatus = :transStatus ");
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@"   and t.{0} like :searchText", searchBy).AppendLine();
            }

            string queryCount = string.Format(" select count(t.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            totalRows = Convert.ToInt32(q.UniqueResult());

            sql.AppendLine(@"   order by t.TransDate desc  ");
            string query = string.Format(" select t {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TTrans> list = q.List<TTrans>();
            return list;
        }

        public IEnumerable<TTrans> GetPagedTransNotRefList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText, string transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TTrans as t
                                where t.Id not in (
                                    select r.TransIdRef.Id from TTransRef r
                                    ) ");
            if (!string.IsNullOrEmpty(transStatus))
            {
                sql.AppendLine(@"   and t.TransStatus = :transStatus ");
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@"   and t.{0} like :searchText", searchBy).AppendLine();
            }

            string queryCount = string.Format(" select count(t.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            totalRows = Convert.ToInt32(q.UniqueResult());

            sql.AppendLine(@"   order by t.TransDate desc  ");
            string query = string.Format(" select t {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TTrans> list = q.List<TTrans>();
            return list;
        }

        public IList<TTrans> GetTransNotRefList(string warehouseId, string transStatus, string transBy)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TTrans as t
                                where t.Id not in (
                                    select r.TransIdRef.Id from TTransRef r
                                    ) ");
            if (!string.IsNullOrEmpty(warehouseId))
            {
                sql.AppendLine(@"   and t.WarehouseId.Id = :warehouseId ");
            }
            if (!string.IsNullOrEmpty(transStatus))
            {
                sql.AppendLine(@"   and t.TransStatus = :transStatus ");
            }
            if (!string.IsNullOrEmpty(transBy))
            {
                sql.AppendLine(@"   and t.TransBy = :transBy ");
            }

            sql.AppendLine(@"   order by t.TransDate desc  ");
            string query = string.Format(" select t {0}", sql);
            IQuery q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(warehouseId))
            {
                q.SetString("warehouseId", warehouseId);
            }
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(transBy))
            {
                q.SetString("transBy", transBy);
            }
            IList<TTrans> list = q.List<TTrans>();
            return list;
        }

        public IEnumerable<TTrans> GetPagedTransNotPaidList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText, string transStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TTrans as t
                                where t.Id not in (
                                    select p.TransId.Id from TPaymentDet p
                                    ) ");
            if (!string.IsNullOrEmpty(transStatus))
            {
                sql.AppendLine(@"   and t.TransStatus = :transStatus ");
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                sql.AppendFormat(@"   and t.{0} like :searchText", searchBy).AppendLine();
            }

            string queryCount = string.Format(" select count(t.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            totalRows = Convert.ToInt32(q.UniqueResult());

            sql.AppendLine(@"   order by t.TransDate desc  ");
            string query = string.Format(" select t {0}", sql);
            q = Session.CreateQuery(query);
            if (!string.IsNullOrEmpty(transStatus))
            {
                q.SetString("transStatus", transStatus);
            }
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchText))
            {
                q.SetString("searchText", string.Format("%{0}%", searchText));
            }
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TTrans> list = q.List<TTrans>();
            return list;
        }
    }
}
