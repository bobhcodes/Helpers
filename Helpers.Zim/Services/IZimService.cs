using Helpers.Zim.Models;

namespace Helpers.Zim.Services;

public interface IZimService
{
	IAsyncEnumerable<Entry> GetEntriesAsync(CancellationToken cancellationToken = default);
	IAsyncEnumerable<Entry> GetEntriesAsync(string name, string? flavor = null, CancellationToken cancellationToken = default);
	IAsyncEnumerable<Uri> GetUrisAsync(Entry entry, CancellationToken cancellationToken = default);
	IAsyncEnumerable<Uri> GetUrisAsync(Uri uri, CancellationToken cancellationToken = default);
}
