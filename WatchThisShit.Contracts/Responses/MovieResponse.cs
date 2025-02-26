using System;
using WatchThisShit.Contracts.Common;

namespace WatchThisShit.Contracts.Responses;

public class MovieResponse : LinkedResource
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public required int YearOfRelease { get; init; }
    public int? UserRating { get; init; }
    public float? Rating { get; init; } = 0;
    public required IEnumerable<string> Genres { get; init; } = [];
}