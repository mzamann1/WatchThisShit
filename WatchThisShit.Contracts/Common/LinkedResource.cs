namespace WatchThisShit.Contracts.Common;

public abstract class LinkedResource
{
    public List<Link> Links { get; set; } = new();
}