namespace _2025.Services.AuthAPI.DTO
{
    public class ResponseDataDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public MetaData metaData { get; set; }
    }

    public class MetaData
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int PageTotal => PageSize == 0? 0: (int)Math.Ceiling((decimal)Total / PageSize);

    }
}
