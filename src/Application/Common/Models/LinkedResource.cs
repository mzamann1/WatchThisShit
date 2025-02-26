namespace WatchThisShit.Application.Common.Models;

public abstract class LinkedResource
{
    public List<Link> Links { get; set; } = new();
}
