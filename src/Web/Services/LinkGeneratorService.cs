using Microsoft.AspNetCore.Mvc;
using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Models;
using WatchThisShit.Web.Controllers;

namespace WatchThisShit.Web.Services;

public class LinkGeneratorService(IUrlHelper urlHelper) : ILinkGeneratorService
{
    public List<Link> GenerateMovieLinks(Guid movieId, string slug)
    {
        List<Link> links = new()
        {
            new Link(
                urlHelper.ActionLink(nameof(MoviesController.GetMovie), "Movies", new { idOrSlug = movieId })!,
                "self",
                "GET"
            ),
            new Link(
                urlHelper.ActionLink(nameof(MoviesController.GetMovie), "Movies", new { idOrSlug = slug })!,
                "self_slug",
                "GET"
            )
            // new(
            //     urlHelper.ActionLink(nameof(MoviesController.UpdateMovie), "Movies", new { id = movieId })!,
            //     "update",
            //     "PUT"
            // ),
            // new(
            //     urlHelper.ActionLink(nameof(MoviesController.DeleteMovie), "Movies", new { id = movieId })!,
            //     "delete",
            //     "DELETE"
            // )
        };

        return links;
    }

    public List<Link> GenerateMoviesPageLinks(int pageNumber, int pageSize, int totalPages,
        string? orderBy, bool isDescending)
    {
        List<Link> links = new();

        // Current page
        links.Add(new Link(
            CreateMoviesPageUrl(pageNumber, pageSize, orderBy, isDescending),
            "self",
            "GET"
        ));

        // First page
        if (pageNumber > 1)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(1, pageSize, orderBy, isDescending),
                "first",
                "GET"
            ));
        }

        // Previous page
        if (pageNumber > 1)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(pageNumber - 1, pageSize, orderBy, isDescending),
                "previous",
                "GET"
            ));
        }

        // Next page
        if (pageNumber < totalPages)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(pageNumber + 1, pageSize, orderBy, isDescending),
                "next",
                "GET"
            ));
        }

        // Last page
        if (pageNumber < totalPages)
        {
            links.Add(new Link(
                CreateMoviesPageUrl(totalPages, pageSize, orderBy, isDescending),
                "last",
                "GET"
            ));
        }

        return links;
    }

    private string CreateMoviesPageUrl(int pageNumber, int pageSize, string? orderBy, bool isDescending)
    {
        return urlHelper.ActionLink(nameof(MoviesController.GetAllMovies), "Movies",
            new { pageNumber, pageSize, orderBy, isDescending })!;
    }
}
