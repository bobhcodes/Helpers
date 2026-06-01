using Helpers.Zim.Models.Generated;

namespace Helpers.Zim.Clients;

public interface IZimClient
{
	IAsyncEnumerable<bookType> GetBooksAsync(CancellationToken cancellationToken = default);
	Task<fileType> GetFileAsync(Uri uri, CancellationToken cancellationToken = default);
}
