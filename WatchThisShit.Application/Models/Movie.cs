using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WatchThisShit.Application.Models;

public partial class Movie
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string Slug => GenerateSlug();


    public required int YearOfRelease { get; init; }
    public required List<string> Genres { get; init; } = [];

    private string GenerateSlug()
    {
        var slug = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
        return $"{slug}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}