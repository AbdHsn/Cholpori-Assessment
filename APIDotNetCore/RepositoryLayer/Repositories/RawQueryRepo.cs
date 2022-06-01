using DataLayer.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class RawQueryRepo<T> : IRawQueryRepo<T> where T : class
    {
        #region "Variables"
        private readonly EntityContext _context;
        #endregion "Variables"

        #region "Constructors"
        public RawQueryRepo(EntityContext context)
        {
            this._context = context;
        }

        #endregion "Constructors"

        #region "Get Methods Implementation"
        public async Task<List<T>> GetAllByWhere(GetAllByWhereGLB getAllByWhereGLB)
        {
            string sql = default(string);
            if (string.IsNullOrEmpty(getAllByWhereGLB.WhereConditions))
            {
                //mssql
                //sql = string.Format("SELECT * FROM {0} ORDER BY {1} OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY",
                //    getAllByWhereGLB.TableOrViewName, getAllByWhereGLB.SortColumn, getAllByWhereGLB.LimitStart, getAllByWhereGLB.LimitEnd);

                if (getAllByWhereGLB.LimitEnd == 0)
                {
                    //mysql
                    sql = string.Format("SELECT * FROM {0} ORDER BY {1}",
                        getAllByWhereGLB.TableOrViewName, getAllByWhereGLB.SortColumn);

                }
                else {
                    //mysql
                    sql = string.Format("SELECT * FROM {0} ORDER BY {1} LIMIT {2}, {3}",
                        getAllByWhereGLB.TableOrViewName, getAllByWhereGLB.SortColumn, getAllByWhereGLB.LimitStart, getAllByWhereGLB.LimitEnd);

                }


            }
            else
            {
                //mssql
                //sql = string.Format("SELECT * FROM {0} WHERE {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY",
                //      getAllByWhereGLB.TableOrViewName, getAllByWhereGLB.WhereConditions, getAllByWhereGLB.SortColumn, getAllByWhereGLB.LimitStart, getAllByWhereGLB.LimitEnd);

                if (getAllByWhereGLB.LimitEnd == 0)
                {
                    //mysql
                    sql = string.Format("SELECT * FROM {0} WHERE {1} ORDER BY {2}",
                          getAllByWhereGLB.TableOrViewName, getAllByWhereGLB.WhereConditions, getAllByWhereGLB.SortColumn);

                }
                else {
                    //mysql
                    sql = string.Format("SELECT * FROM {0} WHERE {1} ORDER BY {2} LIMIT {3}, {4}",
                          getAllByWhereGLB.TableOrViewName, getAllByWhereGLB.WhereConditions, getAllByWhereGLB.SortColumn, getAllByWhereGLB.LimitStart, getAllByWhereGLB.LimitEnd);

                }

            }

            var returnData = await _context.Set<T>().FromSqlRaw(sql).AsNoTracking().ToListAsync();
            return returnData;
        }
        
        public async Task<List<T>> ExportAllByWhere(ExportAllByWhereGLB exportAllByWhereGLB)
        {
            string sql = default(string);
            if (string.IsNullOrEmpty(exportAllByWhereGLB.WhereConditions))
            {
                sql = string.Format("SELECT * FROM {0} ORDER BY {1}",
                    exportAllByWhereGLB.TableOrViewName, exportAllByWhereGLB.SortColumn);
            }
            else
            {
                sql = string.Format("SELECT * FROM {0} WHERE {1} ORDER BY {2}",
                      exportAllByWhereGLB.TableOrViewName, exportAllByWhereGLB.WhereConditions, exportAllByWhereGLB.SortColumn);
            }

            return await _context.Set<T>().FromSqlRaw(sql).AsNoTracking().ToListAsync();
        }

        public async Task<T> CountAllByWhere(CountAllByWhereGLB countAllByWhereGLB )
        {
            string sql = default(string);
            if (string.IsNullOrWhiteSpace(countAllByWhereGLB.WhereConditions))
            {
                sql =  string.Format("SELECT Count(Id) AS TotalRecord FROM {0}", countAllByWhereGLB.TableOrViewName);
            }
            else
            {
                sql = string.Format("SELECT Count(Id) AS TotalRecord FROM {0} WHERE {1}", countAllByWhereGLB.TableOrViewName, countAllByWhereGLB.WhereConditions);
            }
            return await _context.Set<T>().FromSqlRaw(sql).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Object> GetAllByLike(GetAllByLikeGLB getAllByLikeGLB)
        {
            string sql = default(string);
            if (string.IsNullOrEmpty(getAllByLikeGLB.WhereConditions))
            {
                //mssql
                //sql = string.Format("SELECT DISTINCT TOP ({0}) {1} FROM {2} WHERE {3} LIKE('%{4}%') ORDER BY {5} {6}",
                //    getAllByLikeGLB.NumberOfReturnRow, getAllByLikeGLB.ColumnName, getAllByLikeGLB.TableOrViewName, getAllByLikeGLB.ColumnName, getAllByLikeGLB.ColumnValue, getAllByLikeGLB.ColumnName, getAllByLikeGLB.OrderBy);
            
                //mysql
                sql = string.Format("SELECT DISTINCT {0} FROM {1} WHERE {2} LIKE('%{3}%') ORDER BY {4} {5} LIMIT {6}",
                    getAllByLikeGLB.ColumnName, getAllByLikeGLB.TableOrViewName, getAllByLikeGLB.ColumnName, getAllByLikeGLB.ColumnValue, getAllByLikeGLB.ColumnName, getAllByLikeGLB.OrderBy, getAllByLikeGLB.NumberOfReturnRow);
            
            
            }
            else
            {
                //mssql
                //sql = string.Format("SELECT DISTINCT TOP ({0}) {1} FROM {2} WHERE {3} LIKE('%{4}%') AND ({5}) ORDER BY {6} {7}",
                //    getAllByLikeGLB.NumberOfReturnRow, getAllByLikeGLB.ColumnName, getAllByLikeGLB.TableOrViewName, getAllByLikeGLB.ColumnName, getAllByLikeGLB.ColumnValue, getAllByLikeGLB.WhereConditions, getAllByLikeGLB.ColumnName, getAllByLikeGLB.OrderBy);
           
                //mysql
                sql = string.Format("SELECT DISTINCT {0} FROM {1} WHERE {2} LIKE('%{3}%') AND ({4}) ORDER BY {5} {6} LIMIT {7}",
                    getAllByLikeGLB.ColumnName, getAllByLikeGLB.TableOrViewName, 
                    getAllByLikeGLB.ColumnName, getAllByLikeGLB.ColumnValue, 
                    getAllByLikeGLB.WhereConditions, getAllByLikeGLB.ColumnName, 
                    getAllByLikeGLB.OrderBy, getAllByLikeGLB.NumberOfReturnRow);
            }

            List<Object> list = new List<Object>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                _context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            list.Add(result[getAllByLikeGLB.ColumnName]);
                        }
                    }
                }
            }

            return list;
        }
        #endregion "Get Methods Implementation"

        #region CAll SP Functions
        public async Task<List<T>> ExecuteStoreProcedure(string storeProcedure)
        {
            string sp = $"CALL {storeProcedure}";
            return await _context.Set<T>().FromSqlRaw(sp).AsNoTracking().ToListAsync();
        }

        #endregion

    }
}