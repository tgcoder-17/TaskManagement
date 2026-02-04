namespace TaskManagement.API.Common
{
    /// <summary>
    /// Standard paginated response
    /// </summary>
    public class PagedResponseDto<T>
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Number of records per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Total number of records
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);

        /// <summary>
        /// List of items in the current page
        /// </summary>
        public List<T> Items { get; set; } = new();
    }
}
