using WatchThisShit.Application.Common.Interfaces.Repositories;
using WatchThisShit.Domain.Entities;

namespace WatchThisShit.Application.Movies.Commands.CreateMovie;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    private readonly IMovieRepository _movieRepository;

    public CreateMovieCommandValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(movie => movie.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(movie => movie.YearOfRelease).LessThanOrEqualTo(DateTime.Now.Year);
        RuleFor(movie => movie.Genres).NotEmpty().WithMessage("Genre is required");
    }

    private async Task<bool> ValidateSlug(CreateMovieCommand movie, string slug, CancellationToken cancellationToken)
    {
        Movie? existingMovie = await _movieRepository.GetBySlugAsync(slug, cancellationToken);
        return existingMovie is null;
    }
}
