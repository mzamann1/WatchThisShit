namespace WatchThisShit.Application.Movies.Queries.GetMovieById;

public class GetMovieByIdQueryValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdQueryValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithMessage("Id is required");
    }
}
