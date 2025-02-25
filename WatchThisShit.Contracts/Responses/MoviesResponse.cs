using System.Collections.Generic;

namespace WatchThisShit.Contracts.Responses;

public class MoviesResponse
{
    public required IEnumerable<MovieResponse> Items { get; init; } = [];
}