namespace WatchThisShit.Application.FilterModels;

public class PaginationFilter
{
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;
    
    public int PageNumber { get; init; } = 1;
    
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Min(value, MaxPageSize);
    }
}