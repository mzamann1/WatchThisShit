using Microsoft.AspNetCore.Mvc;
using WatchThisShit.API.Controllers;
using WatchThisShit.Contracts.Common;

namespace WatchThisShit.API.Services;

public class LinkGeneratorService : ILinkGeneratorService
{
    private readonly IUrlHelper _urlHelper;

    public LinkGeneratorService(IUrlHelper urlHelper)
    {
        _urlHelper = urlHelper;
    }

    public List<Link> GenerateMovieLinks(Guid movieId, string slug)
    {
        var links = new List<Link>
        {
            new(
                _urlHelper.ActionLink(nameof(MoviesController.GetMovie), "Movies", new { idOrSlug = movieId })!,
                "self",
                "GET"
            ),
            new(
                _urlHelper.ActionLink(nameof(MoviesController.GetMovie), "Movies", new { idOrSlug = slug })!,
                "self_slug",
                "GET"
            ),
            new(
                _urlHelper.ActionLink(nameof(MoviesController.UpdateMovie), "Movies", new { id = movieId })!,
                "update",
                "PUT"
            ),
            new(
                _urlHelper.ActionLink(nameof(MoviesController.DeleteMovie), "Movies", new { id = movieId })!,
                "delete",
                "DELETE"
            )
        };

        return links;
    }

    public List<Link> GenerateMoviesPageLinks(int pageNumber, int pageSize, int totalPages,
        string? sortBy, bool isDescending)
    {
        var links = new List<Link>();

        // Current page
        links.Add(new Link(
            CreateMoviesPageUrl(pageNumber, pageSize, sortBy, isDescending),
            "self",
            "GET"
        ));

        // First page
        if (pageNumber > 1)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(1, pageSize, sortBy, isDescending),
                "first",
                "GET"
            ));
        }

        // Previous page
        if (pageNumber > 1)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(pageNumber - 1, pageSize, sortBy, isDescending),
                "previous",
                "GET"
            ));
        }

        // Next page
        if (pageNumber < totalPages)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(pageNumber + 1, pageSize, sortBy, isDescending),
                "next",
                "GET"
            ));
        }

        // Last page
        if (pageNumber < totalPages)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(totalPages, pageSize, sortBy, isDescending),
                "last",
                "GET"
            ));
        }

        return links;
    }

    private string CreateMoviesPageUrl(int pageNumber, int pageSize, string? sortBy, bool isDescending)
    {
        return _urlHelper.ActionLink(nameof(MoviesController.GetAllMovies), "Movies", new
        {
            pageNumber,
            pageSize,
            sortBy,
            isDescending
        })!;
    }
}