using Helpers.Zim.Clients;
using Helpers.Zim.Models.Generated;
using System.Runtime.CompilerServices;

namespace Helpers.Zim.Services.Concrete;

public class ZimService(IZimClient client) : IZimService
{
	public async IAsyncEnumerable<bookType> GetBooksAsync(string name, string? flavor = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);

		await foreach (var book in GetBooksAsync(cancellationToken))
		{
			if (string.Equals(book.name, name, StringComparison.InvariantCultureIgnoreCase))
			{
				if (string.IsNullOrEmpty(flavor)
					|| string.Equals(book.flavour, flavor, StringComparison.OrdinalIgnoreCase))
				{
					yield return book;
				}
			}
		}
	}

	public IAsyncEnumerable<bookType> GetBooksAsync(CancellationToken cancellationToken = default)
		=> client.GetBooksAsync(cancellationToken);

	public IAsyncEnumerable<Uri> GetUrisAsync(bookType book, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(book);
		return GetUrisAsync(book.url, cancellationToken);
	}

	private IAsyncEnumerable<Uri> GetUrisAsync(string uriString, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(uriString);

		if (Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
		{
			return GetUrisAsync(uri, cancellationToken);
		}

		throw new ArgumentOutOfRangeException(nameof(uriString), uriString, uriString + " could not be parsed as a URI");
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
