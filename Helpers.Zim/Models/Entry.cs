namespace Helpers.Zim.Models;

public readonly record struct Entry(string Id, string Title, DateTime Updated, string Name, string? Flavor, Uri Link);
