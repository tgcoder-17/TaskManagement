using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Common
{
    /// <summary>
    /// Common pagination and sorting parameters
    /// </summary>
    public class PagedRequestDto
    {
        /// <summary>
        /// Page number (starting from 1)
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of records per page
        /// </summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Sort by CreatedAt (asc or desc)
        /// </summary>
        [RegularExpression("^(asc|desc)$", ErrorMessage = "SortOrder must be 'asc' or 'desc'")]
        public string SortOrder { get; set; } = "desc";
    }
}
