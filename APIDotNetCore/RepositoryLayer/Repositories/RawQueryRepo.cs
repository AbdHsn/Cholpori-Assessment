using DataLayer.Models.Global;
using Microsoft.EntityFrameworkCore;

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
        #endregion "Get Methods Implementation"
    }
}