using WatchThisShit.Application.Common.Interfaces.Filtering;

namespace WatchThisShit.Application.Common.Models.Requests;

public class PaginationFilter : IPagination, ISorting, IFiltering
{
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;

    private readonly int _pageSize = DefaultPageSize;

    public string SearchTerm { get; init; } = string.Empty;

    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Min(value, MaxPageSize);
    }

    public string OrderBy { get; init; } = "id"; // Default sorting by Id
    public bool IsDescending { get; init; } = false;
}
