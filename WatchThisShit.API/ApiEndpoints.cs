namespace WatchThisShit.API;

public static class ApiEndpoints
{
    private const string ApiBaseUrl = "api";

    public static class Movies
    {
        private const string BaseUrlMovie = $"{ApiBaseUrl}/movies";
        public const string Create = BaseUrlMovie;
        public const string Get = $"{BaseUrlMovie}/{{idOrSlug}}";
        public const string GetAll = BaseUrlMovie;
        public const string Update = $"{BaseUrlMovie}/{{id:guid}}";
        public const string Delete = $"{BaseUrlMovie}/{{id:guid}}";
    }
}