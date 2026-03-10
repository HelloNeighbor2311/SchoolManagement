namespace SchoolManagement.Models
{
    public class PageResult<T>
    {
        public List<T>? Items { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public bool hasPrevious => PageNumber > 1;
        public bool hasNext => PageNumber < TotalPage;

        public PageResult(List<T> items, int count, int pageNum, int pageSize)
        {
            Items = items;
            PageSize = pageSize;
            PageNumber = pageNum;
            TotalPage = (int)Math.Ceiling((double)count / pageSize);
        }
    }
}
