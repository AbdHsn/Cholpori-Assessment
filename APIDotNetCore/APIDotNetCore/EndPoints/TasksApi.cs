using DataLayer.Models.Entities;
using DataLayer.Models.Global;
using RepositoryLayer;

namespace APIDotNetCore.EndPoints
{
    public class TasksApi
    {
        #region Properties
        private readonly IEntityRepo<Tasks> _task;
        private readonly IRawQueryRepo<Tasks> _taskRawSql;
        private readonly IRawQueryRepo<TotalRecordCountGLB> _taskCountRawSql;
        #endregion

        #region Constructor
        public TasksApi(
           IEntityRepo<Tasks> task,
           IRawQueryRepo<Tasks> taskRawSql,
           IRawQueryRepo<TotalRecordCountGLB> taskCountRawSql
        )
        {
            _task = task;
            _taskRawSql = taskRawSql;
            _taskCountRawSql = taskCountRawSql;
        }
        #endregion

        public async Task TaskAPIEndPoints(WebApplication app)
        {
            app.MapGet("/task-api/get", async () =>
            {
                try
                {
                    return Results.Ok(await _task.GetAll());
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return Results.BadRequest("Failed to proceed.");
                }

            });
            
            app.MapPost("/task-api/get-grid", async (DatatableGLB tableObj) =>
            {
                try
                {
                    int rowSize = 0;
                    if (tableObj.length == "All")
                    {
                        rowSize = 0;
                    }
                    else
                    {
                        rowSize = int.Parse(tableObj.length);
                    }

                    string searchText = default(string);
                    if (tableObj.search != null)
                    {
                        searchText = tableObj.search.value;
                    }

                    #region single sort gathering code
                    string sortInformation = null;
                    if (tableObj.orders != null && tableObj.orders.Count > 0)
                    {
                        var getSort = tableObj.orders.FirstOrDefault();
                        sortInformation = getSort.column + " " + getSort.order_by;
                    }
                    else
                    {
                        //assign default sort info base on column
                        sortInformation = "Id DESC";
                    }


                    #endregion single sort code

                    #region where-condition gathering code
                    string whereConditionStatement = null;
                    if (tableObj != null && tableObj.searches.Count() > 0)
                    {
                        foreach (var item in tableObj.searches)
                        {
                            if (!string.IsNullOrEmpty(item.value))
                                whereConditionStatement += item.search_by + " = '" + item.value + "' AND ";
                        }
                        if (!string.IsNullOrEmpty(whereConditionStatement))
                        {
                            whereConditionStatement = whereConditionStatement.Substring(0, whereConditionStatement.Length - 4);
                        }
                    }
                    #endregion where-condition gathering code

                    #region database query code 

                    var dataGrid = await _taskRawSql.GetAllByWhere(new GetAllByWhereGLB()
                    {
                        TableOrViewName = "Tasks",
                        SortColumn = sortInformation,
                        WhereConditions = whereConditionStatement,
                        LimitStart = tableObj.start,
                        LimitEnd = rowSize
                    });

                    var dataGridCount = await _taskCountRawSql.CountAllByWhere(new CountAllByWhereGLB()
                    {
                        TableOrViewName = "Tasks",
                        WhereConditions = whereConditionStatement
                    });

                    #endregion database query code

                    return Results.Ok(new {
                        data = dataGrid,
                        totalRecords = dataGridCount.TotalRecord
                    });
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return Results.BadRequest("Failed to proceed.");
                }
            });

            app.MapGet("/task-api/get/{id}", async (long id) =>
            {
                try
                {
                    if (id <= 0)
                        return Results.BadRequest("Request is not valid.");

                    var getTask = await _task.GetById(x => x.id == id);

                    if (getTask == null)
                    {
                        return Results.NotFound("Requested item is not found.");
                    }
                    return Results.Ok(getTask);
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return Results.BadRequest("Failed to proceed.");
                }
            });

            app.MapPost("/task-api/create", async (Tasks task) =>
            {
                try
                {
                    if (task == null)
                        return Results.BadRequest("Provide valid data.");

                    return Results.Ok(await _task.Insert(task));
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return Results.BadRequest("Failed to proceed.");
                }

            });

            app.MapPut("/task-api/update", async (Tasks task) =>
            {
                try
                {

                    if (task.id <= 0)
                        return Results.BadRequest("Provide valid data.");

                    var getTask = await _task.GetById(x => x.id == task.id);

                    if (getTask == null)
                    {
                        return Results.NotFound("Requested item is not found.");
                    }

                    getTask.title = task.title;
                    getTask.details = task.title;
                    getTask.progress_ratio = task.progress_ratio;

                    return Results.Ok(await _task.Update(getTask));
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return Results.BadRequest("Failed to proceed.");
                }
            });

            app.MapDelete("/task-api/delete/{id}", async (long id) =>
            {
                try
                {
                    if (id <= 0)
                        return Results.BadRequest("Data is not valid.");

                    var getTask = await _task.GetById(x => x.id == id);

                    if (getTask == null)
                    {
                        return Results.NotFound("Requested item is not found.");
                    }

                    return Results.Ok(await _task.Delete(getTask));
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                    return Results.BadRequest("Failed to proceed.");
                }

            });
        }
    }
}
