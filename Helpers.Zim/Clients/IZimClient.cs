using Helpers.Zim.Models;
using Helpers.Zim.Models.Generated;

namespace Helpers.Zim.Clients;

public interface IZimClient
{
	IAsyncEnumerable<Entry> GetEntriesAsync(CancellationToken cancellationToken = default);
	Task<fileType> GetFileAsync(Uri uri, CancellationToken cancellationToken = default);
}
