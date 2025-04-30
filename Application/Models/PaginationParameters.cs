namespace Application.Models
{
    public class PaginationParameters
    {
        private const int MaxPageSize = 100;

        private int pageNumber = 1;
        private int pageSize = 10;

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value <= 0 ? 1 : value;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value <= 0 ? 10 : (value > MaxPageSize ? MaxPageSize : value);
        }

        public string? OrderBy { get; set; }
        public bool OrderDescending { get; set; } = false;

        public int ValidatedPageNumber => PageNumber <= 0 ? 1 : PageNumber;
        public int ValidatedPageSize => PageSize <= 0 ? 10 : (PageSize > MaxPageSize ? MaxPageSize : PageSize);
    }
}
