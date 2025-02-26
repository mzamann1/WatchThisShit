namespace WatchThisShit.Application.Common.Interfaces.Filtering;

public interface IPagination
{
    int PageNumber { get; init; }
    int PageSize { get; init; }
}

public interface ISorting
{
    string OrderBy { get; init; }
    bool IsDescending { get; init; }
}

public interface IFiltering
{
    string SearchTerm { get; init; }
}
