using FluentValidation;
using WatchThisShit.Application.Models;
using WatchThisShit.Application.Repositories;

namespace WatchThisShit.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;

    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(movie => movie.Id).NotEmpty().NotNull();
        RuleFor(movie => movie.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(movie => movie.YearOfRelease).LessThanOrEqualTo(DateTime.Now.Year);
        RuleFor(movie => movie.Genres).NotEmpty().WithMessage("Genre is required");
        RuleFor(movie => movie.Slug).MustAsync(ValidateSlug).WithMessage("Movie already exists");
    }

    private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken arg3)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug);
        if (existingMovie is not null)
            return existingMovie.Id == movie.Id;
        return existingMovie is null;
    }
}