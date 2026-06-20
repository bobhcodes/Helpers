using Helpers.Zim.Clients;
using Helpers.Zim.Models;
using Helpers.Zim.Models.Generated;
using System.Runtime.CompilerServices;

namespace Helpers.Zim.Services.Concrete;

public class ZimService(IZimClient client) : IZimService
{
	public async IAsyncEnumerable<Entry> GetEntriesAsync(string name, string? flavor = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);

		await foreach (var entry in GetEntriesAsync(cancellationToken))
		{
			if (string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase))
			{
				if (string.IsNullOrEmpty(flavor)
					|| string.Equals(entry.Flavor, flavor, StringComparison.OrdinalIgnoreCase))
				{
					yield return entry;
				}
			}
		}
	}

	public IAsyncEnumerable<Entry> GetEntriesAsync(CancellationToken cancellationToken = default)
		=> client.GetEntriesAsync(cancellationToken);

	public IAsyncEnumerable<Uri> GetUrisAsync(Entry entry, CancellationToken cancellationToken = default)
	{
		return GetUrisAsync(entry.Link, cancellationToken);
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
