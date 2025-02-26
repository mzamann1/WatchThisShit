

using WatchThisShit.Contracts.Common;

namespace WatchThisShit.API.Services;

public interface ILinkGeneratorService
{
    List<Link> GenerateMovieLinks(Guid movieId, string slug);
    List<Link> GenerateMoviesPageLinks(int pageNumber, int pageSize, int totalPages, string? sortBy, bool isDescending);
}

