using Helpers.Zim.Models;

namespace Helpers.Zim.Services;

public interface IZimService
{
	IAsyncEnumerable<entryType> GetEntriesAsync(CancellationToken cancellationToken = default);
	IAsyncEnumerable<entryType> GetEntriesAsync(string name, string? flavor = null, CancellationToken cancellationToken = default);
	IAsyncEnumerable<Uri> GetUrisAsync(entryType entry, CancellationToken cancellationToken = default);
	IAsyncEnumerable<Uri> GetUrisAsync(Uri uri, CancellationToken cancellationToken = default);
}
