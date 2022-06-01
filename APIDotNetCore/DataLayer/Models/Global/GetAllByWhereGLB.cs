using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.Global
{
    public class GetAllByWhereGLB : CommonProperties
    {
        public string SortColumn { get; set; }
        public int LimitStart { get; set; }
        public dynamic LimitEnd { get; set; }

        //public GetAllByWhereGLB()
        //{
        //    if (LimitEnd is string)
        //    {
        //        this.LimitEnd = 0;
        //    }
        //}
    }
}
