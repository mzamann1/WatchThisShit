namespace WatchThisShit.Application.Movies.Queries.GetMovieBySlug;

public class GetMovieBySlugQueryValidator : AbstractValidator<GetMovieBySlugQuery>
{
    public GetMovieBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotNull().NotEmpty();       
    }
}
