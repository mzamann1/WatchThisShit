namespace WatchThisShit.Application.Common.Models;

public class Link
{
    public Link(string href, string rel, string method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }

    public string Href { get; set; }
    public string Rel { get; set; }
    public string Method { get; set; }
}
