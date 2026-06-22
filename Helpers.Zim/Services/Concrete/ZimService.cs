using Helpers.Zim.Clients;
using Helpers.Zim.Models.Generated;
using System.Runtime.CompilerServices;

namespace Helpers.Zim.Services.Concrete;

public class ZimService(IZimClient client) : IZimService
{
	public async IAsyncEnumerable<entryType> GetEntriesAsync(string name, string? flavor = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);

		await foreach (var entry in GetEntriesAsync(cancellationToken))
		{
			if (string.Equals(entry.name, name, StringComparison.OrdinalIgnoreCase))
			{
				if (string.IsNullOrEmpty(flavor)
					|| string.Equals(entry.flavour, flavor, StringComparison.OrdinalIgnoreCase))
				{
					yield return entry;
				}
			}
		}
	}

	public IAsyncEnumerable<entryType> GetEntriesAsync(CancellationToken cancellationToken = default)
		=> client.GetEntriesAsync(cancellationToken);

	public IAsyncEnumerable<Uri> GetUrisAsync(entryType entry, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entry);
		return GetUrisAsync(entry.link, cancellationToken);
	}

	public IAsyncEnumerable<Uri> GetUrisAsync(ICollection<linkType> links, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(links);
		ArgumentOutOfRangeException.ThrowIfZero(links.Count);
		var link = links.FirstOrDefault(l => string.Equals("application/x-zim", l.type, StringComparison.OrdinalIgnoreCase));
		return GetUrisAsync(link!, cancellationToken);
	}

	public IAsyncEnumerable<Uri> GetUrisAsync(linkType link, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(link);
		ArgumentException.ThrowIfNullOrWhiteSpace(link.href);
		if (Uri.TryCreate(link.href, UriKind.Absolute, out var uri))
		{
			return GetUrisAsync(uri, cancellationToken);
		}

		throw new ArgumentOutOfRangeException(nameof(link), link, $"The provided href '{link.href}' is not a valid absolute URI.");
	}

	public async IAsyncEnumerable<Uri> GetUrisAsync(Uri uri, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		static Uri parseuri(string s) => Uri.TryCreate(s, UriKind.Absolute, out var uri)
			? uri
			: throw new Exception();

		fileType file = await client.GetFileAsync(uri, cancellationToken);

		foreach (var u in file.url
			.OrderBy(o => o.priority)
			.Select(o => o.Value)
			.Select(parseuri))
		{
			yield return u;
		}
	}
}
