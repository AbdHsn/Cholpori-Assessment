namespace DataLayer.Models.Global
{
    public class DatatableResponseGLB
    {
        public object data { get; set; }
        public object sumdata { get; set; }
        public int draw { get; set; }
        public long totalFilteredRecords { get; set; }
        public long totalRecords { get; set; }
        public string error { get; set; }
    }
}
