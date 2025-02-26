using WatchThisShit.Application.Common.Models;

namespace WatchThisShit.Application.Common.Interfaces.Services;

public interface ILinkGeneratorService
{
    List<Link> GenerateMovieLinks(Guid movieId, string slug);
    List<Link> GenerateMoviesPageLinks(int pageNumber, int pageSize, int totalPages, string? sortBy, bool isDescending);
}
