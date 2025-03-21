namespace _2025.Services.AuthAPI.DTO
{
    public class BaseSearchDTO
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Skip => (PageIndex - 1) * PageSize;
        public int Total { get; set; }
    }
}
